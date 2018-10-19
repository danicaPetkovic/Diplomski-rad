using Baza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleChatSignalR.Models
{
    public class PlaceView
    {
        public int Id_place { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Nullable<int> Points { get; set; }
        public string Image { get; set; }

        public PlaceView() { }

        public PlaceView(Place place)
        {
            Id_place = place.Id_place;
            Name = place.Name;
            Description = place.Description;
            Category = place.Category;
            Longitude = float.Parse(place.Longitude);
            Latitude = float.Parse(place.Latitude);
            Points = place.Points;
            Image = place.Image;
        }
    }
}