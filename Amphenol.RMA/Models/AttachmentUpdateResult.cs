using System.Collections.Generic;

namespace Amphenol.RMA.Models
{
    public sealed class AttachmentUpdateResult
    {
        public List<string> FilesToDelete { get; } = [];
        public List<string> NewlyCreatedFiles { get; } = [];
    }
}
