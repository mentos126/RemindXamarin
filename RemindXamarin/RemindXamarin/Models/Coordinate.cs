namespace RemindXamarin.Models
{
    public class Coordinate
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public double h { get; set; }

        public Coordinate(double lat, double lng, double height){
            this.lat = lat;
            this.lng = lng;
            this.h = height;
        }

    }
}