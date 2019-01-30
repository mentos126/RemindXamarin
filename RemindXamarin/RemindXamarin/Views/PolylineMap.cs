using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace RemindXamarin.Views
{
    public class PolylineMap : Map
    {
        public List<Position> RouteCoordinates { get; set; }

        public PolylineMap()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
