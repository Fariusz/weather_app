using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MeteoCMD
{
    class MeteoDBEncy
    {
        public string Place { get; set; }
        public string dt { get; set; }
        public string TempOut { get; set; }
        public string HiTemp { get; set; }
        public string LowTemp { get; set; }
        public string outHum { get; set; }
        public string DevPt { get; set; }
        public string WindSpeed { get; set; }
        public string WindDir { get; set; }
        public string WindRun { get; set; }
        public string HiSpeed { get; set; }
        public string HDir { get; set; }
        public string WindHill { get; set; }
        public string HeatIndex { get; set; }
        public string THWIndex { get; set; }
        public string Bar { get; set; }
        public string Rain { get; set; }
        public string RainRate { get; set; }
        public string HeatDD { get; set; }
        public string CoolDD { get; set; }
        public string InTemp { get; set; }
        public string InHum { get; set; }
        public string InDev { get; set; }
        public string InHeat { get; set; }
        public string InEMC { get; set; }
        public string InAirDensity { get; set; }
        public string WindTx { get; set; }
        public string ISSRec { get; set; }
        public string Arc_int { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    class StationEncy
    {
        public string Station { get; set; }
        public string ID { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
    }
    
    class Program
    {
        static MeteoDBEncy getMeteoDB(String id)
        {
            WebClient client = new WebClient();
            String downloadedString = client.DownloadString("http://infomet.nazwa.pl/data/values.php?stationid=" + id);

            MeteoDBEncy meteoData = JsonConvert.DeserializeObject<MeteoDBEncy>(downloadedString);

            return meteoData;
        }

        static IList<StationEncy> getStationsList()
        {
            WebClient client = new WebClient();
            String downloadedString = client.DownloadString("http://infomet.nazwa.pl/data/stations.php");

            IList <StationEncy> stationList = JsonConvert.DeserializeObject<IList<StationEncy>>(downloadedString);

            return stationList;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Lista dostępnych stacji: ");
            IList<StationEncy> stacje = getStationsList();
            for (int i = 0; i < stacje.Count(); i++)
            {
                Console.Write("[" + (i+1) + "] ");
                Console.WriteLine(stacje[i].Station);
            }

            int numer;
            do
            {
                bool test = false;
                do
                {
                    Console.Write("\nWybierz nr stacji: ");
                    test = int.TryParse(Console.ReadLine(), out numer);

                    if (!test) Console.WriteLine("Niepoprawna wartość!");
                } while (test == false);
                if (numer < 1 || numer >= stacje.Count() + 1) Console.WriteLine("Podaj liczbe z zakresu numerów stacji!");
            } while (numer < 1 || numer >= stacje.Count() + 1);

            StationEncy stacja = stacje[numer-1];
            Console.WriteLine("\nDane pogodowe dla wybranej stacji: " + stacja.Station);

            MeteoDBEncy danepogodowe = getMeteoDB(stacja.ID);
            Console.WriteLine("Czas aktualizacji: " + danepogodowe.dt);
            Console.WriteLine("Temperatura: " + danepogodowe.TempOut);
            Console.WriteLine("Predkosc wiatru: " + danepogodowe.WindSpeed);
            Console.WriteLine("Kierunek wiatru: " + danepogodowe.WindDir);

            Console.ReadLine();
        }
    }
}
