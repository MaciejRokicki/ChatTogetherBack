namespace ChatTogether.Commons.ConfigurationModels
{
    public class StaticFilesConfiguration
    {
        public string Path { get; set; }
        public string[] AllowedFiles { get; set; }
        public long MaxFileSize { get; set; }
    }
}
