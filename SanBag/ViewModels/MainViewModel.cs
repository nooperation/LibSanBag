using LibSanBag;
using Microsoft.Win32;
using SanBag.Commands;
using SanBag.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SanBag.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public CommandOpenBag CommandOpenBag { get; set; }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private string _bagPath = string.Empty;
        public string BagPath
        {
            get => _bagPath;
            set
            {
                _bagPath = value;
                OnPropertyChanged();
            }
        }

        private List<FileRecord> _records = new List<FileRecord>();
        public List<FileRecord> Records
        {
            get => _records.FindAll(n => n.Name.Contains(RecordsFilter));
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        private string _recordsFilter = string.Empty;
        public string RecordsFilter
        {
            get => _recordsFilter;
            set
            {
                _recordsFilter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Records));
            }
        }

        public MainViewModel()
        {
            CommandOpenBag = new CommandOpenBag(this);
            CurrentView = new TextureResourceView()
            {
                DataContext = new TextureResourceViewModel(this)
            };
        }

        public void OnOpenFile()
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "Bag files|*.bag|All files|*.*"
                };
                if (dialog.ShowDialog() == true)
                {
                    OpenBag(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open bag: {ex.Message}");
            }
        }

        public void OpenBag(string path)
        {
            BagPath = path;

            using (var in_stream = File.OpenRead(path))
            {
                Records = Bag.Read(in_stream).ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
