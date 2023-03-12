using System;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LabFour
{
    internal class Searcher
    {
        public static void FileKeywordsSearcher(string FilePath, string FileKeywords)
        {
            List<string> ReadyList = new List<string>();
            try
            {
                var txtFiles = Directory.EnumerateFiles(FilePath, "*.txt", SearchOption.AllDirectories);

                foreach (string currentFile in txtFiles)
                {
                    string fileName = currentFile.Substring(FilePath.Length);
                    if (File.ReadLines(FilePath + fileName).Any(line => line.Contains(FileKeywords)) || fileName.Contains(FileKeywords))
                    {
                        ReadyList.Add(fileName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (ReadyList.Count != 0)
            {
                for (int ElementIndex = 0; ElementIndex < ReadyList.Count; ++ElementIndex)
                {
                    Console.Write($"{ElementIndex + 1}. {ReadyList[ElementIndex]}\n");
                }
            }
            else
            {
                Console.WriteLine("Нет подходящих файлов");
            }
        }
    }

    internal static class Indexator
    {
        public static void PerformIndexation(string FilePath)
        {
            List<string> extensions = new List<string>();
            Console.WriteLine("Вводите расширения, по которым вы бы хотели провести индексацию. Для остановки введите ^ и Enter");
            while (!extensions.Contains("^"))
            {
                extensions.Add(Console.ReadLine());
            }
            extensions.Remove("^");

            Console.Write("\nВведите имя файла c расширением, в который нужно сохранить результат индексации: ");
            string loggingFileName = FilePath + @"\" + Console.ReadLine();
            FileStream indexatedFile = new FileStream(loggingFileName, FileMode.OpenOrCreate);
            using (StreamWriter writer = new StreamWriter(indexatedFile))
                foreach (string currentExtension in extensions)
                {
                    var extensionFiles = Directory.EnumerateFiles(FilePath, "*." + currentExtension,
                        SearchOption.AllDirectories);
                    writer.WriteLine(currentExtension + ":\n");
                    foreach (string currentFile in extensionFiles)
                    {
                        string fileName = currentFile.Substring(FilePath.Length);
                        writer.WriteLine(fileName);
                    }
                }
            indexatedFile.Close();
        }
    }
    public interface IOriginator
    {
        object GetMemento();
        void SetMemento(object memento);
    }

    class Memento
    {
        public Dictionary<string, string> Content { get; set; }
        public List<string> FileName { get; set; }
    }

    public class Caretaker
    {
        private object memento;
        public void SaveState(IOriginator originator)
        {
            originator.SetMemento(memento);
        }

        public void RestoreState(IOriginator originator)
        {
            memento = originator.GetMemento();
        }
    }

    [Serializable]
    class TextClass : IOriginator
    {
        public Dictionary<string, string> Content { get; set; }
        public List<string> FileName { get; set; }

        public TextClass()
        {
            Content = new Dictionary<string, string>();
            FileName = new List<string>();
        }
        public TextClass(string Content, string FileName)
        {
            this.Content.Add(FileName, Content);
            this.FileName.Add(FileName);
        }

        public void BinarySerialization(FileStream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Flush();
            fs.Close();
        }

        public void BinaryDeserialization(FileStream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            TextClass deserialized = (TextClass)bf.Deserialize(fs);
            Content = deserialized.Content;
            FileName = deserialized.FileName;
            fs.Close();
        }

        public void XmlSerialization(FileStream fs)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(TextClass));
            xmlserializer.Serialize(fs, this);
            fs.Flush();
            fs.Close();
        }

        public void XmlDeserialization(FileStream fs)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(TextClass));
            TextClass deserialized = (TextClass)xmlserializer.Deserialize(fs);
            Content = deserialized.Content;
            FileName = deserialized.FileName;
            fs.Close();
        }

        object IOriginator.GetMemento()
        {
            return new Memento { Content = this.Content, FileName = this.FileName };
        }

        void IOriginator.SetMemento(object memento)
        {
            if (memento is Memento)
            {
                var memen = memento as Memento;
                Content = memen.Content;
                FileName = memen.FileName;
            }
        }
    }
}