using System.Globalization;

namespace HabitTracker
{
    public class User
    {
        public Database db;

        public User()
        {
            db = new Database();
            db.OpenDatabase();
        }
        public void DisplayMenu()
        {
            bool appRunning = true;

            while (appRunning)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine("       Habit Tracker      ");
                Console.WriteLine("--------------------------");
                Console.WriteLine("Press '0' to Exit.");
                Console.WriteLine("Press '1' to View Records.");
                Console.WriteLine("Press '2' to Insert Record.");
                Console.WriteLine("Press '3' to Delete Record.");
                Console.WriteLine("press '4' to Change Record.");
                Console.WriteLine("--------------------------");
                Console.WriteLine("\nPlease Enter your choice");

                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "0":
                        appRunning = false;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetRecords();
                        break;
                    case "2":
                        AddRecord();
                        break;
                    case "3":
                        DeleteRecords();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid Selection. Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }
        }

        public void AddRecord()
        {
            Console.Clear();

            Console.WriteLine("\nPlease insert the date: (Format: DD-MM--YY). Press '0' to return to main menu.");
            string date = GetDateInput();

            Console.WriteLine("\nPlease insert the number of units you would like to track. (no decimals allowed.)");
            int quantity = GetNumberInput();

            db.Insert(date, quantity);
        }

        public void GetRecords()
        {
            Console.Clear();

            List<Habit> data = new();

            db.View(data);

            foreach (var h in data)
            {
                Console.WriteLine($"{h.ID} - {h.Date.ToString("dd-MM-yyyy")} - Quantity: {h.Quantity}");
            }
        }

        public void DeleteRecords()
        {
            Console.Clear();

            GetRecords();

            Console.WriteLine("please Enter the number of the record you would like to delete. Press '0' to return to main menu.");

            int recordId = GetNumberInput();
            db.Delete(recordId);
        }

        public void UpdateRecord()
        {
            Console.Clear();
            GetRecords();
            Console.WriteLine("Please enter the number of the record you would like to update.");
            int recordId = GetNumberInput();

            bool checkValid = db.CheckValid(recordId);

            if (checkValid)
            {
                Console.WriteLine("\nPlease insert the date: (Format: DD-MM--YY). Press '0' to return to main menu.");
                string date = GetDateInput();

                Console.WriteLine("\nPlease insert the number of units you would like to track (no decimals allowed)");
                int quantity = GetNumberInput();

                db.Update(recordId, date, quantity);
            }
            else
            {
                Console.WriteLine($"Record with id {recordId} doesn't exist");
                DisplayMenu();
            }
        }

        public string GetDateInput()
        {
            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                DisplayMenu();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date. Format: dd-mm-yy. Please try again or hit '0' to exit");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        public int GetNumberInput()
        {
            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                DisplayMenu();
            }

            while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("Invalid number. Please try again or hit '0' to exit");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
