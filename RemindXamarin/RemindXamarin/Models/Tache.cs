using System;

namespace RemindXamarin.Models
{
    [Serializable]
    public class Tache
    {
        private int ID { get; set; }
        // private UUID workID;
        private string name { get; set; }
        private string description { get; set; }
        private Category category { get; set; }
        private DateTime dateDeb { get; set; }
        private int warningBefore { get; set; }
        private Boolean isActivatedNotification { get; set; }
        private int timeHour { get; set; }
        private int timeMinutes { get; set; }
        private Boolean[] repete { get; set; }


        private void setup(string name, string description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes) {

            DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.ID =  (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            // this.workID = null;
            this.name = name;
            this.description = description;
            this.category = category;
            this.dateDeb = dateDeb;
            this.warningBefore = warningBefore;
            this.isActivatedNotification = true;
            this.setTimeHour(timeHour);
            this.setTimeMinutes(timeMinutes);
        }

        public Tache(string name, string description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes) {
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

        public void setID(int ID) { this.ID = ID;}
        public int getID() {return ID;}

        // public UUID getWorkID() { return workID; }
        // public void setWorkID(UUID uuid) { workID = uuid; }

        public string getName() {return name;}
        public void setName(string name) {this.name = name;}
        
        public string getDescription() {return description;}
        public void setDescription(string description) {this.description = description;}
        
        public Category getCategory() {return category;}
        public void setCategory(Category category) {this.category = category;}
        
        public DateTime getDateDeb() {return dateDeb;}
        public void setDateDeb(DateTime dateDeb) {this.dateDeb = dateDeb;}
        
        public int getWarningBefore() {return warningBefore;}
        public void setWarningBefore(int warningBefore) {this.warningBefore = warningBefore;}
        
        public Boolean getIsActivatedNotification() {return isActivatedNotification;}
        public void setIsActivatedNotification(Boolean isActivatedNotification) {this.isActivatedNotification = isActivatedNotification;}

        public Boolean[] getRepete() {return repete;}
        public void setRepete(int index) {this.repete[index] = !this.repete[index];}
        
        public int getTimeHour() {return timeHour;}
        public void setTimeHour(int time) {this.timeHour = time;}

        public int getTimeMinutes() {return timeMinutes;}
        public void setTimeMinutes(int timeMinutes) {this.timeMinutes = timeMinutes;}

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
                for(int i=0; i<getRepete().Length; i++){
                    if(getRepete()[i] && first == 0){
                        first = i;
                    }
                    if(i+1 >= (int)now.DayOfWeek && getRepete()[i]) {
                        day = i;
                        break;
                    }
                }
                if(day == 0){
                    day = first + 7;
                }
                DateTime c = new DateTime(now.Year, now.Month, now.Day, getTimeHour(), getTimeMinutes(), 0);
                c.AddDays(day + 2 - (int)now.DayOfWeek);
                return c;
            }else{
                DateTime c = new DateTime(dateDeb.Year, dateDeb.Month, dateDeb.Day, getTimeHour(), getTimeMinutes(), 0);
                return c;
            }
        }

        private long getDateDiff(DateTime date1, DateTime date2/*, TimeUnit timeUnit*/) {
            // long diffInMillies = date2.getTime() - date1.getTime();
            // return timeUnit.convert(diffInMillies,TimeUnit.MILLISECONDS);
            return 0;
        }

        /*public long getDuration(TimeUnit timeUnit){
            Calendar cal = getNextDate();
            cal.set(Calendar.HOUR, getTimeMinutes());
            cal.set(Calendar.MINUTE, getTimeMinutes());
            cal.add(Calendar.MINUTE, -1 * getWarningBefore());
            //cal.roll(Calendar.MINUTE, getWarningBefore());
            Date mDate  = getNextDate().getTime();
            /*mDate.setHours(getTimeHour());
            mDate.setMinutes(getTimeMinutes());*/
            /*return getDateDiff(mDate,new Date(),timeUnit);
        }*/

    }
}