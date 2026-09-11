namespace Amphenol.RMA.ViewModels
{
    public class InternalEmailNotificationViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Message { get; set; }
        public string ActionText { get; set; }
        public string ActionUrl { get; set; }
        public bool HasAction => !string.IsNullOrWhiteSpace(ActionText) && !string.IsNullOrWhiteSpace(ActionUrl);
    }
}
