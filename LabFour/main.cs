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
            string FilePath = @"C:\";
            bool Success = false;
            while (Success == false)
            {
                FilePath = Console.ReadLine();
                if (Directory.Exists(FilePath) && FilePath != string.Empty)
                {
                    Success = true;
                }
                else
                {
                    Console.WriteLine("Такого пути не существует");
                }
            }
            int Choice = 0;
            if (Choice != 4)
            {
                Console.Clear();
                Console.WriteLine($"Выбранный путь: {FilePath}");
                Console.WriteLine("1. Редактировать файл\n2. Найти файлы по ключевым словам\n" +
                    "3. Проиндексировать все файлы в рабочей папке в отдельный файл\n4. Выход");
                Console.Write("Выберите опцию: ");
                if (Choice < 1 || Choice > 4)
                {
                    if (int.TryParse(Convert.ToString(Console.ReadLine()), out Choice) == false)
                    {
                        Console.WriteLine("Такой опции не существует");
                    }
                }
                Console.Clear();
                string FileName;
                switch (Choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Введите название txt файла, который нужно отредактировать: ");
                        FileName = Console.ReadLine();
                        EditorOfFile.InitiateEdit(FilePath + @"\" + FileName + ".txt", FileName);
                        Choice = 0;
                        break;
                    case 2:
                        Console.Write("Введите ключевые слова для поиска: ");
                        string UserKeywords = Console.ReadLine();
                        Console.Clear();
                        Searcher.FileKeywordsSearcher(FilePath, UserKeywords);
                        Console.ReadKey();
                        Choice = 0;
                        break;
                    case 3:
                        Indexator.PerformIndexation(FilePath);
                        Choice = 0;
                        break;
                }
            }
            
        }
    }
}