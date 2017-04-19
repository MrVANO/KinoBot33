using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Runtime.Serialization.Json;
using KinoBot2.DAO;
using System.Text;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace KinoBot2.Parsers
{
    public class JsonParser
    {
        private string weekURL = "https://api.kinohod.ru/api/data/2/5982bb5a-1d76-31f8-abd5-c4253474ecf3/city/48/seances/week.json";
        private string movieURL = "https://api.kinohod.ru/api/data/2/5982bb5a-1d76-31f8-abd5-c4253474ecf3/city/48/running.json?_fields=id,title";
        private string hallsURL = "https://api.kinohod.ru/api/data/2/5982bb5a-1d76-31f8-abd5-c4253474ecf3/halls.json";

        public string MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    StreamReader Reader;
                    if (response.ContentEncoding.Equals("gzip"))
                    {
                        Stream Stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                        Reader = new StreamReader(Stream, Encoding.UTF8);
                    }
                    else
                    {
                        Reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    }
                    string responseString = Reader.ReadToEnd();
                    return responseString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<WeekResponse> getWeekResponse()
        {
            string response = MakeRequest(weekURL);
            List<WeekResponse> wr = JsonConvert.DeserializeObject<List<WeekResponse>>(response);
            return wr;
        }

        public List<MoviesResponse> getMoviesResponse()
        {
            string response = MakeRequest(movieURL);
            List<MoviesResponse> mr = JsonConvert.DeserializeObject<List<MoviesResponse>>(response);
            return mr;
        }

        public List<HallsResponse> getHallsResponse()
        {
            string response = MakeRequest(hallsURL);
            List<HallsResponse> hr = JsonConvert.DeserializeObject<List<HallsResponse>>(response);
            return hr;
        }

    }
}