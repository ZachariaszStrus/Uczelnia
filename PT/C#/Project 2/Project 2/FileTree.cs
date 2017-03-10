using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_2
{
    public class FileTree : INotifyPropertyChanged
    {
        FileTree parent;
        FileSystemInfo file;
        ObservableCollection<FileTree> fileSet;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public FileTree()
        {
            Parent = null;
            File = null;
            FileSet = new ObservableCollection<FileTree>();
        }

        public FileTree(FileSystemInfo dir, FileTree par = null)
        {
            Fill(dir, par);
        }

        public void Fill(FileSystemInfo dir, FileTree par = null)
        {
            Parent = par;
            File = dir;
            if (FileSet == null)
            {
                FileSet = new ObservableCollection<FileTree>();
            }
            else
            {
                FileSet.Clear();
            }
            if ((dir.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                foreach (string child in Directory.GetFiles(dir.FullName))
                {
                    FileSet.Add(new FileTree(new FileInfo(child), this));
                }
                foreach (string child in Directory.GetDirectories(dir.FullName))
                {
                    FileSet.Add(new FileTree(new DirectoryInfo(child), this));
                }
            }
        }

        public void Clear()
        {
            Parent = null;
            File = null;
            FileSet.Clear();
        }

        public void Add(FileSystemInfo dir)
        {
            FileTree newFile = new FileTree(dir, this);
            FileSet.Add(newFile);
        }

        public FileSystemInfo File
        {
            get
            {
                return file;
            }

            set
            {
                file = value;
                NotifyPropertyChanged("File");
            }
        }

        public ObservableCollection<FileTree> FileSet
        {
            get
            {
                return fileSet;
            }

            set
            {
                fileSet = value;
                NotifyPropertyChanged("FileSet");
            }
        }

        public string FileName
        {
            get
            {
                return File.Name;
            }
        }

        public FileTree Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                NotifyPropertyChanged("Parent");
            }
        }

        public void Remove()
        {
            RemoveBranch(this);
            Parent.FileSet.Remove(this);
        }

        private void RemoveBranch(FileTree file)
        {
            foreach (var child in file.FileSet)
            {
                RemoveBranch(child);
            }
            if (Directory.Exists(file.File.FullName))
            {
                Directory.Delete(file.File.FullName);
            }
            else
            {
                (file.File as FileInfo).IsReadOnly = false;
                System.IO.File.Delete(file.File.FullName);
            }
        }
    }
}
