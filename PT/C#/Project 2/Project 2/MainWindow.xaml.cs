using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;

namespace Project_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        FileTree myFileTree;

        internal FileTree MyFileTree
        {
            get
            {
                return myFileTree;
            }

            set
            {
                myFileTree = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MyFileTree = new FileTree();
            MyFileTree.Add(new DirectoryInfo("F:\\Downloads"));
            treeView.ItemsSource = MyFileTree.FileSet;
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = 
                new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();

            string rootDirectory = folderBrowserDialog.SelectedPath;
            if (Directory.Exists(rootDirectory))
            {
                MyFileTree.Clear();
                MyFileTree.Add(new DirectoryInfo(rootDirectory));
            }
        }
        
        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void deleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileTree selectedFile = treeView.SelectedItem as FileTree;
            if(selectedFile != null)
            {
                selectedFile.Remove();
            }
        }

        private void treeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void addContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileTree selectedFile = treeView.SelectedItem as FileTree;
            if (selectedFile != null)
            {
                if (!Directory.Exists(selectedFile.File.FullName))
                {
                    selectedFile = selectedFile.Parent;
                }

                FileCreationWindow window = new FileCreationWindow(selectedFile);
                window.Show();
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FileTree selectedFile = treeView.SelectedItem as FileTree;
            if (selectedFile != null)
            {
                rahsTextBlock.Text = selectedFile.File.GetRahs();
                if(selectedFile.File.Extension == ".txt")
                {
                    LoadTextFile(selectedFile.File);
                }
                else
                {
                    textBox.Text = "";
                }
            }
        }

        private void LoadTextFile(FileSystemInfo file)
        {
            var streamReader = new StreamReader(file.FullName, Encoding.UTF8);
            textBox.Text = streamReader.ReadToEnd();
            streamReader.Close();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            MenuItem item = menu.Items.GetItemAt(1) as MenuItem;
            if(File.Exists((treeView.SelectedItem as FileTree).File.FullName))
            {
                item.Visibility = Visibility.Collapsed;
            }
        }
    }
}
