using LibSanBag;
using LibSanBag.FileResources;
using LibSanBag.ResourceUtils;
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

namespace SanBag.ViewModels
{
    public class ScriptSourceTextResourceViewModel : GenericBagViewModel, INotifyPropertyChanged
    {
        public CommandExportSelectedScriptSourceText CommandExportSelectedScriptSourceText { get; set; }

        private FileRecord _selectedRecord;
        public FileRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                UpdatePreviewText();
                OnPropertyChanged();
            }
        }

        private string _previewCode = "";
        public string PreviewCode
        {
            get => _previewCode;
            set
            {
                _previewCode = value;
                OnPropertyChanged();
            }
        }

        public ScriptSourceTextResourceViewModel(MainViewModel parentViewModel)
                : base(parentViewModel)
        {
            CommandExportSelectedScriptSourceText = new CommandExportSelectedScriptSourceText(this);
        }

        public override bool IsValidRecord(FileRecord record)
        {
            return record.Info?.Resource == FileRecordInfo.ResourceType.ScriptSourceTextResource &&
                   record.Info?.Payload == FileRecordInfo.PayloadType.Payload;
        }

        private void UpdatePreviewText()
        {
            try
            {
                using (var bagStream = File.OpenRead(ParentViewModel.BagPath))
                {
                    var scriptSourceText = new ScriptSourceTextResource(bagStream, SelectedRecord);
                    PreviewCode = scriptSourceText.Source;
                }
            }
            catch (Exception)
            {
                PreviewCode = "";
            }
        }

        public void ExportRecordsAsAssemblies(List<FileRecord> recordsToExport)
        {
            if (recordsToExport.Count == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.Filter = "Script Source|*.cs";
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

                var exportViewModel = new ExportViewModel
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
                             Path.GetExtension(dialog.SafeFileName).ToLower(),
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

        private static void CustomSaveFunction(FileRecord fileRecord, string fileType, string outputDirectory, FileStream bagStream, Action<FileRecord, uint> onProgressReport, Func<bool> shouldCancel)
        {
            try
            {
                byte[] decompressedBytes = null;
                using (var compressedStream = new MemoryStream())
                {
                    fileRecord.Save(bagStream, compressedStream);
                    decompressedBytes = OodleLz.DecompressResource(compressedStream);
                }

                var scriptCompiledBytecode = new ScriptSourceTextResource(bagStream, fileRecord);
                var outputPath = Path.GetFullPath(Path.Combine(outputDirectory, fileRecord.Name + fileType));

                if (fileType == ".cs")
                {
                    File.WriteAllText(outputPath, scriptCompiledBytecode.Source);
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