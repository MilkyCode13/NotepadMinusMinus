using System.IO;

namespace NotepadMinusMinus
{
    /// <summary>
    /// Stores the text file.
    /// </summary>
    public class TextFileService : IFileService<TextFileTab>
    {
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <returns>The text file tab with the file.</returns>
        public TextFileTab Open(string filename)
        {
            return new TextFileTab
            {
                FilePath = filename,
                Text = File.ReadAllText(filename),
                IsChanged = false
            };
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="filename">The filename to save.</param>
        /// <param name="item">The text file tab to save.</param>
        public void Save(string filename, TextFileTab item)
        {
            File.WriteAllText(filename, item.Text);
            item.FilePath = filename;
            item.IsChanged = false;
        }
    }
}