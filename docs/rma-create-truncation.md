# RMA creation: SQL error 8152

An error at RmaController.Create's first SaveChangesAsync occurs while inserting
CSEXSW_Rma, before request numbering, lines, attachments, or notifications.
Error 8152 does not identify the offending column.

The deployed schema was confirmed as:
- Customercomplait: nvarchar(max), with no complaint length restriction.
- Description: nvarchar(29).
- reason: nvarchar(100).

RmaViewModel validates description at 29 characters and SelectedReason at 100.
The entity's reason property also declares the 100-character limit. Complaint
remains required and unrestricted in length. The historical model snapshot's
25-character complaint mapping does not reflect the confirmed deployed schema.

Other bounded user-entered header fields are validated against the entity limits:
PO (25), contact (40), phone/fax (20), extension (4), customer (8), email addresses
(128), ship-to (64), build location (8), and request type (64).

MVC returns validation errors before inserting oversized header values and keeps
the entered text in the form. No submitted text is silently shortened. These checks
also apply when using the view model to edit. No database changes or migrations
are required or executed.

The exact offending column in the original request remains unconfirmed. If
truncation persists with valid inputs, compare the remaining deployed limits
with submitted values and inspect database triggers. Requester and approver
names are populated by the server and have a mapped limit of 64, so they are
not covered by input validation.
