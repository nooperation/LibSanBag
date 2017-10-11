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

                try
                {
                    using (var bagStream = File.OpenRead(ParentViewModel.BagPath))
                    {
                        using (var compressedStream = new MemoryStream())
                        {
                            _selectedRecord.Save(bagStream, compressedStream);
                            var scriptSourceText = ExtractScriptSourceText(compressedStream.GetBuffer());
                            PreviewCode = scriptSourceText.Source;
                        }
                    }
                }
                catch (Exception)
                {
                    PreviewCode = "";
                }

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

        class ScriptSourceText
        {
            public string Filename { get; set; }
            public string Source { get; set; }
        }

        private static ScriptSourceText ExtractScriptSourceText(byte[] compressedBytes)
        {
            var scriptSourceText = new ScriptSourceText();
            byte[] decompressedSourceBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedSourceBytes = OodleLz.DecompressResource(compressedStream);
            }

            using (var decompressedStream = new MemoryStream(decompressedSourceBytes))
            {
                using (var br = new BinaryReader(decompressedStream))
                {
                    // TODO: Find the actual length...
                    var filenameString = "";
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        filenameString += br.ReadChar();
                        if (filenameString.EndsWith(".cs"))
                        {
                            break;
                        }
                    }

                    scriptSourceText.Filename = filenameString;
                    var assemblyLength = br.ReadInt32();
                    scriptSourceText.Source = new string(br.ReadChars(assemblyLength));
                }
            }

            return scriptSourceText;
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

                var scriptCompiledBytecode = ExtractScriptSourceText(decompressedBytes);
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