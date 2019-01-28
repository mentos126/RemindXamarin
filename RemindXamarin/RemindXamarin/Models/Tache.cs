using System;
using Xamarin.Forms;
using SQLite;
using Xamarin.Forms.Maps;

namespace RemindXamarin.Models
{
    public enum CategoryEnum
    {
        Sport,
        RDV,
        Anniversaire,
        Santé,
        Vacances,
        Congés,
        Courses,
        Loisirs,
        Autres,
    }

    [Table(nameof(Tache))]
    public class Tache 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get;  set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Description { get; set; }
        public CategoryEnum Category { get; set; }
        public DateTime? DateDeb { get; set; }
        public int WarningBefore { get; set; }
        public bool IsActivatedNotification { get; set; }
        public int TimeHour { get; set; }
        public String TimeHourFormated {
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
        public int TimeMinutes { get; set; }
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
        public String Photo { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
                    

         public String RepeteFormated
         {
             get
             {
                 if(DateDeb == null)
                 {
                     String s = "";
                     for(int day=0; day<7; day++ )
                     {
                         switch (day)
                         {
                             case 0:
                                 if (Monday)
                                     s += "Lun. ";
                                 break;
                             case 1:
                                 if (Tuesday)
                                     s += "Mar. ";
                                 break;
                             case 2:
                                 if (Wednesday)
                                     s += "Mer. ";
                                 break;
                             case 3:
                                 if (Thursday)
                                     s += "Jeu. ";
                                 break;
                             case 4:
                                 if (Friday)
                                     s += "Ven. ";
                                 break;
                             case 5:
                                 if (Saturday)
                                     s += "Sam. ";
                                 break;
                             case 6:
                                 if (Sunday)
                                     s += "Dim. ";
                                 break;
                             default:
                                 break;
                         }
                     }
                     return s;
                 }
                 else
                 {
                     return "Une seule fois.";
                 }
             }
         }

        public bool IsTakeGPS
        {
            get
            {
                return ! ((Lat == 999.999) && (Lng == 999.999));
            }
        }

        public bool IsTakePhoto
        {
            get
            {
                return !Photo.Equals("");;
            }
        }

        public bool IsNotTakePhoto
        {
            get
            {
                return Photo.Equals(""); ;
            }
        }

        public String NextDate
        {
            get
            {
                return GetNextDate().ToString("dd / MM / yyyy");

            }
        }

        public string FormatedTime
        {
            get
            {
                return string.Format("{0} : {1}", TimeHourFormated, TimeMinutesFormated);
            }
        }

        public String CatIcon
        {
            get
            {
                switch (this.Category)
                {
                    case CategoryEnum.Sport:
                        return "ic_directions_run_black_36dp.png";
                    case CategoryEnum.RDV:
                        return "ic_alarm_black_36dp.png";
                    case CategoryEnum.Anniversaire:
                        return "ic_cake_black_36dp.png";
                    case CategoryEnum.Santé:
                        return "ic_favorite_black_36dp.png";
                    case CategoryEnum.Vacances:
                        return "ic_card_giftcard_black_36dp.png";
                    case CategoryEnum.Congés:
                        return "ic_card_giftcard_black_36dp.png";
                    case CategoryEnum.Courses:
                        return "ic_access_alarm_black_36dp.png";
                    case CategoryEnum.Loisirs:
                        return "ic_access_time_black_36dp.png";
                    default:
                        return "ic_alarm_black_36dp.png";
                }
            }
        }

        public String CatName
        {
            get
            {
                switch (this.Category)
                {
                    case CategoryEnum.Sport:
                        return "Sport";
                    case CategoryEnum.RDV:
                        return "RDV";
                    case CategoryEnum.Anniversaire:
                        return "Anniversaire";
                    case CategoryEnum.Santé:
                        return "Santé";
                    case CategoryEnum.Vacances:
                        return "Vacances";
                    case CategoryEnum.Congés:
                        return "Congés";
                    case CategoryEnum.Courses:
                        return "Courses";
                    case CategoryEnum.Loisirs:
                        return "Loisirs";
                    default:
                        return "Autres";
                }
            }
        }

        public Color CatColor
        {
            get
            {
                switch (this.Category)
                {
                    case CategoryEnum.Sport:
                        return Color.FromHex("ff1d00");
                    case CategoryEnum.RDV:
                        return Color.FromHex("5eff00");
                    case CategoryEnum.Anniversaire:
                        return Color.FromHex("00aeff");
                    case CategoryEnum.Santé:
                        return Color.FromHex("ffd000");
                    case CategoryEnum.Vacances:
                        return Color.FromHex("b700ff");
                    case CategoryEnum.Congés:
                        return Color.FromHex("ff00f2");
                    case CategoryEnum.Courses:
                        return Color.FromHex("a7a7a76e");
                    case CategoryEnum.Loisirs:
                        return Color.FromHex("75705ae7");
                    default:
                        return Color.FromHex("755a6be7");
                }
            }
        }

        public string MyToString() {
            return " [ "
                    + "\n\tID : "+Id
                    + "\n\t"+Name
                    + "\n\t"+Description
                    + "\n\t"+Category
                    + "\n\t"+DateDeb
                    + "\n\t"+WarningBefore
                    + "\n\t"+TimeHour
                    + "\n\t"+TimeMinutes
                    + "\n\t"+IsActivatedNotification
                    +"\n\t["
                    + "\n\t\t Monday"+Monday
                    + "\n\t\t Monday"+Tuesday
                    + "\n\t\t Monday"+Wednesday
                    + "\n\t\t Monday"+Thursday
                    + "\n\t\t Monday"+Friday
                    + "\n\t\t Monday"+Saturday
                    + "\n\t\t Monday"+Sunday
                    +"\n\t]"
                    +"\n] ";
        }

        public DateTime GetNextDate(){
            if(DateDeb == null){
                DateTime now = DateTime.Now;
                int day=0;
                int first = 0;

                for(int dayT=0; day< 7; day++){

                    switch (dayT)
                    {
                        case 0:
                            if (Monday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 1:
                            if (Tuesday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 2:
                            if (Wednesday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 3:
                            if (Thursday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 4:
                            if (Friday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 5:
                            if (Saturday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        case 6:
                            if (Sunday)
                            {
                                if (first == 0)
                                {
                                    first = dayT;
                                }
                                if (dayT + 1 >= (int)now.DayOfWeek)
                                {
                                    day = dayT;
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (day == 0){
                    day = first + 7;
                }
                DateTime c = new DateTime(now.Year, now.Month, now.Day, TimeHour, TimeMinutes, 0);
                c.AddDays(day + 2 - (int)now.DayOfWeek);
                return c;
            }else{
                DateTime c = new DateTime(DateDeb.Value.Year, DateDeb.Value.Month, DateDeb.Value.Day, TimeHour, TimeMinutes, 0);
                return c;
            }
        }

        public Position GetGPSPosition
        {
            get
            {
                return new Position(Lat, Lng);
            }
        }

    }
}