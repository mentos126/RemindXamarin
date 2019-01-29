using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindXamarin.Models
{
    [Table(nameof(SportTache))]
    public class SportTache
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateDeb { get; set; }
        public int TimeHour { get; set; }
        public int TimeMinutes { get; set; }
        public int Steps { get; set; }
        public int Distance { get; set; }
        public long Duration { get; set; }
        public String Coordinates { get; set; }

    }
}
