using Microsoft.AspNetCore.Http;

namespace Amphenol.RMA.ViewModels
{
    public class RmaAttachmentViewModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public bool MarkedForDeletion { get; set; }
    }
}
