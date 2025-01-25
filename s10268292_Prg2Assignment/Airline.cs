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
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }


        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber)) return false;
            Flights[flight.FlightNumber] = flight;
            return true;
        }

        public bool RemoveFlight(string flightNumber)
        {
            return Flights.Remove(flightNumber);
        }

        public double CalculateFees()
        {
            double total = 0;
            foreach (var flight in Flights.Values)
            {
                total += flight.CalculateFees();
            }
            return total;
        }

        public override string ToString() => $"{Name} ({Code})";
    }
}