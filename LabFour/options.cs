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
                    string FileName = currentFile.Substring(FilePath.Length);
                    if (File.ReadLines(FilePath + FileName).Any(line => line.Contains(FileKeywords)) || FileName.Contains(FileKeywords))
                    {
                        ReadyList.Add(FileName);
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
            List<string> Extensions = new List<string>();
            Console.WriteLine("Вводите расширения, по которым вы бы хотели провести индексацию. Для остановки введите ^ и Enter");
            while (!Extensions.Contains("^"))
            {
                Extensions.Add(Console.ReadLine());
            }
            Extensions.Remove("^");

            Console.Write("\nВведите имя файла c расширением, в который нужно сохранить результат индексации: ");
            string LoggingFileName = FilePath + @"\" + Console.ReadLine();
            FileStream IndexatedFile = new FileStream(LoggingFileName, FileMode.OpenOrCreate);
            using (StreamWriter Writer = new StreamWriter(IndexatedFile))
                foreach (string CurrentExtension in Extensions)
                {
                    var ExtensionFiles = Directory.EnumerateFiles(FilePath, "*." + CurrentExtension,
                        SearchOption.AllDirectories);
                    Writer.WriteLine(CurrentExtension + ":\n");
                    foreach (string CurrentFile in ExtensionFiles)
                    {
                        string FileName = CurrentFile.Substring(FilePath.Length);
                        Writer.WriteLine(FileName);
                    }
                }
            IndexatedFile.Close();
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
        private object Memento;
        public void SaveState(IOriginator originator)
        {
            originator.SetMemento(Memento);
        }

        public void RestoreState(IOriginator originator)
        {
            Memento = originator.GetMemento();
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
            TextClass Deserialized = (TextClass)bf.Deserialize(fs);
            Content = Deserialized.Content;
            FileName = Deserialized.FileName;
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
            TextClass Deserialized = (TextClass)xmlserializer.Deserialize(fs);
            Content = Deserialized.Content;
            FileName = Deserialized.FileName;
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
                var Memen = memento as Memento;
                Content = Memen.Content;
                FileName = Memen.FileName;
            }
        }
    }
}