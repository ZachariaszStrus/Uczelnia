using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Project_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RandButton_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            RandTextBox.Text = r.Next().ToString();
        }

        // Newton ------------------------------------------------------------------------------------

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            long n = Convert.ToInt64(NTextBox.Text);
            long k = Convert.ToInt64(KTextBox.Text);

            Task.Run(() => CalcNewton(n, k)).ContinueWith(t => TasksTextBox.Text = t.Result, 
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public string CalcNewton(long n, long k)
        {
            Task<long> t1 = Task.Run<long>(() => CalcUp(n, k));
            Task<long> t2 = Task.Run<long>(() => CalcDown(n, k));

            t1.Wait();
            t2.Wait();

            return (t1.Result / t2.Result).ToString();
        }

        public long CalcUp(long n, long k)
        {
            Thread.Sleep(2000);
            long res = 1;
            for (long i = n - k + 1; i <= n; i++) 
            {
                res *= i;
            }
            return res;
        }

        public long CalcDown(long n, long k)
        {
            Thread.Sleep(2000);
            long res = 1;
            for (long i = 1; i <= k; i++)
            {
                res *= i;
            }
            return res;
        }

        // ------------------------------------------------------------------------------------


        private void DelegatesButton_Click(object sender, RoutedEventArgs e) 
        {
            long n = Convert.ToInt64(NTextBox.Text);
            long k = Convert.ToInt64(KTextBox.Text);

            Task.Run(() => CalcNewtonDelegates(n, k)).ContinueWith(t => DelegatesTextBox.Text = t.Result,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public delegate long AsyncMethodCaller(long n, long k);
        public string CalcNewtonDelegates(long n, long k)
        {
            AsyncMethodCaller caller1 = new AsyncMethodCaller(CalcUp);
            AsyncMethodCaller caller2 = new AsyncMethodCaller(CalcDown);

            IAsyncResult result1 = caller1.BeginInvoke(n, k, null, null);
            IAsyncResult result2 = caller2.BeginInvoke(n, k, null, null);

            long up = caller1.EndInvoke(result1);
            long down = caller2.EndInvoke(result2);
            return (up/down).ToString();
        }

        // ------------------------------------------------------------------------------------

        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            long n = Convert.ToInt64(NTextBox.Text);
            long k = Convert.ToInt64(KTextBox.Text);
            var result = await CalcNewtonAsync(n, k);
            AsyncTextBox.Text = result;
        }

        public Task<string> CalcNewtonAsync(long n, long k)
        {
            return Task.Run(() => CalcNewton(n, k));
        }


        // Fibonacci ------------------------------------------------------------------------------------

        private void FibonacciButton_Click(object s, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(ITextbox.Text);
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += ((object sender, DoWorkEventArgs args) => {
                BackgroundWorker worker = sender as BackgroundWorker;
                int n = (int)args.Argument;

                long a = 0;
                long b = 1;
                for (int j = 0; j < n; j++)
                {
                    Thread.Sleep(20);
                    worker.ReportProgress((j + 1) * 100 / n);
                    long tmp = b;
                    b += a;
                    a = tmp;
                }
                args.Result = a;
            });
            bw.ProgressChanged += ((object sender, ProgressChangedEventArgs args) => {
                FibonacciProgressBar.Value = args.ProgressPercentage;
            });
            bw.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) => {
                FibonacciTextBox.Text = args.Result.ToString();
            });
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(i);
        }


        // Compression ------------------------------------------------------------------------------------

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog =
                new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedPath = folderDialog.SelectedPath;
                if (Directory.Exists(selectedPath))
                {
                    string[] files = Directory.GetFiles(selectedPath);
                    Parallel.ForEach<string>(files, file =>
                    {
                        FileInfo fileToCompress = new FileInfo(file);
                        using (FileStream originalFileStream = fileToCompress.OpenRead())
                        {
                            if ((File.GetAttributes(fileToCompress.FullName) &
                               FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                            {
                                using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                                {
                                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                                       CompressionMode.Compress))
                                    {
                                        originalFileStream.CopyTo(compressionStream);

                                    }
                                }
                                FileInfo info = new FileInfo(selectedPath + "\\" + fileToCompress.Name + ".gz");
                            }
                        }
                    });
                }
            }
        }

        private void DecompressButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog =
                new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedPath = folderDialog.SelectedPath;
                if (Directory.Exists(selectedPath))
                {
                    string[] files = Directory.GetFiles(selectedPath);
                    Parallel.ForEach<string>(files, file =>
                    {
                        FileInfo fileToDecompress = new FileInfo(file);
                        using (FileStream originalFileStream = fileToDecompress.OpenRead())
                        {
                            string currentFileName = fileToDecompress.FullName;
                            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                            using (FileStream decompressedFileStream = File.Create(newFileName))
                            {
                                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                                {
                                    decompressionStream.CopyTo(decompressedFileStream);
                                }
                            }
                        }
                    });
                }
            }
        }

        
        // DNS ------------------------------------------------------------------------------------

        private void ResolveButton_Click(object sender, RoutedEventArgs e)
        {
            string[] hostNames = { "www.microsoft.com",
                                   "www.apple.com", "www.google.com",
                                   "www.ibm.com", "cisco.netacad.net",
                                   "www.oracle.com", "www.nokia.com",
                                   "www.hp.com", "www.dell.com",
                                   "www.samsung.com", "www.toshiba.com",
                                   "www.siemens.com", "www.amazon.com",
                                   "www.sony.com", "www.canon.com",
                                   "www.acer.com", "www.motorola.com"
                                 };

            var adresses = hostNames.AsParallel().Select(n => new { name = n, ip = Dns.GetHostAddresses(n).Last() });

            foreach (var a in adresses)
            {
                DNSTextBox.Text += a.name + " => " + a.ip + "\n";
            }
        }

    }
}
