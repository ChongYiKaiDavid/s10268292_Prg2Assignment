// See https://aka.ms/new-console-template for more information
using s10268292_Prg2Assignment;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
            bool reqDDJB = Convert.ToBoolean(boardingGates[1]);
            bool reqCFFT = Convert.ToBoolean(boardingGates[2]);
            bool reqLWTT = Convert.ToBoolean(boardingGates[3]);

            BoardingGate gate = new BoardingGate(gateName, reqDDJB, reqCFFT, reqLWTT);
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
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.Write("Please select your option: ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                ListAllFLights(terminal);
            }
            else if (choice == "2")
            {
                ListBoardingGates(terminal);
            }
            else if (choice == "3")
            {
                AssignBoardingGate(terminal);
            }
            //else if (choice == "4")
            //{
            //    CreateFlight(terminal);
            //}
            else if (choice == "5")
            {
                DisplayAirlineFlights(terminal);
            }
            else if (choice == "6")
            {
                ModifyFlightDetails(terminal);
            }
            //else if (choice == "7")
            //{
            //    DisplayFlightSchedule(terminal);
            //}
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
    //Basic feature 4
void ListBoardingGates(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n=============================================");
    Console.WriteLine($"{"Gate Name",-16} {"Supports DDJB",-23} {"Supports CFFT",-27} {"Supports LWTT",-25}");
    foreach (var gate in terminal.BoardingGates.Values)
    {
        Console.WriteLine($"{gate.GateName,-16} {gate.SupportsDDJB,-23} {gate.SupportsCFFT,-27} {gate.SupportsLWTT,-25}");
    }
}

//Basic feature 5
void AssignBoardingGate(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nAssign a Boarding Gate to a Flight\r\n=============================================");
    Flight flight;
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().ToUpper();
        if (!terminal.Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine("Flight does not exist");
            return;
        }
        else
        {
            flight = terminal.Flights[flightNumber];
            break;
        }
    }
    
    

    

    BoardingGate gate;
    string? gateName = null;
    while (true)
    {
        Console.Write("Enter Boarding Gate Name: ");
        gateName = Console.ReadLine().ToUpper();
        if (!terminal.BoardingGates.ContainsKey(gateName))
        {
            Console.WriteLine("Gate does not exist");
            return;
        }
        else
        {
            gate = terminal.BoardingGates[gateName];
            if (gate.Flight != null)
            {
                Console.WriteLine("This gate is already assigned to another flight. Please try again.");
            }
            else
            {
                break;
            }
        }
    }
    
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {(flight is CFFTFlight ? "CFFT" : flight is DDJBFlight ? "DDJB" : flight is LWTTFlight ? "LWTT" : "None")}");

    BoardingGate boardinggate = terminal.BoardingGates[gateName];

            Console.WriteLine($"Boarding Gate Name: {boardinggate.GateName}");
            Console.WriteLine($"Supports DDJB: {boardinggate.SupportsDDJB}");
            Console.WriteLine($"Supports CFFT: {boardinggate.SupportsCFFT}");
            Console.WriteLine($"Supports LWTT: {boardinggate.SupportsLWTT}");
        
        
   

    gate.Flight = flight;
    // Allow user to update the flight status
    Console.WriteLine("Would you like to update the status of the flight? (Y/N): ");
    string updateStatus = Console.ReadLine().ToUpper();
    if (updateStatus == "Y")
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.Write("Please select the new status of the flight: ");
        string statusChoice = Console.ReadLine();

        if (statusChoice == "1")
        {
            flight.Status = "Delayed";
        }
        else if (statusChoice == "2")
        {
            flight.Status = "Boarding";
        }
        else if (statusChoice == "3")
        {
            flight.Status = "On Time";
        }
        else
        {
            Console.WriteLine("Invalid choice, setting status to 'On Time' by default.");
            flight.Status = "On Time";
        }
    }
    Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {gate.GateName}!");
}

//Basic Feature 7
void DisplayAirlineFlights(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"Airline Code: {airline.Code}, Airline Name: {airline.Name}");
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Airline does not exist.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];
    Console.WriteLine($"=============================================\r\nList of Flights for {selectedAirline.Name}\r\n=============================================");
    Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-15} {"Destination",-15} {"Expected Departure/Arrival Time",-30}");

    foreach (var flight in terminal.Flights.Values.Where(f => terminal.GetAirlineFromFlight(f).Code == airlineCode))
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {selectedAirline.Name,-20} {flight.Origin,-15} {flight.Destination,-15} {flight.ExpectedTime,-30}");
    }

    Console.Write("Enter the Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();

    if (!terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight does not exist.");
        return;
    }

    Flight selectedFlight = terminal.Flights[flightNumber];
    BoardingGate? gate = terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight == selectedFlight);

    Console.WriteLine("=============================================\r\nFlight Details\r\n=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Special Request Code: {(selectedFlight is CFFTFlight ? "CFFT" : selectedFlight is DDJBFlight ? "DDJB" : selectedFlight is LWTTFlight ? "LWTT" : "None")}");
    Console.WriteLine($"Boarding Gate: {(gate != null ? gate.GateName : "None")}");
}

