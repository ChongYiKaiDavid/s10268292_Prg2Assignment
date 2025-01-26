// See https://aka.ms/new-console-template for more information
using s10268292_Prg2Assignment;
//Basic feature #1
Terminal terminal = new Terminal("Changi Terminal 5");
LoadAirlines(terminal);
LoadBoardingGates(terminal);

static void LoadAirlines(Terminal terminal)
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
        }
    }
}
static void LoadBoardingGates(Terminal terminal)
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
        }
    }
}
//Basic feature #2

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
        flights.Add(flightNumber, flight);
    }
}
//Basic feature 3
//Display menu
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
            ListAllFLights(flightDictionary);
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
void ListAllFLights(Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("=============================================\r\nList of Flights for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Flight Number" +
        "", -15} {"Airline Name", -20} { "Origin", -15} {"Destination", -15} {"Expected Departure/Arrival Time", -20}");
    foreach (var flight in flightDictionary.Values)
    {
        Airline airline = new Terminal().GetAirlineFromFlight(flight); //NOT DONE YET STILL NEED DO BASIC FEATURE 1 FIRST THEN IT WILL WORK

        Console.WriteLine($"{flight.FlightNumber, -15} {airline.Name, -20} {flight.Origin, -15} {flight.Destination, -15} {flight.ExpectedTime, -20}");
    }
}

