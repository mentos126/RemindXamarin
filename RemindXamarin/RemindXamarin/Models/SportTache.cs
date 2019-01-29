using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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

        public Color CatColor
        {
            get
            {
                return Color.FromHex("adeaea");
            }
        }

        public String CatIcon
        {
            get
            {
                return "ic_directions_run_black_36dp.png";
            }
        }

        public String CatName
        {
            get
            {
                return "Sport";
            }
        }

        public String TimeHourFormated
        {
            get
            {
                String res = "";
                if (TimeHour < 10)
                {
                    res += "0";
                }
                return res += TimeHour;
            }
        }

        public String TimeMinutesFormated
        {
            get
            {
                String res = "";
                if (TimeMinutes < 10)
                {
                    res += "0";
                }
                return res += TimeMinutes;
            }
        }

        public string FormatedTime
        {
            get
            {
                return string.Format("{0} : {1}", TimeHourFormated, TimeMinutesFormated);
            }
        }

    }
}

