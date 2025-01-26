// See https://aka.ms/new-console-template for more information
using s10268292_Prg2Assignment;
//Basic feature #2
Dictionary<string, Flight> flightDictionary = new Dictionary<string, Flight>();
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
        if (!flightDictionary.ContainsKey(flightNumber))
        {
            flightDictionary.Add(flightNumber, flight);
        }
        else
        {
            Console.WriteLine("Flight error detected");
        }
    }
}


