using LibSanBag;
using Microsoft.Win32;
using SanBag.Commands;
using SanBag.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SanBag.ViewModels
{
    public class GenericBagViewModel
    {
        public MainViewModel ParentViewModel { get; set; }
        public CommandExportSelected CommandExportSelected { get; set; }
        public CommandCopyAsUrl CommandCopyAsUrl { get; set; }
        public string ExportFilter { get; set; }

        public GenericBagViewModel(MainViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            this.CommandExportSelected = new CommandExportSelected(this);
            this.CommandCopyAsUrl = new CommandCopyAsUrl(this);
            this.ExportFilter = "Raw File|*.*";
        }

        public virtual bool IsValidRecord(FileRecord record)
        {
            return true;
        }

        internal void ExportRecords(List<FileRecord> recordsToExport)
        {
            if (recordsToExport.Count == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.Filter = ExportFilter;

            if (recordsToExport.Count == 1)
            {
                dialog.FileName = recordsToExport[0].Name;
            }
            else
            {
                dialog.FileName = "Multiple Files";
            }

            if (dialog.ShowDialog() == true)
            {
                var outputDirectory = Path.GetDirectoryName(dialog.FileName);
                var fileExtension = Path.GetExtension(dialog.FileName);

                var exportViewModel = new ExportViewModel
                {
                    RecordsToExport = recordsToExport,
                    BagPath = ParentViewModel.BagPath,
                    OutputDirectory = outputDirectory,
                    FileExtension = fileExtension,
                    CustomSaveFunc = OnExportFile
                };

                var exportDialog = new ExportView
                {
                    DataContext = exportViewModel
                };
                exportDialog.ShowDialog();
            }
        }

        private static void ExportRawFile(FileRecord fileRecord, string fileExtension, string outputDirectory, FileStream bagStream, Action<FileRecord, uint> onProgressReport, Func<bool> shouldCancel)
        {
            var outputPath = Path.GetFullPath(Path.Combine(outputDirectory, fileRecord.Name + fileExtension));

            using (var outStream = File.OpenWrite(outputPath))
            {
                fileRecord.Save(bagStream, outStream, onProgressReport, shouldCancel);
            }
        }

        private void OnExportFile(FileRecord fileRecord, string fileExtension, string outputDirectory, FileStream bagStream, Action<FileRecord, uint> onProgressReport, Func<bool> shouldCancel)
        {
            var recordExtension = Path.GetExtension(fileRecord.Name);
            if (string.Equals(recordExtension, fileExtension, StringComparison.OrdinalIgnoreCase))
            {
                ExportRawFile(fileRecord, fileExtension, outputDirectory, bagStream, onProgressReport, shouldCancel);
            }
            else
            {
                CustomFileExport(fileRecord, fileExtension, outputDirectory, bagStream, onProgressReport, shouldCancel);
            }
        }

        protected virtual void CustomFileExport(FileRecord fileRecord, string fileExtension, string outputDirectory, FileStream bagStream, Action<FileRecord, uint> onProgressReport, Func<bool> shouldCancel)
        {
            ExportRawFile(fileRecord, Path.GetExtension(fileRecord.Name), outputDirectory, bagStream, onProgressReport, shouldCancel);
        }

        public static void CopyAsUrl(FileRecord fileRecord)
        {
            if (fileRecord != null)
            {
                Clipboard.SetText($"https://sansar-asset-production.s3-us-west-2.amazonaws.com/{fileRecord.Name}");
            }
        }
    }
}
