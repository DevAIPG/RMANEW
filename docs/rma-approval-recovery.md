# Atomic RMA approval across M10 and ERP

Approval validates the request and prepares the full ERP header, every line,
and any new inventory locations in memory before writing ERP data.

Customer names are copied without trimming or truncation. The `bill_to_name`
and `ship_to_name` model limits are both 40, preserving the original ERP header
field limits. Names with more than 40 non-padding characters fail validation
before ERP writes. This application change does not alter ERP column sizes or
existing data. Length validation reads EF model metadata, not live SQL
column sizes. Rebuild the solution and restart/redeploy the application after
changing model annotations so it loads the updated Models assembly and EF model.
Length validation ignores trailing ASCII space padding without changing the
original value. This permits a three-character manufacturing location supplied
from the four-character customer address field with one trailing space. Actual
four-character codes remain invalid for the three-character `mfg_loc` column.
Leading spaces, tabs, and other whitespace still count toward the field limit.

It saves the M10 approval first, then updates the ERP number counter and saves ERP records.
Both databases participate in one coordinated transaction. They commit together;
a validation error, M10 save error, ERP save error, or transaction abort rolls
back the approval and counter. No number is reserved or consumed separately.

A SQL session application lock in M10 serializes approvals for the same request
across app instances. The ERP counter row uses UPDLOCK, HOLDLOCK to serialize
different requests. Already-approved requests return without another RMA, CAR
job, or notification. Historical partial assignments require reconciliation.
First-stage approval still hands off to the GM without creating an ERP RMA.
The current Edit POST and legacy repository Update use the same request lock
and reload M10 state before changing fields. Approved requests and partial ERP
assignments cannot be edited. Update cannot accept an approval status or ERP
number from its caller, and never overwrites `turno`. A stale Done/Edit submission
therefore cannot clear an assignment and enable a second RMA.

Editing is limited to the requester while the request is a draft (`Pending` /
`Not Submitted`) or returned for changes (`Remark` / `Rejected` with no ERP
assignment). Submitted pending requests are locked through the director and GM
stages. Automatic approvals and finalized requests are also locked. The shared
`CanEdit` policy drives the request list and server guards; it is not a database
column. Approval itself requires a submitted pending request or an eligible
internal automatic approval, so drafts and returned requests must be submitted
again before approval.

Edit GET/POST and the legacy header, line, attachment, and deletion endpoints
enforce the policy and requester ownership while holding the request lock.
Approvers can return or reject a pending request; they cannot edit its content.
Return/reject endpoints verify the assigned approver or configured approval
bypass account. Resubmission locks editing again. Queued legacy line/attachment
edits recheck the saved state and fail without writing when the request is no
longer editable. Legacy Done writes content before marking the request submitted
in the same M10 transaction, and Saveedit preserves workflow state and writes
its lines synchronously rather than queuing changes after submission.

The public Aprobar POST accepts only the comment and request ID and resolves
the approver from the signed-in user. Automatic approval is a private helper
called by Create/Edit after the saved request is assigned to System and has
AutoApprove status. A posted `isAutoApproved` parameter cannot select System.
Create/Edit dispose their local M10 transaction before automatic approval begins.

Users listed in `ApprovalBypass.Users` can approve a request assigned to someone
else, including its director stage. The repository verifies that list against
the employee's M10 `usr_id`; bypass approval still follows the two-stage handoff,
transaction, and duplicate-approval checks.

The controller runs CAR enqueue, report generation, and approval notifications
only after Updateaprobar returns Approved. The transaction scope is disposed and
its commit completes before that return reaches the controller. A failed commit
throws and cannot enter the notification path.

No new tables or schema scripts are required in either database. The reservation
model and earlier table-creation script have been removed from this branch.
Existing RMAs 601758–601761 have not been modified. Keep 601761 active while CS
handles the earlier duplicates separately.

## Deployment requirement

The existing two EF contexts use separate SQL connections. Coordinating them
uses System.Transactions and can promote to a distributed transaction. This
implementation requires Windows and working MSDTC on the application/SQL hosts.
It opts into distributed transactions and explicitly enlists both connections
before any writes. If the coordinator is unavailable, approval fails without
falling back to independent commits.

