namespace NotepadMinusMinus
{
    /// <summary>
    /// The dialog service interface.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Gets or sets the file path used by open and save dialogs.
        /// </summary>
        string? FilePath { get; set; }
        
        /// <summary>
        /// Shows open dialog.
        /// </summary>
        /// <returns>Whether the OK button was selected.</returns>
        bool OpenFileDialog();

        /// <summary>
        /// Shows save dialog.
        /// </summary>
        /// <returns>Whether the OK button was selected.</returns>
        bool SaveFileDialog();
        
        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message to show.</param>
        void ShowMessage(string message);
    }
}