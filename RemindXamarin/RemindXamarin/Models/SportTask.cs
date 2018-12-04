using System;
using System.Collections;

namespace RemindXamarin.Models
{
    public class SportTask : Task
    {
        private ArrayList listCoord = new ArrayList();
        private int steps;
        private int heart;
        private int distance;
        private long duration;


        public SportTask(
            String name, String description, Category category, DateTime dateDeb,
            int warningBefore, int timeHour, int timeMinutes, Boolean[] repete,
            int steps, int heart, int distance, long duration
        ) :  base(name, description, category, dateDeb, warningBefore, timeHour, timeMinutes, repete) {

            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.setID((int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
            // this.workID = null;
            this.setName(name);
            this.setDescription(description);
            this.setCategory(category);
            this.setDateDeb(dateDeb);
            this.setWarningBefore(warningBefore);
            this.setIsActivatedNotification(false);
            this.setTimeHour(timeHour);
            this.setTimeMinutes(timeMinutes);

            this.distance = distance;
            this.steps = steps;
            this.heart = heart;
            this.duration = duration;
        }

        public void addCoord(Coordinate c){listCoord.Add(c);}
        public ArrayList getListCoord() {return listCoord;}

        public int getSteps() {return steps;}
        public void setSteps(int steps) {this.steps = steps;}

        public int getHeart() {return heart;}
        public void setHeart(int heart) {this.heart = heart;}

        public int getDistance() {return distance;}
        public void setDistance(int distance) {this.distance = distance;}

        //TODO a tester
        public void caculateDistance(){
            double res = 0;
            for(int i=0;i<listCoord.Count;i++){
                if(i!=0){
                    res += distanceBetween((Coordinate)listCoord[i-1], (Coordinate)listCoord[i]);
                }
            }
            setDistance((int)res);
        }

        private double distanceBetween(Coordinate c1, Coordinate c2) {
            int R = 6371; // Radius of the earth
            double latDistance = this.ConvertDegreesToRadians(c2.getLat() - c1.getLat());
            double lonDistance = this.ConvertDegreesToRadians(c2.getLng() - c1.getLng());
            double a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                    + Math.Cos(this.ConvertDegreesToRadians(c1.getLat())) * Math.Cos(this.ConvertDegreesToRadians(c2.getLat()))
                    * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c * 1000; // convert to meters

            double height = c1.getHeight() - c2.getHeight();

            distance = Math.Pow(distance, 2) + Math.Pow(height, 2);

            return Math.Sqrt(distance);
        }

        public double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        public long getDuration() {return this.duration;}
        public void setDurationSecondes(long duration) {
            this.duration = duration;
        }

    }
}
