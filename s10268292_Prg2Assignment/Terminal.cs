using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10268292E
// Student Name : Chong Yi Kai David
// Partner Name : Chase Choo Yan Hee
//==========================================================
namespace s10268292_Prg2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();
        *-
        public Terminal() { }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }


        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                return false;
            }
            Airlines[airline.Code] = airline;
            return true;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (BoardingGates.ContainsKey(gate.GateName))
            {
                return false;
            }
            BoardingGates[gate.GateName] = gate;
            return true;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            string flightCode = flight.FlightNumber.Substring(0, 2);
            Airline? airline = null;

            foreach (KeyValuePair<string, Airline> kvp in Airlines)
            {
                if (kvp.Key == flightCode)
                {
                    airline = kvp.Value;
                }
            }
            return airline;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"{airline.Name}: ${airline.CalculateFees():F2}");
            }
        }


        public override string ToString()
        {
            return "";
        }
    }
}