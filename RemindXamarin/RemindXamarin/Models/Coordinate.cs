namespace RemindXamarin.Models
{
    public class Coordinate
    {
        private double lat;
        private double lng;
        private double h;

        public Coordinate(double lat, double lng, double height){
            this.lat = lat;
            this.lng = lng;
            this.h = height;
        }

        public double getLat() {return this.lat;}
        public void setLat(double lat) {this.lat = lat;}

        public double getLng() {return this.lng;}
        public void setLng(double lng) {this.lng = lng;}

        public double getHeight() {return this.h;}
        public void setHeight(double height) {this.h = height;}
        }
}