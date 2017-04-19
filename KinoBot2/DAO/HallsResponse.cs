using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoBot2.DAO
{

    public class HallsResponse
    {
        public bool isIMAX { get; set; }
        public object description { get; set; }
        public int? placeCount { get; set; }
        public int cinemaId { get; set; }
        public bool isVIP { get; set; }
        public bool ticketScanner { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool orderScanner { get; set; }
    }

}