using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace NotepadMinusMinus
{
    /// <summary>
    /// Represents the text file tab.
    /// </summary>
    public class TextFileTab : INotifyPropertyChanged
    {
        private string? filePath;

        private string text = string.Empty;

        private string title = "Untitled";

        private bool isChanged;
        
        private bool isNewTabButton;
        
        private string searchText = "";
        
        private RelayCommand? findCommand;

        /// <summary>
        /// Gets the object instance.
        /// </summary>
        public TextFileTab Self => this;

        /// <summary>
        /// Gets or sets the file path opened in the tab..
        /// </summary>
        public string? FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the title of the tab.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the text in the tab.
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (value != text)
                {
                    text = value;
                    IsChanged = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the text of the tab was changed.
        /// </summary>
        public bool IsChanged
        {
            get => isChanged;
            set
            {
                if (value != isChanged)
                {
                    isChanged = value;
                    Title = value ? Caption + '*' : Caption;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the caption of the tab.
        /// </summary>
        public string Caption => Path.GetFileName(filePath) ?? "Untitled";

        /// <summary>
        /// Gets or sets whether the tab is the new tab button.
        /// </summary>
        public bool IsNewTabButton
        {
            get => isNewTabButton;
            set
            {
                isNewTabButton = value;
                Title = value ? "+" : Caption;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CloseButtonVisibility));
            }
        }

        /// <summary>
        /// Gets the visibility of close button.
        /// </summary>
        public Visibility CloseButtonVisibility => IsNewTabButton ? Visibility.Collapsed : Visibility.Visible;
        
        /// <summary>
        /// Gets the "Find" command.
        /// </summary>
        public RelayCommand FindCommand
        {
            get
            {
                return findCommand ??= new RelayCommand(obj =>
                {
                    if (obj is not TextBox textBox)
                    {
                        return;
                    }

                    Find(SearchText, textBox);
                });
            }
        }

        /// <summary>
        /// Searches in the text.
        /// </summary>
        /// <param name="searchString">The string to search.</param>
        /// <param name="textBox">The text box to search in.</param>
        private void Find(string searchString, TextBox textBox)
        {
            int foundIndex = Text.IndexOf(searchString, textBox.CaretIndex + 1, StringComparison.Ordinal);
            if (foundIndex != -1)
            {
                textBox.CaretIndex = foundIndex;
                textBox.SelectionStart = foundIndex;
                textBox.SelectionLength = searchString.Length;
                textBox.Focus();
                return;
            }

            foundIndex = Text.IndexOf(searchString, StringComparison.Ordinal);
            if (foundIndex != -1)
            {
                textBox.CaretIndex = foundIndex;
                textBox.SelectionStart = foundIndex;
                textBox.SelectionLength = searchString.Length;
                textBox.Focus();
            }
        }
        
        /// <summary>
        /// Invokes when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="property">The property to pass in to the event.</param>
        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}