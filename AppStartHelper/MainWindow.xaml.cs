using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using AppStartHelper.Data;

namespace AppStartHelper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model1Container1 db;
        public MainWindow()
        {
            InitializeComponent();
            db = new Model1Container1();
        }


        private void CategoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(CategoryNameTextBox.Text)))
            {
                string categoryName = CategoryNameTextBox.Text;
                db.CategorySet.Add(new Category() { CategoryName= categoryName });
                db.SaveChanges();
                LoadCategories();
                CategoryNameTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Category name can`t be null!","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void CategoryDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryListBox.SelectedIndex != -1)
            {
                try
                {
                    Category selected = CategoryListBox.SelectedItem as Category;
                    db.CategorySet.Remove(selected);
                    db.SaveChanges();
                    LoadCategories();
                    MessageBox.Show("Category deleted!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Delete applications at first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Category not selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadCategories()
        {
            CategoryListBox.Items.Clear();
            foreach(var item in db.CategorySet)
            {
                CategoryListBox.Items.Add(item);
            }
        }
        private void LoadApps()
        {
            ApplicationListBox.Items.Clear();  
            if (CategoryListBox.SelectedIndex != -1)
            {
                Category ct = CategoryListBox.SelectedItem as Category;
                foreach(var item in ct.Applications)
                {
                    ApplicationListBox.Items.Add(item);
                }
            }
        }
        private void ApplicationAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryListBox.SelectedIndex != -1)
            {
                PathSelectorView ps = new PathSelectorView();
                if (ps.ShowDialog() == true)
                {
                    Category ct = CategoryListBox.SelectedItem as Category;
                    string appName = ps.ApplicationName;
                    string appPath = ps.ApplicationPath;
                    AppStartHelper.Data.Application ap = new AppStartHelper.Data.Application();
                    ap.FileName = appName;
                    ap.FilePath = appPath;
                    ap.Category = ct;
                    db.ApplicationSet.Add(ap);
                    db.SaveChanges();
                    LoadApps();
                }
            }
        }

        private void ApplicationDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(ApplicationListBox.SelectedIndex != -1)
            {
                AppStartHelper.Data.Application ap = ApplicationListBox.SelectedItem as AppStartHelper.Data.Application;
                db.ApplicationSet.Remove(ap);
                db.SaveChanges();
                LoadApps();
                MessageBox.Show("Application deleted!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Application not selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadApps();
        }

        private void ApplicationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ApplicationListBox.SelectedIndex != -1)
            {
                AppStartHelper.Data.Application ap = ApplicationListBox.SelectedItem as AppStartHelper.Data.Application;
                FilePathTextBox.Text = ap.FilePath;
            }
            else
            {
                FilePathTextBox.Text = "";
            }
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationListBox.Items.Clear();
            if (CategoryListBox.SelectedIndex != -1)
            {
                Category ct = CategoryListBox.SelectedItem as Category;
                foreach(var item in ct.Applications)
                {
                    ApplicationListBox.Items.Add(item);
                }
            }
        }
        Process currentProc;
        private void StartAppButton_Click(object sender, RoutedEventArgs e)
        {
            currentProc = new Process();
            currentProc.StartInfo = new ProcessStartInfo(FilePathTextBox.Text);
            currentProc.Start();
            StartAppButton.IsEnabled = false;
            CategoryListBox.IsEnabled = false;
            ApplicationListBox.IsEnabled = false;
            StopAppButton.IsEnabled = true;
        }

        private void StopAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentProc != null)
            {
                try
                {
                    currentProc.Kill();
                }
                catch
                {
                    MessageBox.Show("Process stop error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            StartAppButton.IsEnabled = true;
            CategoryListBox.IsEnabled = true;
            ApplicationListBox.IsEnabled = true;
            StopAppButton.IsEnabled = false;
        }
    }
}