//Basic Feature 8
void ModifyFlightDetails(Terminal terminal)
{
    Console.WriteLine("=============================================\r\nList of Airlines for Changi Airport Terminal 5\r\n=============================================");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"Airline Code: {airline.Code}, Airline Name: {airline.Name}");
    }

    string airlineCode;
    while (true)
    {
        Console.Write("Enter Airline Code: ");
        airlineCode = Console.ReadLine().ToUpper();
        if (string.IsNullOrEmpty(airlineCode) || !terminal.Airlines.ContainsKey(airlineCode))
        {
            Console.WriteLine("Invalid Airline Code. Please try again.");
        }
        else
        {
            break;
        }
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];
    Console.WriteLine($"=============================================\r\nList of Flights for {selectedAirline.Name}\r\n=============================================");
    Console.WriteLine($"{"Flight Number",-15} {"Origin",-15} {"Destination",-15} {"Expected Departure/Arrival Time",-30}");

    foreach (var flight in terminal.Flights.Values.Where(f => terminal.GetAirlineFromFlight(f).Code == airlineCode))
    {
        Console.WriteLine($"{flight.FlightNumber,-15} {flight.Origin,-15} {flight.Destination,-15} {flight.ExpectedTime,-30}");
    }

    string flightNumber;
    while (true)
    {
        Console.Write("Choose an existing Flight to modify or delete: ");
        flightNumber = Console.ReadLine().ToUpper();
        if (string.IsNullOrEmpty(flightNumber) || !terminal.Flights.ContainsKey(flightNumber))
        {
            Console.WriteLine("Invalid Flight Number. Please try again.");
        }
        else
        {
            break;
        }
    }

    Flight selectedFlight = terminal.Flights[flightNumber];

    string option;
    while (true)
    {
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        Console.Write("Choose an option: ");
        option = Console.ReadLine();
        if (option != "1" && option != "2")
        {
            Console.WriteLine("Invalid option. Please choose 1 or 2.");
        }
        else
        {
            break;
        }
    }

    if (option == "1")
    {
        string modifyOption;
        while (true)
        {
            Console.WriteLine("1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("Choose an option: ");
            modifyOption = Console.ReadLine();
            if (modifyOption != "1" && modifyOption != "2" && modifyOption != "3" && modifyOption != "4")
            {
                Console.WriteLine("Invalid option. Please choose 1, 2, 3, or 4.");
            }
            else
            {
                break;
            }
        }

        if (modifyOption == "1")
        {
            Console.Write("Enter new Origin: ");
            selectedFlight.Origin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            selectedFlight.Destination = Console.ReadLine();
            while (true)
            {
                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newExpectedTime))
                {
                    selectedFlight.ExpectedTime = newExpectedTime;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }
        }
        else if (modifyOption == "2")
        {
            Console.Write("Enter new Status: ");
            selectedFlight.Status = Console.ReadLine();
        }
        else if (modifyOption == "3")
        {
            Console.Write("Enter new Special Request Code: ");
            string specialRequestCode = Console.ReadLine();
            if (specialRequestCode == "CFFT")
            {
                selectedFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
            }
            else if (specialRequestCode == "DDJB")
            {
                selectedFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
            }
            else if (specialRequestCode == "LWTT")
            {
                selectedFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
            }
            else
            {
                selectedFlight = new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime);
            }
            terminal.Flights[flightNumber] = selectedFlight;
        }
        else if (modifyOption == "4")
        {
            while (true)
            {
                Console.Write("Enter new Boarding Gate: ");
                string gateName = Console.ReadLine().ToUpper();
                if (string.IsNullOrEmpty(gateName) || !terminal.BoardingGates.ContainsKey(gateName))
                {
                    Console.WriteLine("Invalid Boarding Gate. Please try again.");
                }
                else
                {
                    BoardingGate gate1 = terminal.BoardingGates[gateName];
                    gate1.Flight = selectedFlight;
                    break;
                }
            }
        }
        Console.WriteLine("Flight updated!");
    }
    else if (option == "2")
    {
        while (true)
        {
            Console.Write("Are you sure you want to delete this flight? (Y/N): ");
            string confirm = Console.ReadLine().ToUpper();
            if (confirm == "Y")
            {
                terminal.Flights.Remove(flightNumber);
                Console.WriteLine("Flight deleted!");
                return;
            }
            else if (confirm == "N")
            {
                Console.WriteLine("Flight deletion canceled.");
                return;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter Y or N.");
            }
        }
    }

    Console.WriteLine("=============================================\r\nUpdated Flight Details\r\n=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime}");
    Console.WriteLine($"Status: {selectedFlight.Status}");
    Console.WriteLine($"Special Request Code: {(selectedFlight is CFFTFlight ? "CFFT" : selectedFlight is DDJBFlight ? "DDJB" : selectedFlight is LWTTFlight ? "LWTT" : "None")}");
    BoardingGate? gate = terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight == selectedFlight);
    Console.WriteLine($"Boarding Gate: {(gate != null ? gate.GateName : "None")}");
}
