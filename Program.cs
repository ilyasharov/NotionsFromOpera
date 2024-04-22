using Npgsql;
using System.Text.RegularExpressions;
using static System.Console;

namespace ParseNotes{
    class Program{
        static void Main(string[] args){
            try{

                // **Вставьте свои данные для подключения к базе данных PostgreSQL**
                string connString = "Host=localhost;Database=postgres;Username=postgres;Password=Admin123";

                // Чтение файла "notes.adr"
                string filePath = "C:\\Users\\Admin\\Desktop\\-\\notes2.adr";
                string fileContent = File.ReadAllText(filePath);

                int id = 1;

                // Регулярные выражения
                Regex folderRegex = new Regex(@"#FOLDER\s+NAME=(?'name'.+)");
                Regex noteRegex = new Regex(@"#NOTE\s+NAME=(?'name'.+)");

                // Обработка строк файла
                foreach (Match match in folderRegex.Matches(fileContent)){

                    string category = match.Groups["name"].Value;

                    foreach (Match noteMatch in noteRegex.Matches(fileContent)){

                        string content = noteMatch.Groups["name"].Value;

                        // Запись данных в базу данных
                        using (NpgsqlConnection connection = new NpgsqlConnection(connString))
                        {
                            connection.Open();

                            using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO NotionsTemp (id, Category, Content) VALUES (@id, @category, @content)", connection))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.Parameters.AddWithValue("@category", category);
                                command.Parameters.AddWithValue("@content", content);
                                command.ExecuteNonQuery();
                            }
                        }

                        WriteLine($"Записано: {id} - {category} - {content}"); ReadLine();
                        content = ""; id++;
                    }
                }
            } catch (Exception e) { WriteLine(e.Message.ToString() ); ReadLine(); }
        }
    }
}
