using System;

namespace RemindXamarin.Models
{
    [Serializable]
    public class Task
    {
        private int ID;
        // private UUID workID;
        private String name;
        private String description;
        private Category category;
        private DateTime dateDeb;
        private int warningBefore;
        private Boolean isActivatedNotification;
        private int timeHour;
        private int timeMinutes;
        private Boolean[] repete ;

   
        private void setup(String name, String description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes) {

            DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.ID =  (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            // this.workID = null;
            this.name = name;
            this.description = description;
            this.category = category;
            this.dateDeb = dateDeb;
            this.warningBefore = warningBefore;
            this.setIsActivatedNotification(true);
            this.setTimeHour(timeHour);
            this.setTimeMinutes(timeMinutes);
        }

        public Task(String name, String description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes) {
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
        
        public Task(String name, String description, Category category, DateTime dateDeb, int warningBefore, int timeHour, int timeMinutes, Boolean[] repete) {
            this.setup(name, description, category, dateDeb, warningBefore, timeHour, timeMinutes);
            this.repete = repete;
        }

        public void setID(int ID) { this.ID = ID;}
        public int getID() {return ID;}

        // public UUID getWorkID() { return workID; }
        // public void setWorkID(UUID uuid) { workID = uuid; }

        public String getName() {return name;}
        public void setName(String name) {this.name = name;}
        
        public String getDescription() {return description;}
        public void setDescription(String description) {this.description = description;}
        
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

        public String toString() {
            String r = "";
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
                DateTime now = new DateTime();
                int day=0;
                int first = 0;
                for(int i=0; i<getRepete().Length; i++){
                    if(getRepete()[i] && first == 0){
                        first = i;
                    }
                    if((i+1 >= now. && getRepete()[i]){
                        day = i;
                        break;
                    }
                }
                if(day == 0){
                    day = first + 7;
                }
                DateTime c = new DateTime(now.get(DateTime.YEAR), now.get(DateTime.MONTH), now.get(DateTime.DAY_OF_MONTH), getTimeHour(), getTimeMinutes());
                c.Add(DateTime.DAY_OF_MONTH, day+2 - now.get(DateTime.DAY_OF_WEEK));
                return c;
            }else{
                DateTime c = new DateTime(dateDeb.get(DateTime.YEAR), dateDeb.get(DateTime.MONTH), dateDeb.get(DateTime.DAY_OF_MONTH), getTimeHour(), getTimeMinutes());
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