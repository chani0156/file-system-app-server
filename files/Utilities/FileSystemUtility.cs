using Newtonsoft.Json.Linq;

namespace files.Utilities
{
    public class FileSystemUtility
    {
        public static object ConvertNode(JToken jsonNode)
        {
            string label = jsonNode["name"]?.ToString();
            List<object> children = new List<object>();
            string expandedIcon = null;
            string collapsedIcon = null;
            if (jsonNode["directories"] is JArray directories)
            {
                expandedIcon = "pi pi-folder-open";
                collapsedIcon = "pi pi-folder";
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
                    children.Add(new { label = file.ToString(), icon = "pi pi-file" });
                }
            }

            return new { label, expandedIcon, collapsedIcon, children };
        }
        public static void SearchData(IEnumerable<object> data, string query, List<object> result)
        {
            foreach (var item in data)
            {
                if (item is { } labeledItem && labeledItem.GetType().GetProperty("label") is { } labelProperty)
                {
                    string label = labelProperty.GetValue(labeledItem).ToString();

                    if (label.StartsWith(query, StringComparison.OrdinalIgnoreCase))
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
    }
}
