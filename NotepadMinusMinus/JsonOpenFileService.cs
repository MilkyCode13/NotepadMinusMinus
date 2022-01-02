using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NotepadMinusMinus
{
    /// <summary>
    /// Stores the list of text file tabs in file.
    /// </summary>
    public class JsonOpenFileService : IFileService<List<TextFileTab>>
    {
        /// <summary>
        /// Gets the list of tabs from the file.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <returns>The list of tabs retrieved from file.</returns>
        public List<TextFileTab> Open(string filename)
        {
            string jsonString = File.ReadAllText(filename);
            List<string> files = JsonSerializer.Deserialize<List<string>>(jsonString) ?? new List<string>();
            
            var service = new TextFileService();
            var tabs = new List<TextFileTab>();
            foreach (string file in files)
            {
                try
                {
                    tabs.Add(service.Open(file));
                }
                catch
                {
                    // ignored
                }
            }

            return tabs;
        }

        /// <summary>
        /// Saves the list of tabs to the file.
        /// </summary>
        /// <param name="filename">The filename to save.</param>
        /// <param name="item">The list of tabs to save.</param>
        public void Save(string filename, List<TextFileTab> item)
        {
            string jsonString = JsonSerializer.Serialize(item.Select(tab => tab.FilePath),
                new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(filename, jsonString);
        }
    }
}