Verify MSDTC service availability, security configuration, authentication, and
required network access with the server administrator in staging before release.
The repository connection settings point to the same SQL instance, but deployed
connection overrides must also be checked. Do not assume that sharing an instance
avoids transaction promotion when two connections are enlisted.

References:
- [EF Core transactions](https://learn.microsoft.com/en-us/ef/core/saving/transactions)
- [SQL Server distributed transactions](https://learn.microsoft.com/en-us/sql/connect/ado-net/distributed-transactions)

Deploy all application instances together; older versions do not participate in
this approval locking and transaction protocol. Review historical Hangfire
CSEXSW_RmaRepository.falloRMA jobs: that method no longer recreates ERP records
or overwrites M10 assignments. It reports manual reconciliation without retry.
The legacy OERDTFIL_SQLRepository methods `nuevorma`, `lineascrear`, `lineas`,
and `falloRMA` are also disabled before any database access. Their signatures
remain for old callers and serialized jobs; the creation/recovery job methods
have automatic retries disabled. Review jobs targeting either repository,
including jobs already running on an older worker, before deployment.

## Validation before release

Build with the .NET 8 SDK:

    dotnet build Amphenol.RMA.sln

Use the test M10/ERP databases and a Windows test host with working MSDTC.
Integration tests read the application's `ConnectionStrings:ConnectionM10`
and `ConnectionStrings:Connection500` settings. Each connection must
target `AIO-POS` or `M10TestNA01` (case-insensitive); those are the approved test
servers. Named instances and TCP ports on those hosts are accepted. Database
names remain unchanged; no `_RmaApprovalTests` suffix is required. Other server
names, IP addresses, and unlisted fully qualified names are rejected even if
the database has the old suffix. No schema scripts are needed.

The application `appsettings*.json` files are copied into the test output's
`ApplicationSettings` directory at build time. Tests load the base settings,
then settings for `DOTNET_ENVIRONMENT` / `ASPNETCORE_ENVIRONMENT` (default:
Development), development user secrets from the application assembly, and
environment variable overrides. The application and its background services
are not started to load these settings. Both database destinations are validated
before either test connection can be used. Missing settings or an unapproved
server fail the tests rather than silently skipping them.

No `RMA_TEST_M10` or `RMA_TEST_ERP` variables are needed. If the application
settings already point to the approved test servers, use those settings directly.
To override the connections for a test run without editing application files:

```powershell
$env:ConnectionStrings__ConnectionM10 = 'Server=<APPROVED_TEST_SERVER>;Database=<M10_DATABASE>;Integrated Security=True;'
$env:ConnectionStrings__Connection500 = 'Server=<APPROVED_TEST_SERVER>;Database=<ERP_DATABASE>;Integrated Security=True;'
```

Replace each placeholder with the correct test server/database and use the
authentication settings appropriate for those servers.
Set RMA_TEST_ROLLBACK_ID, RMA_TEST_M10_FAILURE_ID,
RMA_TEST_VALIDATION_FAILURE_ID, and RMA_TEST_CONCURRENT_ID to four distinct
pending requests below $20,000 with valid approvers, lines, and ERP master data
(use USD fixtures). Confirm INVALID-TEST-ITEM is absent from ERP item master.
Also set `RMA_TEST_BYPASS_ID` to another pending USD request below $20,000 and
`RMA_TEST_BYPASS_DIRECTOR_ID` to a pending USD request at or above $20,000 assigned
to the quality director. These fixtures must have valid ERP data and employee
accounts other than their assigned approvers, with nonblank `usr_id` values;
the director fixture also needs the configured director and GM roles.
Also set `RMA_TEST_EDIT_PROTECTION_ID` to a separate valid pending USD request
below $20,000. The test retains a stale copy of that request while another context
approves it, then attempts to reset its assignment through legacy Update.
Set `RMA_TEST_EDIT_LIFECYCLE_ID` to another distinct submitted pending USD request
below $20,000 to check pending edit protection, return for changes, editing,
blocked draft/returned approvals, resubmission, and line/attachment protection.
Run:

    dotnet test tests/Amphenol.RMA.ApprovalTests/Amphenol.RMA.ApprovalTests.csproj

The suite checks:
- Failure after all ERP inserts executed rolls back both databases and the counter.
- A failed M10 save causes no ERP SaveChanges calls and leaves ERP unchanged.
- An invalid later line prevents writes and counter movement.
- Concurrent approvals create one RMA and advance the counter once.
- A stale legacy edit cannot erase a committed approval or enable another RMA;
  an update cannot manufacture an approval or ERP number on a pending request.
- Submitted requests cannot be edited; returned requests can be changed until
  resubmission. Header, queued line, and attachment writes recheck this state.
- An unconfigured non-approver is denied; a configured bypass user can approve
  the request and repeat the action without generating another RMA.
- A configured bypass user can act at both director and GM stages while retaining
  the handoff between them.

Tests modify the clones and consume fixtures; restore clones before rerunning.
Integration tests no longer skip based on connection environment variables.
They have not been run on this workstation because it has .NET runtimes but
no SDK. Fixture request IDs are still required for the data-changing tests.

### Diagnosing fixture setup failures

The latest configuration change enables the eight SQL tests that previously
skipped; a missing fixture is a setup failure before approval runs. Each SQL
test now prints the selected M10 and ERP server/database names without exposing
credentials. Fixture variables refer to the internal `CSEXSW_Rma.Id`, not the
displayed `Rmarequest` or ERP RMA number. If a fixture was selected on the other
test server, change the test connection override or select a request in the
database actually shown in test output.

To find candidate fixtures, run this read-only query in the selected M10 test
database. Confirm the customer's currency is USD and ERP master data is valid
before choosing a fixture. Use different fresh IDs for each test.

```sql
SELECT TOP (20) r.Id, r.Rmarequest, r.Customer, r.Status, r.Sumbit,
    r.turno, r.Totalrmavalues, r.res_id_approver
FROM dbo.CSEXSW_Rma AS r
WHERE r.Status = 'Pending'
    AND r.Sumbit = 'Submitted'
    AND (r.turno IS NULL OR LTRIM(RTRIM(r.turno)) = '')
    AND r.Totalrmavalues >= 0 AND r.Totalrmavalues < 20000
    AND r.res_id_approver > 0
    AND EXISTS (SELECT 1 FROM dbo.csexsw_coustumer AS l WHERE l.RmaId = r.Id)
ORDER BY r.Id DESC;
```

Set `RMA_TEST_CONCURRENT_ID` to one of those internal IDs and rerun the concurrency
test. A successful run approves the fixture; restore the test data or choose a
different fresh request before rerunning. Missing requests, already-approved
fixtures, absent lines, and missing ERP counter setup now produce explicit
messages rather than an unexplained `Sequence contains no elements` error.

`ApprovalProtectionTests` additionally checks finalized/partial assignment
protection, the public approval action's identity parameters, and legacy writers
failing without opening SQL connections. These tests do not require database
connections:

    dotnet test tests/Amphenol.RMA.ApprovalTests/Amphenol.RMA.ApprovalTests.csproj --filter FullyQualifiedName~ApprovalProtectionTests

Additional staging checks:
- A missing inventory location is created with the complete ERP RMA, and repeated
  lines at that location do not generate duplicate inventory records.
- Two-stage approval hands off once; replaying the director POST does not approve
  the GM stage or send another handoff notification.
- Different requests approved concurrently receive distinct numbers.
- Replaying an approved request, including 601761, creates no records or notifications.
- Legacy partial assignments are blocked for reconciliation.
- Failed transaction enlistment or commit produces no approval notifications.
- Report/email failure after commit never generates another RMA on retry.
- A stale current Edit POST and a legacy Done POST cannot change an approved
  request, its lines, or its assigned ERP number.
- Posting `isAutoApproved=true` as a non-approver does not grant approval rights;
  eligible Create/Edit automatic approvals still succeed after local commit.

Email delivery and CAR job execution occur after the database commit and are not
part of its transaction. A failure there cannot roll back an approved RMA. Failed
notifications need separate resend handling; a repeated approval intentionally
skips sending them again. A durable notification outbox would be a separate
improvement for automatic delivery retries.

No production database, MSDTC setting, or notification was changed while
preparing this patch.
