using Amphenol.RMA.Models;
using Amphenol.RMA.Models.Enumerations;
using Amphenol.RMA.Models.ModelsM10;

namespace Amphenol.RMA.Extensions
{
    public static class CSEXSW_Rma_Extensions
    {
        public static void SetApprover(this CSEXSW_Rma rma, Approver approver)
        {
            rma.Approver = approver.Name;
            rma.res_id_approver = approver.Id;
        }
        public static void ChangeRequestStatus(this CSEXSW_Rma rma, RmaRequestStatus status)
        {
            rma.Status = status.ToDisplayString();
        }
    }
}
