using LibSanBag;
using SanBag.Commands;
using SanBag.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static LibSanBag.FileResources.ManifestResource;

namespace SanBag.ViewModels
{
    public class ExportManifestViewModel : INotifyPropertyChanged
    {
        private CancellationTokenSource ExportCancellationTokenSource { get; set; } = null;

        public CommandManifestCancelExport CommandManifestCancelExport { get; set; }
        public string OutputDirectory { get; set; }
        public string BagPath { get; set; }
        public List<ManifestEntry> RecordsToExport { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ManifestEntry _currentRecord;
        public ManifestEntry CurrentRecord
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

        private bool _isRunning = true;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        //public Action<ExportParameters> CustomSaveFunc { get; set; }
        public string FileExtension { get; set; }

        public ExportManifestViewModel()
        {
            CommandManifestCancelExport = new CommandManifestCancelExport(this);
        }

        public void CancelExport()
        {
            if (ExportCancellationTokenSource != null)
            {
                ExportCancellationTokenSource.Cancel();
            }
        }

        private void OnProgressReport(FileRecord record, uint bytesRead)
        {
            MinorProgress = 100.0f * ((float)bytesRead / record.Length);
            TotalRead = bytesRead;
        }

        public bool Export(List<ManifestEntry> recordsToExport, string outputDirectory, Func<bool> shouldCancel)
        {
            RecordsToExport = recordsToExport;
            var totalExported = 0;
            var exportSuccessful = true;

            foreach (var record in recordsToExport)
            {
                if (shouldCancel != null && shouldCancel())
                {
                    exportSuccessful = false;
                    break;
                }

                try
                {
                    CurrentRecord = record;
                    var payloadTypes = new List<string>{ "payload", "manifest" };
                    var assetType = FileRecordInfo.GetResourceType(record.Name);
                    var assetVersions = AssetVersions.GetResourceVersions(assetType);

                    foreach (var payloadType in payloadTypes)
                    {
                        var downloadResult = DownloadResourceAsync(record.HashString.ToLower(), assetType, payloadType).Result;
                        if (downloadResult == null)
                        {
                            MessageBox.Show("Failed");
                            break;
                        }

                        var outputPath = Path.Combine(outputDirectory, downloadResult.Item1);

                        using (var out_stream = File.OpenWrite(outputPath))
                        {
                            out_stream.Write(downloadResult.Item2, 0, downloadResult.Item2.Length);
                        }
                    }

                    ++totalExported;
                    Progress = 100.0f * (totalExported / (float)RecordsToExport.Count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export '{record.Name}'\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    exportSuccessful = false;
                    continue;
                }
            }

            return exportSuccessful;
        }

        private bool ShouldCancel()
        {
            return ExportCancellationTokenSource.IsCancellationRequested;
        }

        public async Task StartAsync()
        {
            IsRunning = true;
            ExportCancellationTokenSource = new CancellationTokenSource();
            var taskWasSuccessful = false;

            await Task.Run(() =>
            {
                try
                {
                    taskWasSuccessful = Export(RecordsToExport, OutputDirectory, ShouldCancel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export: {ex.Message}");
                }
            }, ExportCancellationTokenSource.Token);

            if (taskWasSuccessful)
            {
                foreach (var item in Application.Current.Windows)
                {
                    var window = item as Window;
                    if (window != null && window.DataContext == this)
                    {
                        window.Close();
                        break;
                    }
                }
                MessageBox.Show($"Successfully exported {RecordsToExport.Count} record(s) to {OutputDirectory}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            IsRunning = false;
        }

        private async static Task<Tuple<string, byte[]>> DownloadResourceAsync(string resourceId, FileRecordInfo.ResourceType resourceType, string payloadType)
        {
            using (var client = new HttpClient())
            {
                var resourceTypeName = FileRecordInfo.GetResourceTypeName(resourceType);
                var versions = AssetVersions.GetResourceVersions(resourceType);
                for (int i = 0; i < versions.Count; i++)
                {
                    try
                    {
                        var itemName = $"{ resourceId }.{ resourceTypeName}.v{ versions[i].ToLower()}.{payloadType}.v0.noVariants";
                        var address = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{itemName}";
                        var bytes = await client.GetByteArrayAsync(address);
                        return Tuple.Create(itemName, bytes);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return null;
        }
    }
}
