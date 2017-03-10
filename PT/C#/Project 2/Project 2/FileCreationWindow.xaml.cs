using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_2
{
    /// <summary>
    /// Interaction logic for FileCreationWindow.xaml
    /// </summary>
    public partial class FileCreationWindow : Window
    {
        FileTree destinationDirectory;

        internal FileTree DestinationDirectory
        {
            get
            {
                return destinationDirectory;
            }

            set
            {
                destinationDirectory = value;
            }
        }

        public FileCreationWindow(FileTree dir)
        {
            InitializeComponent();

            this.DestinationDirectory = dir;
        }
        
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = fileNameTextBox.Text.Substring(0, fileNameTextBox.Text.Length - 2);
            string pattern = @"^[\-~\w]{1,8}\.(txt|htm|php)";

            if(fileName != "")
            {
                bool? isDirectory = directoryRadioButton.IsChecked;
                string filePath = System.IO.Path.Combine(destinationDirectory.File.FullName, fileName);

                if (isDirectory.HasValue && isDirectory == true)
                {
                    if (!Directory.Exists(fileName))
                    {
                        Directory.CreateDirectory(filePath);
                        DirectoryInfo newFile = new DirectoryInfo(filePath);
                        destinationDirectory.Add(newFile);
                        Finnish();
                    }
                }
                else
                {
                    if(Regex.IsMatch(fileName, pattern))
                    {
                        if (!File.Exists(filePath))
                        {
                            FileStream fs = File.Create(filePath);
                            FileInfo newFile = new FileInfo(filePath);
                            SetFileAttributes(newFile);

                            destinationDirectory.Add(newFile);

                            fs.Close();
                            Finnish();
                        }
                        else
                        {
                            messageLabel.Content = "That file already exists!";
                        }
                    }
                    else
                    {
                        messageLabel.Content = "Wrong file name!";
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Finnish();
        }
        
        private void SetFileAttributes(FileSystemInfo newFile)
        {
            bool? isReadOnly = readOnlyCheckBox.IsChecked;
            bool? isArchive = archiveCheckBox.IsChecked;
            bool? isHidden = hiddenCheckBox.IsChecked;
            bool? isSystem = systemCheckBox.IsChecked;

            if (isReadOnly.HasValue && isReadOnly == true)
            {
                newFile.Attributes |= FileAttributes.ReadOnly;
            }
            if (isArchive.HasValue && isArchive == true)
            {
                newFile.Attributes |= FileAttributes.Archive;
            }
            if (isHidden.HasValue && isHidden == true)
            {
                newFile.Attributes |= FileAttributes.Hidden;
            }
            if (isSystem.HasValue && isSystem == true)
            {
                newFile.Attributes |= FileAttributes.System;
            }
        }

        private void Finnish()
        {
            this.Close();
        }
    }
}
