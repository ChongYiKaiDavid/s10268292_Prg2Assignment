// See https://aka.ms/new-console-template for more information
using s10268292_Prg2Assignment;
//Basic feature #1

Terminal terminal = new Terminal("Changi Terminal 5");
int boardingGateCount = 0;
int airlineCount = 0;
int flightCount = 0;
Console.WriteLine("Loading Airlines...");
LoadAirlines(terminal);
Console.WriteLine("Loading Boarding Gates...");
LoadBoardingGates(terminal);
Console.WriteLine("Loading Flights...");
LoadFlights(terminal);
void LoadAirlines(Terminal terminal)
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] airlines = s.Split(',');
            string name = airlines[0];
            string code = airlines[1];
            Airline airline = new Airline(name, code);
            if (!terminal.AddAirline(airline))
            {
                Console.WriteLine("Airline already exists");
            }
            airlineCount++;
        }
        Console.WriteLine($"{airlineCount} Airlines Loaded!");
    }
}

void LoadBoardingGates(Terminal terminal)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] boardingGates = s.Split(',');
            string gateName = boardingGates[0];
            bool reqCFFT = boardingGates[1] == "1";
            bool reqDDJB = boardingGates[2] == "1";
            bool reqLWTT = boardingGates[3] == "1";

            BoardingGate gate = new BoardingGate(gateName, reqCFFT, reqDDJB, reqLWTT);
            if (!terminal.AddBoardingGate(gate))
            {
                Console.WriteLine("Boarding gate already exists");
            }
            boardingGateCount++;
        }
        Console.WriteLine($"{boardingGateCount} Boardings Gate Loaded!");
    }
}
//Basic feature #2


void LoadFlights(Terminal terminal)
{
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {

            string[] flights = s.Split(',');
            string flightNumber = flights[0];
            string origin = flights[1];
            string destination = flights[2];
            DateTime expectedTime = DateTime.Parse(flights[3]);
            string specialRequestCode = flights[4];
            Flight flight;
            if (specialRequestCode == "CFFT")
            {
                flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
            }
            else if (specialRequestCode == "DDJB")
            {
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
            }
            else if (specialRequestCode == "LWTT")
            {
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
            }
            else
            {
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
            }
            //adding flight object into the dictionary
            terminal.Flights.Add(flightNumber, flight);
            flightCount++;
        }
        Console.WriteLine($"{flightCount} Flights Loaded!");
    }
}

//Basic feature 3
//display menu
void DisplayMenu(Terminal terminal)
{
    while (true)
    {
        try
        {
            Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("0. Exit");
            Console.Write("Please select your option: ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                ListAllFLights(terminal);
            }
            else if (choice == "0")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid option, please try again");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error has occurred: {e.Message}");
            Console.WriteLine("Please try again.");
        }
    }
}
DisplayMenu(terminal);
void ListAllFLights(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Flight Number", -16} {"Airline Name", -23} { "Origin", -27} {"Destination", -25} {"Expected Departure/Arrival Time", -20}");
    foreach (var flight in terminal.Flights.Values)
    {
        Airline airline = terminal.GetAirlineFromFlight(flight);

        Console.WriteLine($"{flight.FlightNumber, -16} {airline.Name, -23} {flight.Origin, -27} {flight.Destination, -25} {flight.ExpectedTime, -20}");
    }
}

