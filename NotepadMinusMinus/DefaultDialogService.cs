using System.Windows;
using Microsoft.Win32;

namespace NotepadMinusMinus
{
    /// <summary>
    /// The default dialog service.
    /// </summary>
    public class DefaultDialogService : IDialogService
    {
        /// <summary>
        /// Gets or sets the file path used by open and save dialogs.
        /// </summary>
        public string? FilePath { get; set; }
        
        /// <summary>
        /// Shows open dialog.
        /// </summary>
        /// <returns>Whether the OK button was selected.</returns>
        public bool OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Shows save dialog.
        /// </summary>
        /// <returns>Whether the OK button was selected.</returns>
        public bool SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}