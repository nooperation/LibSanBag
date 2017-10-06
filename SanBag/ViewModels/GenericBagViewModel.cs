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

        public GenericBagViewModel(MainViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            this.CommandExportSelected = new CommandExportSelected(this);
            this.CommandCopyAsUrl = new CommandCopyAsUrl(this);
        }

        internal void ExportRecords(List<FileRecord> fileRecords)
        {
            var filesToExport = new List<FileRecord>();
            foreach (var item in fileRecords)
            {
                var record = item as FileRecord;
                if (record == null)
                {
                    MessageBox.Show("Item is not a record");
                    continue;
                }

                filesToExport.Add(record);
            }

            if (filesToExport.Count == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog();

            if (filesToExport.Count == 1)
            {
                dialog.FileName = filesToExport[0].Name;
            }
            else
            {
                dialog.FileName = "Multiple Files";
            }

            if (dialog.ShowDialog() == true)
            {
                var outputDirectory = Path.GetDirectoryName(dialog.FileName);

                var exportViewModel = new ExportViewModel()
                {
                    RecordsToExport = filesToExport,
                    BagPath = ParentViewModel.BagPath,
                    OutputDirectory = outputDirectory
                };

                var exportDialog = new ExportView
                {
                    DataContext = exportViewModel
                };
                exportDialog.ShowDialog();
            }
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
