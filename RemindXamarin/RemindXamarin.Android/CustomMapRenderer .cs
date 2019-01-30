using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemindXamarin.Droid;
using RemindXamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(PolylineMap), typeof(CustomMapRenderer))]
namespace RemindXamarin.Droid
{

        public class CustomMapRenderer : MapRenderer
        {
            List<Position> routeCoordinates;

            public CustomMapRenderer(Context context) : base(context)
            {
            }

            protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
            {
                base.OnElementChanged(e);

                if (e.OldElement != null)
                {
                    // Unsubscribe
                }

                if (e.NewElement != null)
                {
                    var formsMap = (PolylineMap)e.NewElement;
                    routeCoordinates = formsMap.RouteCoordinates;
                    Control.GetMapAsync(this);
                }
            }

            protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
            {
                base.OnMapReady(map);

                var polylineOptions = new PolylineOptions();
                polylineOptions.InvokeColor(Android.Graphics.Color.Blue);
                polylineOptions.InvokeWidth(20);

                foreach (var position in routeCoordinates)
                {
                    polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
                }

                NativeMap.AddPolyline(polylineOptions);
            }
        }
    }
