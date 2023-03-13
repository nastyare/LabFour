using System;


namespace LabFour
{
    internal static class EditorOfFile
    {
        public static void InitiateEdit(string FilePath, string FileName)
        {
            Console.Write("Что cделать с указанным файлом?\n\n1. Изменить текст\n" +
                "2. Запомнить состояние\n3. Откатить изменения\n\nВведите номер опции: ");
            int Choice = 0;
            while (Choice < 1 || Choice > 3)
            {
                if (int.TryParse(Convert.ToString(Console.ReadLine()), out Choice) == false)
                {
                    Console.WriteLine("Данные введены неверно. Попробуйте ещё раз");
                }
            }

            FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate);
            switch (Choice)
            {
                case 1:
                    FileReader(file, FileName);
                    Console.Clear();
                    Console.WriteLine("Введите новое содержание файла(нажмите ^ и Enter для выхода):");
                    char Ch;
                    int Element;
                    string Input = "";
                    do
                    {
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
                    } while (Ch != '^');
                    FileWriter(Input, FilePath, FileName);
                    Console.Clear();
                    Console.WriteLine("Изменения добавлены успешно");
                    Console.ReadKey();
                    break;
                case 2:
                    FileReader(file, FileName);
                    care.SaveState(textFile);
                    break;
                case 3:
                    try
                    {
                        file.Close();
                        RestoreData(FilePath, FileName);
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine("Не было изменений");
                        Console.ReadKey();
                    }
                    break;
            }
            file.Close();
        }

        static TextClass textFile = new TextClass();
        static Caretaker care = new Caretaker();

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
                textFile.Content.Add(FileName, OutString);
                textFile.FileName.Add(FileName);
            }
            catch (Exception)
            {
                textFile.Content[FileName] = OutString;
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
            care.RestoreState(textFile);
            using (StreamWriter Writer = new StreamWriter(UserPath, false))
            {
                Writer.Write(textFile.Content[FileName]);
            }
        }
    }
}