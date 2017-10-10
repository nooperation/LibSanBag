using LibSanBag;
using Microsoft.Win32;
using SanBag.Commands;
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
using System.Windows;
using System.Windows.Media.Imaging;

namespace SanBag.ViewModels
{
    public class ScriptCompiledBytecodeResourceViewModel : GenericBagViewModel, INotifyPropertyChanged
    {
        public CommandExportSelectedAssembly CommandExportSelectedAssembly { get; set; }

        private FileRecord _selectedRecord;
        public FileRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }

        public ScriptCompiledBytecodeResourceViewModel(MainViewModel parentViewModel)
            : base(parentViewModel)
        {
            CommandExportSelectedAssembly = new CommandExportSelectedAssembly(this);
        }

        public override bool IsValidRecord(FileRecord record)
        {
            return record.Info?.Resource == FileRecordInfo.ResourceType.ScriptCompiledBytecodeResource &&
                   record.Info?.Payload == FileRecordInfo.PayloadType.Payload;
        }

        private static byte[] ExtractDds(FileRecord record, Stream inStream)
        {
            using (var outStream = new MemoryStream())
            {
                try
                {
                    record.Save(inStream, outStream);
                    return OodleLz.DecompressResource(outStream);
                }
                catch (Exception)
                {
                }
            }

            return null;
        }

        public void ExportRecordsAsAssemblies(List<FileRecord> recordsToExport)
        {
            if (recordsToExport.Count == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.Filter = ".Net Assembly|*.dll";
            dialog.FilterIndex = 0;
            if (recordsToExport.Count == 1)
            {
                dialog.FileName = recordsToExport[0].Info.Hash;
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
                    RecordsToExport = recordsToExport,
                    BagPath = ParentViewModel.BagPath,
                    OutputDirectory = outputDirectory,
                    CustomSaveFunc = (
                        fileRecord,
                        bagStream,
                        onProgressReport,
                        shouldCancel
                    ) => CustomSaveFunction(
                             fileRecord,
                             Path.GetExtension(dialog.SafeFileName),
                             outputDirectory,
                             bagStream,
                             onProgressReport,
                             shouldCancel
                         )
                };

                var exportDialog = new ExportView
                {
                    DataContext = exportViewModel
                };
                exportDialog.ShowDialog();
            }
        }

        private static void CustomSaveFunction(FileRecord fileRecord, string fileType, string outputDirectory, FileStream inStream, Action<FileRecord, uint> onProgressReport, Func<bool> shouldCancel)
        {
            try
            {
                var fileBytes = new byte[fileRecord.Length];

                inStream.Seek(fileRecord.Offset, SeekOrigin.Begin);
                inStream.Read(fileBytes, 0, fileBytes.Length);

                var outputPath = Path.GetFullPath(outputDirectory + "\\" + fileRecord.Name + fileType);
                using (var outFile = File.OpenWrite(outputPath))
                {

                }
            }
            catch (Exception)
            {
            }

            onProgressReport?.Invoke(fileRecord, 0);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
