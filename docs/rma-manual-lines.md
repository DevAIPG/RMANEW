# RMA lines without invoices

Create and Edit provide a **No invoice** checkbox on each line. A request can
mix invoice-linked and manual lines. Manual lines display Not applicable for
invoice and sequence, keep part numbers read-only and enable the existing ERP part selector,
and require the user to enter quantity, price, cost, location, action, and return
code. Manual lines may use zero for both price and unit cost. Negative amounts
remain invalid; for other amount combinations, price must exceed unit cost.
Invoice-linked lines retain their positive price/cost requirements. Price and cost
accept decimal values without a fixed browser step so existing invoice costs
with more than four decimal places remain valid. Field validation messages use
the indexed line names and appear beside the affected input. Zero-total
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
Switch each type in both directions and check that prior values clear. Confirm manual part numbers cannot be typed and must be selected from ERP.
Unknown parts posted directly must still fail server validation with no save. Submit and approve
a valid manual line on a test server; verify ERP receives invoice 0 and sequence 0.

Browser regression scenarios and JavaScript syntax checks passed in the Node runtime.
The .NET tests could not run on this workstation because the SDK is not installed.

Create and Edit load the unobtrusive validation adapter before RmaRequest.js.
Adding a line disposes the previous validator before parsing the updated form
and restores each line's invoice-mode rules. If the adapter is unavailable,
the refresh preserves existing validation and still initializes the line UI.

Part numbers remove trailing ERP padding before browser length validation and
before server model validation. Leading and embedded characters are preserved;
genuine part numbers over 20 characters still fail validation.

Price and unit cost use inline validation after leaving either field, with no
confirmation popup. Once shown, errors update as the values are corrected. Save
validates every line and scrolls to/focuses the first invalid field. Zero/zero
remains valid for No invoice lines; changing invoice mode clears amount errors.

Each editable line has a controls row followed by a warning row with matching column cells.
Each field message wraps beneath its corresponding control without changing
control alignment.
Validation, totals, and line counts use only controls rows; deleting a line
removes its warning row as well. Server validation still uses indexed field names.
