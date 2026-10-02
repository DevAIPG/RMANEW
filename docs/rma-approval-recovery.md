# Atomic RMA approval across M10 and ERP

Approval validates the request and prepares the full ERP header, every line,
and any new inventory locations in memory before writing ERP data. It saves
the M10 approval first, then updates the ERP number counter and saves ERP records.
Both databases participate in one coordinated transaction. They commit together;
a validation error, M10 save error, ERP save error, or transaction abort rolls
back the approval and counter. No number is reserved or consumed separately.

A SQL session application lock in M10 serializes approvals for the same request
across app instances. The ERP counter row uses UPDLOCK, HOLDLOCK to serialize
different requests. Already-approved requests return without another RMA, CAR
job, or notification. Historical partial assignments require reconciliation.
First-stage approval still hands off to the GM without creating an ERP RMA.

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

## Validation before release

Build with the .NET 8 SDK:

    dotnet build Amphenol.RMA.sln

Use isolated M10/ERP clones and a Windows test host with working MSDTC. Set
RMA_TEST_M10 and RMA_TEST_ERP to the clone connection strings. Both database
names must end with _RmaApprovalTests. No schema scripts are needed.
Set RMA_TEST_ROLLBACK_ID, RMA_TEST_M10_FAILURE_ID,
RMA_TEST_VALIDATION_FAILURE_ID, and RMA_TEST_CONCURRENT_ID to four distinct
pending requests below $20,000 with valid approvers, lines, and ERP master data
(use USD fixtures). Confirm INVALID-TEST-ITEM is absent from ERP item master.
Run:

    dotnet test tests/Amphenol.RMA.ApprovalTests/Amphenol.RMA.ApprovalTests.csproj

The suite checks:
- Failure after all ERP inserts executed rolls back both databases and the counter.
- A failed M10 save causes no ERP SaveChanges calls and leaves ERP unchanged.
- An invalid later line prevents writes and counter movement.
- Concurrent approvals create one RMA and advance the counter once.

Tests modify the clones and consume fixtures; restore clones before rerunning.
Without connection variables, tests are explicitly skipped. They have not been
run on this workstation because it has .NET runtimes but no SDK.

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

Email delivery and CAR job execution occur after the database commit and are not
part of its transaction. A failure there cannot roll back an approved RMA. Failed
notifications need separate resend handling; a repeated approval intentionally
skips sending them again. A durable notification outbox would be a separate
improvement for automatic delivery retries.

No production database, MSDTC setting, or notification was changed while
preparing this patch.
