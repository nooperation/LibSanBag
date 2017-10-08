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
    public class TextureResourceViewModel : GenericBagViewModel, INotifyPropertyChanged
    {
        public CommandExportSelectedTextures CommandExportSelectedTextures { get; set; }

        private FileRecord _selectedRecord;
        public FileRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                UpdatePreviewImage();
                OnPropertyChanged();
            }
        }

        private BitmapImage _currentPreview;
        public BitmapImage PreviewImage
        {
            get => _currentPreview;
            set
            {
                _currentPreview = value;
                OnPropertyChanged();
            }
        }

        private static BitmapImage _blankPreview = new BitmapImage();

        public TextureResourceViewModel(MainViewModel parentViewModel)
            : base(parentViewModel)
        {
            CommandExportSelectedTextures = new CommandExportSelectedTextures(this);
        }

        private static byte[] ExtractDds(FileRecord record, Stream inStream)
        {
            using (var outStream = new MemoryStream())
            {
                try
                {
                    record.Save(inStream, outStream);
                    return OodleLz.DecompressTextureResource(outStream);
                }
                catch (Exception)
                {
                }
            }

            return null;
        }

        private static BitmapImage ExtractImage(
            FileRecord record,
            Stream inStream,
            int width = 0,
            int height = 0,
            LibDDS.ConversionOptions.DXGI_FORMAT format = LibDDS.ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT)
        {
            using (var outStream = new MemoryStream())
            {
                try
                {
                    record.Save(inStream, outStream);
                    var ddsBytes = OodleLz.DecompressTextureResource(outStream);
                    if (ddsBytes[0] == 'D' && ddsBytes[1] == 'D' && ddsBytes[2] == 'S')
                    {

                        var imageData = LibDDS.GetImageBytesFromDds(ddsBytes, width, height, format);

                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = new MemoryStream(imageData);
                        image.EndInit();

                        return image;
                    }
                }
                catch (Exception)
                {
                }
            }

            return _blankPreview;
        }

        private static BitmapImage ExtractImage(
            FileRecord record,
            string bagPath,
            int width = 0,
            int height = 0,
            LibDDS.ConversionOptions.DXGI_FORMAT format = LibDDS.ConversionOptions.DXGI_FORMAT.DXGI_FORMAT_R32G32B32A32_FLOAT)
        {
            using (var inStream = File.OpenRead(bagPath))
            {
                return ExtractImage(record, inStream, width, height, format);
            }
        }

        public void ExportRecordsAsTextures(List<FileRecord> recordsToExport)
        {
            if (recordsToExport.Count == 0)
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.Filter = "DDS Source Image|*.dds|PNG Image|*.png|JPG Image|*.jpg|BMP Image|*.bmp|GIF Image|*.gif";
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
                    if (string.Equals(fileType, ".dds", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var imageBytes = ExtractDds(fileRecord, inStream);
                        outFile.Write(imageBytes, 0, imageBytes.Length);
                    }
                    else
                    {
                        var image = ExtractImage(fileRecord, inStream);
                        BitmapEncoder encoder = null;
                        switch (fileType.ToLower())
                        {
                            case ".png":
                                encoder = new PngBitmapEncoder();
                                break;
                            case ".jpg":
                            case ".jpeg":
                                encoder = new JpegBitmapEncoder();
                                break;
                            case ".gif":
                                encoder = new GifBitmapEncoder();
                                break;
                            default:
                                encoder = new BmpBitmapEncoder();
                                break;
                        }
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(outFile);
                    }
                }
            }
            catch (Exception)
            {
            }
            onProgressReport?.Invoke(fileRecord, 0);
        }

        private void UpdatePreviewImage()
        {
            try
            {
                if (SelectedRecord == null ||
                    SelectedRecord.Info == null ||
                    SelectedRecord.Info.Resource != FileRecordInfo.ResourceType.TextureResource ||
                    SelectedRecord.Info.Payload != FileRecordInfo.PayloadType.Payload)
                {
                    PreviewImage = _blankPreview;
                    return;
                }

                PreviewImage = ExtractImage(SelectedRecord, ParentViewModel.BagPath, 512, 512);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to view texture: " + ex.Message, "ERROR");
                PreviewImage = _blankPreview;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
