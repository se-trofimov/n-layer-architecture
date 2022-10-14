namespace CartingService.DataAccessLayer.ValueObjects
{
    public class Image
    {
        public Image(string? url, string? altText)
        {
            Url = url;
            AltText = altText;
        }

        public string? Url { get; }
        public string? AltText { get; }
    }
}
