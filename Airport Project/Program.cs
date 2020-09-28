using System;
using System.IO;

namespace Airport_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice = -1;
            bool isLiverpool = true;
            string[] airportDetails = new string[] { "Void"};
            string[] numOfPassengers = new string[] { "Void" };

            do
            {
                choice = Menu();
                if (choice == 1)
                {
                    airportDetails = AirportDetails().Split(",");
                    if (airportDetails[0] == "BOH")
                    {
                        isLiverpool = false;
                    }
                    //Airport code, Airport Name, LPL dist, BOH dist
                }
                else if (choice == 2)
                {
                    numOfPassengers = FlightDetails().Split(",");
                    //Type, cost per seat per km, range, capacity-standard, min first class, num of passengers, first class, total
                }
                else if (choice == 3)
                {
                    Console.WriteLine(CalculateProfit(airportDetails, numOfPassengers, isLiverpool));
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                }

            } while (choice != 3 && choice != -1);
            Console.ReadLine();
        }

        static int Menu()
        {
            int choice = -1;
            do
            {
                Console.WriteLine("1. Enter Airport Details.");
                Console.WriteLine("2. Enter Flight Details.");
                Console.WriteLine("3. Enter Price Plan and Calculate Profits.");
                Console.WriteLine("Choose 1, 2 or 3. If you want to quit, press enter : ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid Choice... ");
                }
            } while (choice == -1);

            return choice;

        }

        static string AirportDetails()
        {
            const int numOfValsPerRow = 4;
            string ukAirport = "";
            string abroad = "";

            string[] readAirports = readFile("Airports");

            do
            {
                Console.WriteLine("Choose UK Airport: Liverpool John Lennon (type LPL) or Bourenmouth International (type BOH) : ");
                ukAirport = Console.ReadLine();
            } while (ukAirport != "LPL" && ukAirport != "BOH");

            Console.WriteLine("Enter abroad airport code: ");
            abroad = Console.ReadLine();

            return validateFileData(readAirports, abroad, numOfValsPerRow);
        }

        static string FlightDetails()
        {
            const int numOfValsPerRow = 5;
            string data = "";
            string planeInput = "";
            int firstClassSeats = 0;
            int minimumFirstClass = 0;
            int totalSeats = 0;
            string[] planeTypes = readFile("Planes");
            do
            {
                Console.WriteLine("Choose an Aircraft Type: ");
                Console.WriteLine("Medium narrow body, Large narrow body Or Medium wide body");

                planeInput = Console.ReadLine();
            } while (validateFileData(planeTypes, planeInput, numOfValsPerRow).Length < 20);
            Console.WriteLine(validateFileData(planeTypes, planeInput, numOfValsPerRow));
            string[] chosenPlane = validateFileData(planeTypes, planeInput, numOfValsPerRow).Split(',');

            do
            {
                try
                {
                    Console.WriteLine("Enter num of first class seats: ");
                    firstClassSeats = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("That's not a number!");
                }

                minimumFirstClass = Convert.ToInt32(chosenPlane[numOfValsPerRow - 1]);
                totalSeats = Convert.ToInt32(chosenPlane[numOfValsPerRow - 2]);
            } while (firstClassSeats < minimumFirstClass || firstClassSeats > totalSeats / 2);

            data = string.Join(',', chosenPlane) + "," + Convert.ToString(totalSeats - 2 * firstClassSeats) + "," + Convert.ToString(firstClassSeats) + "," + Convert.ToString(totalSeats);
            return data;

        }

        static int CalculateProfit(string[] airport, string[] flight, bool isLiverpool)
        {
            int distStored = -1;
            if (isLiverpool)
            {
                distStored = Convert.ToInt32(airport[2]);
            }
            else
            {
                distStored = Convert.ToInt32(airport[3]);
            }
            Console.WriteLine("How much does a first class seat cost: ");
            int firstClassCost = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Standard seat cost: ");
            int standardCost = Convert.ToInt32(Console.ReadLine());

            int numOfPassengers = Convert.ToInt32(flight[flight.Length - 1]);
            int flightCostPerSeat = Convert.ToInt32(flight[5]) * distStored / 100;
            int flightCost = flightCostPerSeat * numOfPassengers;
            int flightIncome = (Convert.ToInt32(flight[flight.Length - 2]) * firstClassCost) + ((Convert.ToInt32(flight[flight.Length - 1]) - Convert.ToInt32(flight[flight.Length - 2])) * standardCost);
            return flightIncome - flightCost;
        }

        static string[] readFile(string filename)
        {
            string contents = "";
            string[] terms = new string[5];
            contents = File.ReadAllText(filename + ".txt");
            terms = contents.Split(',');

            return terms;
        }

        static string validateFileData(string[] fileContents, string val, int numOfValsPerRow)
        {

            bool found = false;
            int count = 0;

            while (!found && count < fileContents.Length)
            {
                if (fileContents[count] == val || fileContents[count] == "\r\n" + val)
                {
                    found = true;
                    for (int i = 1; i < numOfValsPerRow; i++)
                    {
                        val += "," + fileContents[count + 1];
                        count++;
                    }
                }
                else
                {
                    count += numOfValsPerRow;
                }
            }

            return val;
        }

    }
}
