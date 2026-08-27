namespace Amphenol.RMA.Models
{
    public class ReportDownloadDto
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
