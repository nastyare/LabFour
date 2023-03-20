using System;


namespace LabFour
{
    internal static class EditorOfFile
    {
        public static void InitiateEdit(string FilePath, string FileName)
        {
            Console.Write("Что cделать с этим файлом?\n\n1. Изменить текст\n" +
                "2. Запомнить состояние\n3. Откатить изменения\n\nВведите номер опции: ");
            int Choice = 0;
            while (Choice < 1 || Choice > 3)
            {
                if (int.TryParse(Convert.ToString(Console.ReadLine()), out Choice) == false)
                {
                    Console.WriteLine("Такой опции нет");
                }
            }

            FileStream File = new FileStream(FilePath, FileMode.OpenOrCreate);
            switch (Choice)
            {
                case 1:
                    FileReader(File, FileName);
                    Console.Clear();
                    Console.WriteLine("Введите новое содержание файла(:");
                    char Ch;
                    int Element;
                    string Input = "";
                        Element = Console.Read();
                        try
                        {
                            Ch = Convert.ToChar(Element);
                            Input += Ch;
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine($"{Element} - не подходящее значение");
                            Ch = Char.MinValue;
                        }
                    FileWriter(Input, FilePath, FileName);
                    Console.Clear();
                    Console.WriteLine("Изменения добавлены успешно");
                    Console.ReadKey();
                    break;
                case 2:
                    FileReader(File, FileName);
                    Care.SaveState(TextFile);
                    break;
                case 3:
                    try
                    {
                        File.Close();
                        RestoreData(FilePath, FileName);
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine("Не было изменений");
                        Console.ReadKey();
                    }
                    break;
            }
            File.Close();
        }

        static TextClass TextFile = new TextClass();
        static Caretaker Care = new Caretaker();

        private static void FileReader(FileStream file, string FileName)
        {
            string OutString = "";
            var Reader = new StreamReader(file);

            while (!Reader.EndOfStream)
            {
                OutString += Reader.ReadLine();
            }
            try
            {
                TextFile.Content.Add(FileName, OutString);
                TextFile.FileName.Add(FileName);
            }
            catch (Exception)
            {
                TextFile.Content[FileName] = OutString;
            }
            Reader.Close();
        }

        private static void FileWriter(string Input, string UserPath, string FileName)
        {
            using (StreamWriter Writer = new StreamWriter(UserPath, true))
            {
                Writer.Write(Input);
            }
        }

        private static void RestoreData(string UserPath, string FileName)
        {
            Care.RestoreState(TextFile);
            using (StreamWriter Writer = new StreamWriter(UserPath, false))
            {
                Writer.Write(TextFile.Content[FileName]);
            }
        }
    }
}