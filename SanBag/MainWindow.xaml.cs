using LibSanBag;
using Microsoft.Win32;
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
    }
}
