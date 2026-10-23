using System;
using System.Collections.Generic;
using System.Linq;

namespace Amphenol.RMA.Models
{
    public static class RmaOrderGenerationPolicy
    {
        public static bool CanGenerateOrder(int requesterId, string username, string employeeId,
            int customerServiceManagerId, IEnumerable<string> approvalBypassUsers,
            int customerServiceSupervisorId = 0)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            var hasEmployeeId = int.TryParse(employeeId, out var id) && id > 0;
            return (hasEmployeeId && requesterId > 0 && id == requesterId)
                || (hasEmployeeId && customerServiceManagerId > 0 && id == customerServiceManagerId)
                || (hasEmployeeId && customerServiceSupervisorId > 0 && id == customerServiceSupervisorId)
                || (approvalBypassUsers?.Any(user => !string.IsNullOrWhiteSpace(user)
                    && string.Equals(user.Trim(), username.Trim(), StringComparison.OrdinalIgnoreCase)) ?? false);
        }
    }
}
