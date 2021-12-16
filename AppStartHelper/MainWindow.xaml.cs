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
                Category selected = CategoryListBox.SelectedItem as Category;
                db.CategorySet.Remove(selected);
                db.SaveChanges();
                LoadCategories();
                MessageBox.Show("Category deleted!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadApps();
        }
    }
}
