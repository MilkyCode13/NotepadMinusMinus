namespace NotepadMinusMinus
{
    /// <summary>
    /// The file service interface, provides opening and saving of objects into files.
    /// </summary>
    /// <typeparam name="T">The object stored in files.</typeparam>
    public interface IFileService<T>
    {
        /// <summary>
        /// Gets the object from file.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <returns>Object retrieved from file.</returns>
        T Open(string filename);

        /// <summary>
        /// Saves the object to the file.
        /// </summary>
        /// <param name="filename">The filename to save.</param>
        /// <param name="item">The object to save.</param>
        void Save(string filename, T item);
    }
}