using files.Models;
using Newtonsoft.Json;

namespace files.Utilities
{
    public class FileSystemUtility
    {
        public static void SearchData(IEnumerable<FileItem> data, string query, List<object> result)
        {
            try
            {
                foreach (var item in data)
                {
                    if (item.Name != null && item.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(item.Name);
                    }

                    if (item.Files != null)
                    {
                        foreach (var file in item.Files)
                        {
                            if (file != null && file.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                            {
                                result.Add(file);
                            }
                        }
                    }

                    if (item.Directories != null)
                    {
                        foreach (var subdir in item.Directories)
                        {
                            SearchData(subdir, query, result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching.", ex);
            }
        }

        public static List<FileItem> ReadJsonFromFile(string filePath)
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(filePath);
                List<FileItem> data = JsonConvert.DeserializeObject<List<FileItem>>(jsonContent);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading JSON file.", ex);
            }
        }
    }
}
