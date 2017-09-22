using LibSanBag;
using Microsoft.Win32;
using ResourceUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SanBag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainModel Backend { get; set; } = new MainModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sansar  -> InstallLocation
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Bag files|*.bag|All files|*.*";

                if (dialog.ShowDialog() == true)
                {
                    Backend.OpenBag(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open bag: {ex.Message}");
            }
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            var filesToExport = new List<FileRecord>();
            foreach (var item in dataGridContent.SelectedItems)
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
                var record = dataGridContent.SelectedItem as FileRecord;
                dialog.FileName = record.Name;
            }
            else
            {
                dialog.FileName = "Multiple Files";
            }

            if (dialog.ShowDialog() == true)
            {
                var outputDirectory = Path.GetDirectoryName(dialog.FileName);
                var exportDialog = new ExportWindow(filesToExport, Backend.BagPath, outputDirectory);
                exportDialog.ShowDialog();
            }
        }

        private void ViewRecord(FileRecord record)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), record.Name);
            using (var inStream = File.OpenRead(Backend.BagPath))
            {
                using (var outStream = File.OpenWrite(tempPath))
                {
                    record.Save(inStream, outStream);
                }
            }

            Process.Start(tempPath);
        }

        private void buttonView_Click(object sender, RoutedEventArgs e)
        {
            var record = dataGridContent.SelectedItem as FileRecord;
            ViewRecord(record);
        }

        private void buttonCopyUrl_Click(object sender, RoutedEventArgs e)
        {
            var record = dataGridContent.SelectedItem as FileRecord;
            if (record != null)
            {
                Clipboard.SetText($"https://sansar-asset-production.s3-us-west-2.amazonaws.com/{record.Name}");
            }
        }

        private void dataGridContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imagePreview.Visibility = Visibility.Hidden;

            try
            {
                var record = dataGridContent.SelectedItem as FileRecord;
                if (record != null)
                {
                    var lowerName = record.Name.ToLower();
                    if (lowerName.Contains("texture-resource") && lowerName.Contains("payload"))
                    {
                        using (var inStream = File.OpenRead(Backend.BagPath))
                        {
                            using (var outStream = new MemoryStream())
                            {
                                try
                                {
                                    record.Save(inStream, outStream);
                                    var ddsBytes = OodleLz.DecompressTextureResource(outStream);
                                    if (ddsBytes[0] == 'D' && ddsBytes[1] == 'D' && ddsBytes[2] == 'S')
                                    {
                                        var imageData = LibDDS.GetImageBytesFromDds(ddsBytes);

                                        var image = new BitmapImage();
                                        image.BeginInit();
                                        image.StreamSource = new MemoryStream(imageData);
                                        image.EndInit();

                                        imagePreview.Source = image;
                                        imagePreview.Visibility = Visibility.Visible;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to view texture: " + ex.Message, "ERROR");
            }
        }
    }
}
