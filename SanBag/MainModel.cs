using LibSanBag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SanBag
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                OnPropertyChanged(nameof(Records));
            }
        }

        public string BagPath { get; set; }

        public void OpenBag(string path)
        {
            using (var in_stream = File.OpenRead(path))
            {
                Records = Bag.Read(in_stream).ToList();
            }

            BagPath = path;
        }
    }
}
