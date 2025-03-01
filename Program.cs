using System;
using System.Collections.Generic;
using static System.Console;

namespace WorkWithString{
public partial class ReadTextFileLines{

        // Количество записей в базу
        static int id = 0;

        // Путь к файлу // Замените на реальный путь к файлу
        static string filePath = @"C:\Users\user\Desktop\Notes\notes.adr";

        // Подключение к базе данных
        static string connString = "Host=localhost;Database=postgres;Username=postgres;Password=Admin123";

        // Массив для строк из файла
        static List<string> stringNotes = new List<string>(10000);

        public static void Main(string[] args){

            string folderName = "";
            string noteText = "";
            DateTime dtCreated = DateTime.Now;

            try{

                // Помещает строки из файла в массив stringNotes
                readFile();

                // Перебор строк
                foreach (string line in stringNotes) {

                    if (line.StartsWith("#FOLDER"))
                    {
                        folderName = strFolderNameSearch(line);
                    }

                    if (line.StartsWith("#NOTE"))
                    {
                        noteText = strNoteNameSearch(line);
                        dtCreated = ConvertUnixTimestampToDateTime(strNoteCreatedSearch(line));

                        // Отладка
                        //WriteLine($"Note number: {id++}, text: {noteText}, date create: {dtCreated}, category: {folderName}");

                        recordInDB(folderName, noteText, dtCreated);
                    }
                }

                WriteLine("-----------");
                WriteLine("END OF FILE");
                WriteLine("-----------");

                WriteLine($"String Number of : {id} was record in DB");
                ReadLine();

            } catch (Exception ex) { 
                WriteLine(ex.Message.ToString()); ReadLine(); }
        }
    }
}
