using KinoBot2.DAO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KinoBot2.Parsers
{
    public class Transformer
    {
        public List<WeekResponse> uniteResponses(List<WeekResponse> weekResponse, List<MoviesResponse> moviesResponse, List<HallsResponse> hallsResponse)
        {
            foreach (WeekResponse wr in weekResponse)
            {
                MoviesResponse mr = moviesResponse.First(item => item.id == wr.movieId);
                wr.movie = mr;
                HallsResponse hr = hallsResponse.First(item => item.id == wr.hallId);
                wr.hall = hr;
                wr.dateTime  = DateTime.ParseExact(wr.date+" "+wr.time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            return weekResponse;
        }

        public List<string> getAllMovies(List<WeekResponse> weekResponse)
        {
            HashSet<string> movies = new HashSet<string>();
            foreach (WeekResponse wr in weekResponse)
            {
                movies.Add(wr.movie.title);
            }
            return movies.ToList();
        }

        public List<string> getMovieDates(List<WeekResponse> weekResponse, string movieName)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (WeekResponse wr in weekResponse)
            {
                if (wr.movie.title.Equals(movieName))
                {
                    dates.Add(wr.dateTime);
                }
            }
            dates.Sort();

            //TODO сортировка списка дат
            List<string> datesAsStrings = new List<string>();
            foreach (DateTime dt in dates)
            {
                datesAsStrings.Add(dt.Date.ToString("yyyy-MM-dd"));
            }

            return datesAsStrings;
        }

        public Dictionary<string, List<string>> getMovieTimes(List<WeekResponse> weekResponse, string movieName, string movieDate)
        {
            List<DateTime> times2D = new List<DateTime>();
            List<DateTime> times3D = new List<DateTime>();
            foreach (WeekResponse wr in weekResponse)
            {
                if (wr.movie.title.Equals(movieName) && wr.dateTime.Date.ToString("yyyy-MM-dd").Equals(movieDate))
                {
                    if (wr.formats[0].Equals("3d"))
                    {
                        times3D.Add(wr.dateTime);
                    }
                    else
                    {
                        times2D.Add(wr.dateTime);
                    }
                }
            }
            times2D.Sort();
            times3D.Sort();
            Dictionary<string, List<string>> formatsMap = new Dictionary<string, List<string>>();
             //TODO сортировка списка дат
            List <string> timesAsStrings2D = new List<string>();
            List<string> timesAsStrings3D = new List<string>();
            foreach (DateTime dt in times2D)
            {
                //string s = dt.Hour.ToString().Length == 1 ? "0" + dt.Hour.ToString() : dt.Hour.ToString();
                timesAsStrings2D.Add((dt.Hour.ToString().Length == 1 ? "0" + dt.Hour.ToString() : dt.Hour.ToString()) + ":"+ (dt.Minute.ToString().Length == 1 ? "0" + dt.Minute.ToString() : dt.Minute.ToString()));
            }
            foreach (DateTime dt in times3D)
            {
                timesAsStrings3D.Add((dt.Hour.ToString().Length == 1 ? "0" + dt.Hour.ToString() : dt.Hour.ToString()) + ":" + (dt.Minute.ToString().Length == 1 ? "0" + dt.Minute.ToString() : dt.Minute.ToString()));

            }
            if (timesAsStrings2D.Count > 0)
            {
                formatsMap.Add("2D", timesAsStrings2D);
            }
            if (timesAsStrings3D.Count > 0)
            {
                formatsMap.Add("3D", timesAsStrings3D);
            }
            return formatsMap;
        }


        public string getSeanse(List<WeekResponse> weekResponse, string movieName, string movieDate, string movieTime)
        {
            List<DateTime> times2D = new List<DateTime>();
            List<DateTime> times3D = new List<DateTime>();
            string result="";
            foreach (WeekResponse wr in weekResponse)
            {
                if (wr.movie.title.Equals(movieName) && wr.dateTime.Date.ToString("yyyy-MM-dd").Equals(movieDate) && wr.time.Equals(movieTime))
                {
                    result = "Ссылка на форму покупки билетов: \n\r" +
                        "http://kinohod.ru/widget/seances/" + wr.id + "\n\r" +
                        "Напишите 'Фильмы на неделе', чтобы получить список фильмов";
                }
            }
            return result;
        }
    }
}