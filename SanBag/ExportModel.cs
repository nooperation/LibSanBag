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
    public class ExportModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        FileRecord _currentRecord;
        public FileRecord CurrentRecord
        {
            get => _currentRecord;
            set
            {
                _currentRecord = value;
                OnPropertyChanged();
            }
        }

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        private float _minorProgress;
        public float MinorProgress
        {
            get => _minorProgress;
            set
            {
                _minorProgress = value;
                OnPropertyChanged();
            }
        }

        public uint _totalRead;
        public uint TotalRead
        {
            get => _totalRead;
            set
            {
                _totalRead = value;
                OnPropertyChanged();
            }
        }

        private List<FileRecord> RecordsToExport { get; set; }

        private void OnProgressReport(FileRecord record, uint bytesRead)
        {
            MinorProgress = 100.0f * ((float)bytesRead / record.Length);
            TotalRead = bytesRead;
        }

        public bool Export(List<FileRecord> recordsToExport, string bagPath, string outputDirectory, Func<bool> shouldCancel)
        {
            RecordsToExport = recordsToExport;
            var totalExported = 0;
            var exportSuccessful = true;

            using (var bagStream = File.OpenRead(bagPath))
            {
                foreach (var record in recordsToExport)
                {
                    if(shouldCancel != null && shouldCancel())
                    {
                        exportSuccessful = false;
                        break;
                    }

                    try
                    {
                        CurrentRecord = record;
                        var outputPath = Path.Combine(outputDirectory, record.Name);
                        using (var out_stream = File.OpenWrite(outputPath))
                        {
                            record.Save(bagStream, out_stream, OnProgressReport, shouldCancel);
                        }

                        ++totalExported;
                        Progress = 100.0f * (totalExported / (float)RecordsToExport.Count);
                    }
                    catch (Exception)
                    {
                        exportSuccessful = false;
                        continue;
                    }
                }
            }

            return exportSuccessful;
        }
    }
}
