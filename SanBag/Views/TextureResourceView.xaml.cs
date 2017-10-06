using LibSanBag;
using Microsoft.Win32;
using ResourceUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SanBag.Views
{
    /// <summary>
    /// Interaction logic for TextureResourceView.xaml
    /// </summary>
    public partial class TextureResourceView : UserControl
    {
        public TextureResourceView()
        {
            InitializeComponent();

            this.DataContext = this;
        }
        
        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ViewRecord(FileRecord record)
        {
        }

        private void buttonView_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonCopyUrl_Click(object sender, RoutedEventArgs e)
        {
        }

        private void dataGridContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        //    imagePreview.Visibility = Visibility.Hidden;

        //    try
        //    {
        //        var record = dataGridContent.SelectedItem as FileRecord;
        //        if (record != null)
        //        {
        //            var lowerName = record.Name.ToLower();
        //            if (lowerName.Contains("texture-resource") && lowerName.Contains("payload"))
        //            {
        //                using (var inStream = File.OpenRead(MainModel.Instance.BagPath))
        //                {
        //                    using (var outStream = new MemoryStream())
        //                    {
        //                        try
        //                        {
        //                            record.Save(inStream, outStream);
        //                            var ddsBytes = OodleLz.DecompressTextureResource(outStream);
        //                            if (ddsBytes[0] == 'D' && ddsBytes[1] == 'D' && ddsBytes[2] == 'S')
        //                            {
        //                                var imageData = LibDDS.GetImageBytesFromDds(ddsBytes);

        //                                var image = new BitmapImage();
        //                                image.BeginInit();
        //                                image.StreamSource = new MemoryStream(imageData);
        //                                image.EndInit();

        //                                imagePreview.Source = image;
        //                                imagePreview.Visibility = Visibility.Visible;
        //                            }
        //                        }
        //                        catch (Exception)
        //                        {
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to view texture: " + ex.Message, "ERROR");
        //    }
        }
    }
}
