using LibSanBag;
using Microsoft.Win32;
using SanBag.Commands;
using SanBag.Models;
using SanBag.ResourceUtils;
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
            get => _records.FindAll(CurrentView.Filter);
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        private string _recordNameFilter = string.Empty;
        public string RecordNameFilter
        {
            get => _recordNameFilter;
            set
            {
                _recordNameFilter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Records));
            }
        }

        public List<ViewType> Views { get; set; } = new List<ViewType>();

        private ViewType _currentView;
        public ViewType CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Records));
            }
        }

        private bool RecordPassesNameFilter(FileRecord record)
        {
            return record.Name.IndexOf(RecordNameFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public MainViewModel()
        {
            CommandOpenBag = new CommandOpenBag(this);

            Views.Add(new ViewType()
            {
                View = new GenericBagView()
                {
                    DataContext = new GenericBagViewModel(this)
                },
                Filter = (record => RecordPassesNameFilter(record)),
                Name = "Default"
            });

            if (LibDDS.IsAvailable && OodleLz.IsAvailable)
            {
                Views.Add(new ViewType()
                {
                    View = new TextureResourceView()
                    {
                        DataContext = new TextureResourceViewModel(this)
                    },
                    Filter = (record => RecordPassesNameFilter(record) && record.Info.Resource == FileRecordInfo.ResourceType.TextureResource),
                    Name = "TextureResource"
                });
            }

            CurrentView = Views[0];
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
