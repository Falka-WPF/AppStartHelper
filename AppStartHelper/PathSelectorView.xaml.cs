using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AppStartHelper
{
    /// <summary>
    /// Логика взаимодействия для PathSelectorView.xaml
    /// </summary>
    public partial class PathSelectorView : Window
    {
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public PathSelectorView()
        {
            InitializeComponent();
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog()==true)
            {
                ApplicationPath = ofd.FileName;
            }
            Path_TextBox.Text = ApplicationPath;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationName = Name_TextBox.Text;
            if (String.IsNullOrWhiteSpace(ApplicationPath) || String.IsNullOrWhiteSpace(ApplicationName))
            {
                MessageBox.Show("Some fields aren`t filled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                this.DialogResult = true;
            }
        }
    }
}
