namespace MongoDB.Testing
{
    public interface IFileSystem
    {
        string CreateTempFolder();
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void DeleteDirectory(string path, bool recursive);
    }
}