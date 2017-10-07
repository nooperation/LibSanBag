using LibSanBag;
using SanBag.ResourceUtils;
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

        }

        private void UpdatePreviewImage()
        {
            try
            {
                if (SelectedRecord == null || SelectedRecord.Info == null || SelectedRecord.Info.Type != FileRecordInfo.ResourceType.TextureResource || SelectedRecord.Info.ContentType != "payload")
                {
                    PreviewImage = _blankPreview;
                    return;
                }

                using (var inStream = File.OpenRead(ParentViewModel.BagPath))
                {
                    using (var outStream = new MemoryStream())
                    {
                        try
                        {
                            SelectedRecord.Save(inStream, outStream);
                            var ddsBytes = OodleLz.DecompressTextureResource(outStream);
                            if (ddsBytes[0] == 'D' && ddsBytes[1] == 'D' && ddsBytes[2] == 'S')
                            {
                                var imageData = LibDDS.GetImageBytesFromDds(ddsBytes);

                                var image = new BitmapImage();
                                image.BeginInit();
                                image.StreamSource = new MemoryStream(imageData);
                                image.EndInit();

                                PreviewImage = image;
                            }
                        }
                        catch (Exception)
                        {
                            PreviewImage = _blankPreview;
                        }
                    }
                }
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
