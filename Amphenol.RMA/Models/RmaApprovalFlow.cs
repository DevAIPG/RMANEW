using Amphenol.RMA.Models.ModelsM10;
using System.Collections.Generic;

namespace Amphenol.RMA.Models
{
    public class RmaApprovalFlow
    {
        public Approver Approver { get; set; }
        public Approver FinalApprover { get; set; }
        public List<string> NotifyEmails { get; set; } = [];

    }
}
