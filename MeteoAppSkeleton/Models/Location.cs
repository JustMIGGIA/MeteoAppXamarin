using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MeteoAppSkeleton.Models
{
    public class Location
    {
        [PrimaryKey]
        public string ID {get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string Icon { get; set; }

        public double Temp { get; set; }

        public double TempMin { get; set; }

        public double TempMax { get; set; }

        public double Pressure { get; set; }

        public double Humidity { get; set; }
        public bool Gps { get; set; }


    }
}
