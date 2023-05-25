using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    public class Database
    {
        private string connectionString = @"Data Source=Habit.db";
        public void OpenDatabase() 
        {
            using (var connection = new SqliteConnection(connectionString)) {

                connection.Open();

                var command = connection.CreateCommand();
                
                command.CommandText = @"CREATE TABLE IF NOT EXISTS habits (id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Hours INTEGER)";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public void Insert(string date, int quantity) 
        {
            using ( var connection = new SqliteConnection( connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = $"INSERT INTO habits(Date, Hours) VALUES('{date}', '{quantity}')";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public void View(List<Habit> tableData)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = $"SELECT * FROM habits ";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new Habit
                            {
                                ID = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                } 
                else
                {
                    Console.WriteLine("No Rows found in habit");
                }
                
                command.Dispose();
                connection.Dispose();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = $"DELETE from habits WHERE Id = '{id}'";

                int rowCount = command.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record id '{id}' doesn't exist");
                    return;
                }
                
                command.Dispose();
                connection.Dispose();
            }
        }

        public void Update(int id, string date, int quantity)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = $"UPDATE habits Set Date = '{date}', Hours = {quantity} WHERE Id = {id}";

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }

        public bool CheckValid(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {id})";
                int checkQuery = Convert.ToInt32(command.ExecuteScalar());

                if (checkQuery == 0)
                {
                    command.Dispose();
                    connection.Dispose();
                    return false;
                }
                else
                {
                    command.Dispose();
                    connection.Dispose();
                    return true;
                }
            }
        }
    }
     
}
