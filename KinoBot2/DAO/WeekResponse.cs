using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace KinoBot2.DAO
{

    public class WeekResponse
    {
        public string date { get; set; }
        public string time { get; set; }
        public int movieId { get; set; }
        public int minPrice { get; set; }
        public object subtitleId { get; set; }
        public string[] formats { get; set; }
        public int cinemaId { get; set; }
        public bool isSaleAllowed { get; set; }
        public string id { get; set; }
        public string startTime { get; set; }
        public int maxPrice { get; set; }
        public string timeZone { get; set; }
        public int maxSeatsInOrder { get; set; }
        public int groupOrder { get; set; }
        public object languageId { get; set; }
        public int timeBeforeSeance { get; set; }
        public int hallId { get; set; }
        public string groupName { get; set; }
        public HallsResponse hall { get; set; }
        public MoviesResponse movie { get; set; }
        public DateTime dateTime { get; set; }
    }

}