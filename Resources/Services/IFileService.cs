namespace Resources.Services
{
    public interface IFileService
    {
        string GetFromFile();
        bool SaveToFile(string content);
    }
}