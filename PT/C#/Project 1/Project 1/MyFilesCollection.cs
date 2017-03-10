using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Project_1
{
    [Serializable]
    class MyFilesCollection : SortedDictionary<string, long>
    {
        public MyFilesCollection(DirectoryInfo dir, IComparer<string> comparer) : base(comparer)
        {
            foreach (string child in Directory.GetFiles(dir.FullName))
            {
                FileInfo childFile = new FileInfo(child);
                Add(childFile.Name, childFile.GetSize());
            }
            foreach (string child in Directory.GetDirectories(dir.FullName))
            {
                DirectoryInfo childDir = new DirectoryInfo(child);
                Add(childDir.Name, childDir.GetSize());
            }
        }

        public void Print()
        {
            foreach (string key in this.Keys)
            {
                Console.WriteLine(key + " -> " + Convert.ToString(this[key]));
            }
        }

        public void Serialize(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, this);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public void Deserialize(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                this.Clear();
                MyFilesCollection deserialisedCollection = (MyFilesCollection)formatter.Deserialize(fs);
                foreach (string key in deserialisedCollection.Keys)
                {
                    this.Add(key, deserialisedCollection[key]);
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            Print();
        }

    }
}
