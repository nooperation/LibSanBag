using LibSanBag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SanBag
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        public ExportModel Backend { get; set; } = new ExportModel();

        public List<FileRecord> RecordsToExport { get; set; }
        public string BagPath { get; set; }
        public string OutputDirectory { get; set; }
        public bool IsRunning { get; private set; } = false;

        private CancellationTokenSource ExportCancellationTokenSource { get; set; } = null;

        public ExportWindow(List<FileRecord> recordsToExport, string bagPath, string outputDirectory)
        {
            RecordsToExport = recordsToExport;
            OutputDirectory = outputDirectory;
            BagPath = bagPath;

            InitializeComponent();

            this.DataContext = this;
            this.Closing += ExportWindow_Closing;
            this.Loaded += ExportWindow_Loaded;
        }

        private async void ExportWindow_Loaded(object sender, RoutedEventArgs e)
        {
            buttonCancel.Visibility = Visibility.Visible;
            IsRunning = true;
            await StartAsync();
        }

        private void ExportWindow_Closing(object sender, CancelEventArgs e)
        {
            if (ExportCancellationTokenSource != null)
            {
                ExportCancellationTokenSource.Cancel();
            }
        }

        private bool ShouldCancel()
        {
            return ExportCancellationTokenSource.IsCancellationRequested;
        }

        public async Task StartAsync()
        {
            ExportCancellationTokenSource = new CancellationTokenSource();
            var taskWasSuccessful = false;

            await Task.Run(() =>
            {
                try
                {
                    taskWasSuccessful = Backend.Export(RecordsToExport, BagPath, OutputDirectory, ShouldCancel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export: {ex.Message}");
                }
            }, ExportCancellationTokenSource.Token);

            buttonCancel.Visibility = Visibility.Hidden;
            buttonClose.Visibility = Visibility.Visible;

            if (taskWasSuccessful)
            {
                Close();
                MessageBox.Show($"Successfully exported {RecordsToExport.Count} record(s) to {OutputDirectory}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            IsRunning = false;
            buttonCancel.Visibility = Visibility.Hidden;
            ExportCancellationTokenSource.Cancel();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
