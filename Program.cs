using System;
using System.IO;
using Npgsql;
using static System.Console;

class Program{
    static void Main(string[] args){
        try{
            string filePath = "C:\\Users\\Admin\\Desktop\\-\\notes2.adr";

            // Подключение к базе данных
            string connString = "Host=localhost;Database=postgres;Username=postgres;Password=Admin123";
            using (var conn = new NpgsqlConnection(connString)){
                conn.Open();

                // Чтение файла
                string fileContent = File.ReadAllText(filePath);

                // Разделение файла на строки
                string[] lines = fileContent.Split('\n');

                // Переменные для значений
                string category = null;
                string content = null;

                int id = 1;

                // Обработка строк
                foreach (string line in lines){
                    // Проверка префикса "#FOLDER"
                    if (line.StartsWith("#FOLDER")){
                        // Извлечение значения NAME
                        string[] parts = line.Split('=');
                        if (parts.Length > 1)
                        {
                            category = parts[1].Trim(); // Удаление лишних пробелов
                        }
                    }

                    // Проверка префикса "#NOTE"
                    else if (line.StartsWith("#NOTE")){

                        // Извлечение значения NAME
                        string[] parts = line.Split('=');
                        if (parts.Length > 1)
                        {
                            content = parts[1].Trim(); // Удаление лишних пробелов
                        }

                        // Запись в базу данных (если category и content не null)
                        if (category != null && content != null)
                        {
                            string insertQuery = "INSERT INTO NotionsTemp (id, Content, Category) VALUES (@id, @content, @category)";
                            using (var cmd = new NpgsqlCommand(insertQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@content", content);
                                cmd.Parameters.AddWithValue("@category", category);
                                cmd.ExecuteNonQuery();
                            }

                            WriteLine($"Записано: {category} - {content}"); ReadLine();

                            // Сброс переменных
                            //category = null;
                            content = null;

                            id++;
                        }
                    }
                }
            }
        }   catch (Exception e) { WriteLine(e.Message.ToString() ); ReadLine(); }
    }
}