using Newtonsoft.Json.Linq;

namespace files.Utilities
{
    public class FileSystemUtility
    {
        public object ConvertNode(JToken jsonNode)
        {
            string label = jsonNode["name"]?.ToString();
            List<object> children = new List<object>();

            if (jsonNode["directories"] is JArray directories)
            {
                foreach (var subdir in directories)
                {
                    var childNode = ConvertNode(subdir[0]);
                    children.Add(childNode);
                }
            }

            if (jsonNode["files"] is JArray files)
            {
                foreach (var file in files)
                {
                    children.Add(new { label = file.ToString(), icon = Constants.Constants.FileIcon });
                }
            }

            return new { label, expandedIcon = Constants.Constants.ExpandedIcon, collapsedIcon = Constants.Constants.CollapsedIcon, children };
        }

        /// <summary>
        /// Searches data for items starting with the specified query.
        /// </summary>
        public void SearchData(IEnumerable<object> data, string query, List<object> result)
        {
            foreach (var item in data)
            {
                if (item is { } labeledItem && labeledItem.GetType().GetProperty("label") is { } labelProperty)
                {
                    string label = labelProperty.GetValue(labeledItem)?.ToString();

                    if (!string.IsNullOrEmpty(label) && label.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(item);
                    }
                }

                if (item is { } childrenItem && childrenItem.GetType().GetProperty("children") is { } childrenProperty)
                {
                    var children = (List<object>)childrenProperty.GetValue(childrenItem);
                    SearchData(children, query, result);
                }
            }
        }

        public List<object> GetDataFromSource()
        {       
                string jsonFilePath = Path.Combine(AppContext.BaseDirectory, Constants.Constants.JsonFileName);

                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException($"File '{Constants.Constants.JsonFileName}' not found at '{jsonFilePath}'.");
                }

                string jsonContent = File.ReadAllText(jsonFilePath);
                JArray jsonNodes = JArray.Parse(jsonContent);

                List<object> primeNgNodes = new List<object>();
                foreach (var jsonNode in jsonNodes)
                {
                    var primeNgNode = ConvertNode(jsonNode);
                    primeNgNodes.Add(primeNgNode);
                }

                return primeNgNodes;        
        }
    }
}
