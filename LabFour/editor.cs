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
                    char ch;
                    int element;
                    string Input = "";
                    do
                    {
                        element = Console.Read();
                        try
                        {
                            ch = Convert.ToChar(element);
                            Input += ch;
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine($"{element} - не подходящее значение");
                            ch = Char.MinValue;
                        }
                    } while (ch != '^');
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
            string outString = "";
            var reader = new StreamReader(file);

            while (!reader.EndOfStream)
            {
                outString += reader.ReadLine();
            }
            try
            {
                textFile.Content.Add(FileName, outString);
                textFile.FileName.Add(FileName);
            }
            catch (Exception)
            {
                textFile.Content[FileName] = outString;
            }
            reader.Close();
        }

        private static void FileWriter(string input, string UserPath, string FileName)
        {
            using (StreamWriter writer = new StreamWriter(UserPath, true))
            {
                writer.Write(input);
            }
        }

        private static void RestoreData(string UserPath, string FileName)
        {
            care.RestoreState(textFile);
            using (StreamWriter writer = new StreamWriter(UserPath, false))
            {
                writer.Write(textFile.Content[FileName]);
            }
        }
    }
}