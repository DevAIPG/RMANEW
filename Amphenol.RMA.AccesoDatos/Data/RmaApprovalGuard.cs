using Amphenol.RMA.Models;
using System;

namespace Amphenol.RMA.AccesoDatos.Data
{
    public static class RmaApprovalGuard
    {
        public static void EnsureEditable(CSEXSW_Rma request)
        {
            if (request == null)
                throw new InvalidOperationException("The RMA request was not found.");

            if (!string.IsNullOrWhiteSpace(request.turno)
                || string.Equals(request.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    "This request already has an approval or ERP RMA assignment and cannot be edited. Reconcile any incomplete approval before making changes.");

            if (!request.CanEdit)
                throw new InvalidOperationException(
                    "This request is waiting for approval and cannot be edited. An approver must return it for changes before editing and resubmitting.");
        }
    }
}
