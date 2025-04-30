namespace Common.Extensions;

public static class CreateFolder
{
    public static string CreateDirectoryForFile(string path, DateTime date)
    {
        try
        {
            string newPath = path + date.Year + @"\" + date.Month + @"\" + date.Day + @"\";
            path = Path.Combine(path, newPath);
            if (Directory.Exists(path))
            {
                return path;
            }
            Directory.CreateDirectory(path);
            return path;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }
}
