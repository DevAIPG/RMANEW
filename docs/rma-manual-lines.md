# RMA lines without invoices

Create and Edit provide a **No invoice** checkbox on each line. A request can
mix invoice-linked and manual lines. Manual lines display Not applicable for
invoice and sequence, enable part-number entry and the existing ERP part search,
and require the user to enter quantity, price, cost, location, action, and return
code. Manual lines may use zero for both price and unit cost. Negative amounts
remain invalid; for other amount combinations, price must exceed unit cost.
Invoice-linked lines retain their positive price/cost requirements. Zero-total
requests follow the existing lowest-value approval tier.

Switching modes clears invoice, sequence, part, quantity, price, and cost so data
from an earlier selection cannot carry into a different mode. Existing saved
manual lines retain their entered values when Edit initializes.

Create/Edit validate manual part numbers against ERP before saving. The server
always persists invoice "0" and sequence 0 when No invoice is selected, including
when a caller posts stale invoice references. The checkbox is a view-model field;
no database column or ERP schema change is needed. Saved blank/zero invoices are
recognized as manual lines when reopened. Invoice-linked lines require an invoice
and a sequence between 1 and 32767.

Browser behavior regression checks:

    node tests/rma-manual-lines.test.cjs

Server validation and persistence-mapping checks:

    dotnet test tests/Amphenol.RMA.ApprovalTests/Amphenol.RMA.ApprovalTests.csproj --filter FullyQualifiedName~RmaManualLineTests

Staging verification: create a request containing both line types; save and reopen;
confirm manual values remain editable and invoice lines retain their references.
Switch each type in both directions and check that prior values clear. Enter an
unknown manual part and confirm an inline error with no save. Submit and approve
a valid manual line on a test server; verify ERP receives invoice 0 and sequence 0.

Six browser scenarios and JavaScript syntax checks passed in the Node runtime.
The .NET tests could not run on this workstation because the SDK is not installed.
