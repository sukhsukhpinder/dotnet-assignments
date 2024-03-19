namespace RegistarationApp.Core.Models.Setting
{
    public class RepositorySettings
    {
        public string? RepositoryType { get; set; }
        public FileSystemSettings? FileSystemSettings { get; set; }
        public DatabaseSettings? DatabaseSettings { get; set; }
    }

    public class FileSystemSettings
    {
        public string? FilePath { get; set; }
    }
    public class DatabaseSettings
    {
        public string? DataAccessLayer { get; set; }
        public string? ConnectionString { get; set; }
    }
}
