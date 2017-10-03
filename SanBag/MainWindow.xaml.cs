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
        public UserControl CurrentView { get; set; } = new Views.TextureResourceView();
        public MainModel Backend => MainModel.Instance;

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
                var dialog = new OpenFileDialog()
                {
                    Filter = "Bag files|*.bag|All files|*.*"
                };
                if (dialog.ShowDialog() == true)
                {
                    MainModel.Instance.OpenBag(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open bag: {ex.Message}");
            }
        }
    }
}
