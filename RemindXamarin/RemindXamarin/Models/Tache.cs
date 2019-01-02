using System;

namespace RemindXamarin.Models
{
    public class Tache
    {
        public int ID { get; set; }
        // private UUID workID;
        public string name { get; set; }
        public string description { get; set; }
        public Category category { get; set; }
        public DateTime? dateDeb { get; set; }
        public int warningBefore { get; set; }
        public Boolean isActivatedNotification { get; set; }
        public int timeHour { get; set; }
        public int timeMinutes { get; set; }
        public Boolean[] repete { get; set; }
        public String photo { get; set; }
        public bool isTakePhoto
        {
            get
            {
                return !photo.Equals("");;
            }
        }
        public bool isNotTakePhoto
        {
            get
            {
                return photo.Equals(""); ;
            }
        }

        public String NextDate
        {
            get
            {
                return getNextDate().ToString("dd/M/yyyy");

            }
        }
        public string formatedTime
        {
            get
            {
                return string.Format("{0} : {1}", timeHour, timeMinutes);
            }
        }

        private void setup(string name, string description, Category category, DateTime? dateDeb, int warningBefore, int timeHour, int timeMinutes) {

            DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.ID =  (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            // this.workID = null;
            this.name = name;
            this.description = description;
            this.category = category;
            this.dateDeb = dateDeb;
            this.warningBefore = warningBefore;
            this.isActivatedNotification = true;
            this.timeHour = timeHour;
            this.timeMinutes = timeMinutes;
            this.photo = "";
        }

        public Tache(string name, string description, Category category, DateTime? dateDeb, int warningBefore, int timeHour, int timeMinutes) {
            this.setup(name, description, category, dateDeb, warningBefore, timeHour, timeMinutes);
            this.repete = new Boolean[] {
                    false, 
                    false, 
                    false, 
                    false, 
                    false, 
                    false, 
                    false,    
                    };
        }
        
        public Tache(string name, string description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes, Boolean[] repete) {
            this.setup(name, description, category, dateDeb, warningBefore, timeHour, timeMinutes);
            this.repete = repete;
        }

        public string toString() {
            string r = "";
            foreach(Boolean x in this.repete) {					
                r+="\n\t\t"+x;
            }
            return " [ "
                    + "\n\tID : "+ID
                    + "\n\t"+name
                    + "\n\t"+description
                    + "\n\t"+category
                    + "\n\t"+dateDeb
                    + "\n\t"+warningBefore
                    + "\n\t"+timeHour
                    + "\n\t"+timeMinutes
                    + "\n\t"+isActivatedNotification
                    +"\n\t["
                    +r
                    +"\n\t]"
                    +"\n] ";
        }

        public DateTime getNextDate(){
            if(dateDeb == null){
                DateTime now = DateTime.Now;
                int day=0;
                int first = 0;
                for(int i=0; i< repete.Length; i++){
                    if(repete[i] && first == 0){
                        first = i;
                    }
                    if(i+1 >= (int)now.DayOfWeek && repete[i]) {
                        day = i;
                        break;
                    }
                }
                if(day == 0){
                    day = first + 7;
                }
                DateTime c = new DateTime(now.Year, now.Month, now.Day, timeHour, timeMinutes, 0);
                c.AddDays(day + 2 - (int)now.DayOfWeek);
                return c;
            }else{
                DateTime c = new DateTime(dateDeb.Value.Year, dateDeb.Value.Month, dateDeb.Value.Day, timeHour, timeMinutes, 0);
                return c;
            }
        }

    }
}