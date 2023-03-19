using System;
using System.IO;
using LabFour;

namespace LabFour
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Вставьте полный путь папки: ");
            string Path = Console.ReadLine();
            bool Success = false;
            FileInfo fileInfo = new FileInfo(Path);
            if (!fileInfo.Exists)
            {
                Console.WriteLine("Неправильный путь к файлу.");
            }
            int UserChoice = 0;
            while (UserChoice != 4)
            {
                Console.Clear();
                Console.WriteLine($"Что сделать с этим файлом?");
                Console.WriteLine("1. Редактировать файл\n2. Найти файлы по ключевым словам\n" +
                    "3. Проиндексировать все файлы в рабочей папке в отдельный файл\n4. Выход");
                Console.Write("Выберите опцию: ");
                while (UserChoice < 1 || UserChoice > 4)
                {
                    if (int.TryParse(Convert.ToString(Console.ReadLine()), out UserChoice) == false)
                    {
                        Console.WriteLine("Такой опции не существует");
                    }
                }
                Console.Clear();
                string FileName;
                switch (UserChoice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Введите название txt файла, который нужно отредактировать: ");
                        FileName = Console.ReadLine();
                        EditorOfFile.InitiateEdit(Path + @"\" + FileName + ".txt", FileName);
                        UserChoice = 0;
                        break;
                    case 2:
                        Console.Write("Введите ключевое слово: ");
                        string Keywords = Console.ReadLine();
                        Console.Clear();
                        Searcher.FileKeywordsSearcher(Path, Keywords);
                        Console.ReadKey();
                        UserChoice = 0;
                        break;
                    case 3:
                        Indexator.PerformIndexation(Path);
                        UserChoice = 0;
                        break;
                }
            }
            
        }
    }
}