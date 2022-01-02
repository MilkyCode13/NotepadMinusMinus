using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NotepadMinusMinus
{
    /// <summary>
    /// Contains the view model for the main window.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TextFileTab? selectedTab;

        private readonly string? settingsPath;

        private readonly JsonOpenFileService jsonOpenFileService = new();

        private readonly IFileService<TextFileTab> fileService = new TextFileService();

        private readonly IDialogService dialogService = new DefaultDialogService();

        private RelayCommand? newCommand;

        private RelayCommand? openCommand;
        
        private RelayCommand? saveCommand;
        
        private RelayCommand? closeCommand;

        /// <summary>
        /// Constructs an instance of view model.
        /// </summary>
        public MainWindowViewModel()
        {
            try
            {
                settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "NotepadMinusMinus");
                Directory.CreateDirectory(settingsPath);
                foreach (TextFileTab tab in jsonOpenFileService.Open(Path.Combine(settingsPath, "openFiles.json")))
                {
                    Tabs.Add(tab);
                }
            }
            catch
            {
                // ignored
            }

            if (Tabs.Count == 0)
            {
                Tabs.Add(new TextFileTab());
            }
            
            Tabs.Add(new TextFileTab {IsNewTabButton = true});
        }

        /// <summary>
        /// Gets or sets the collection of tabs.
        /// </summary>
        public ObservableCollection<TextFileTab> Tabs { get; set; } = new();

        /// <summary>
        /// Gets the "New" command.
        /// </summary>
        public RelayCommand NewCommand
        {
            get
            {
                return newCommand ??= new RelayCommand(_ =>
                {
                    TextFileTab tab = new TextFileTab();
                    Tabs.Insert(Tabs.Count - 1, tab);
                    SelectedTab = tab;
                });
            }
        }
        
        /// <summary>
        /// Gets the "Open" command.
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??= new RelayCommand(_ =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog() && dialogService.FilePath != null)
                        {
                            TextFileTab tab = fileService.Open(dialogService.FilePath);
                            Tabs.Insert(Tabs.Count - 1, tab);
                            SelectedTab = tab;
                        }
                    }
                    catch (Exception e)
                    {
                        dialogService.ShowMessage(e.Message);
                    }
                });
            }
        }
        
        /// <summary>
        /// Gets the "Save" command.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??= new RelayCommand(obj =>
                {
                    if (SelectedTab == null)
                    {
                        return;
                    }

                    if (obj is not bool saveAs)
                    {
                        saveAs = false;
                    }
                    try
                    {
                        if ((saveAs || SelectedTab.FilePath == null) && dialogService.SaveFileDialog() &&
                            dialogService.FilePath != null)
                        {
                            SelectedTab.FilePath = dialogService.FilePath;
                        }
                        if (SelectedTab.FilePath != null)
                        {
                            fileService.Save(SelectedTab.FilePath, SelectedTab);
                        }
                    }
                    catch (Exception e)
                    {
                        dialogService.ShowMessage(e.Message);
                    }
                }, _ => SelectedTab != null);
            }
        }
        
        /// <summary>
        /// Gets the "Close" command.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ??= new RelayCommand(obj  =>
                {
                    if (obj is TextFileTab tab)
                    {
                        if (Tabs.IndexOf(tab) == Tabs.Count - 2 && Tabs.Count > 2)
                        {
                            SelectedTab = Tabs[^3];
                        }
                        Tabs.Remove(tab);
                    }
                }, _ => Tabs.Count > 0);
            }
        }

        /// <summary>
        /// Gets the "Window Exiting" command.
        /// </summary>
        public RelayCommand ExitingCommand
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    try
                    {
                        if (settingsPath != null)
                        {
                            jsonOpenFileService.Save(Path.Combine(settingsPath, "openFiles.json"),
                                Tabs.SkipLast(1).ToList());
                        }
                    }
                    catch (Exception e)
                    {
                        dialogService.ShowMessage(e.Message);
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets the selected tab.
        /// </summary>
        public TextFileTab? SelectedTab
        {
            get => selectedTab;
            set
            {
                selectedTab = value;
                if (selectedTab is {IsNewTabButton: true})
                {
                    selectedTab.IsNewTabButton = false;
                    Tabs.Add(new TextFileTab {IsNewTabButton = true});
                }
                OnPropertyChanged();
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