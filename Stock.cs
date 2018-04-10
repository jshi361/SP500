using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class Stock
    {
        private string date;
        private string ticker;
        private double open_market;
        private double high;
        private double low;
        private double close_market;
        private double volume;
        private double ex_dividend;
        private double split_ratio;
        private double adj_open;
        private double adj_high;
        private double adj_low;
        private double adj_close;
        private double adj_volume;
        private static int api_call_credits;
        private RootObject obj;
        public List<Stock> stock;
        
        public int getApicall()
        {
            return api_call_credits;
        }
        public void setApicall(int i)
        {
            api_call_credits = i;
        }

        public void setDate(string date) { this.date = date; }
        public string getTicker() { return ticker; }
        public void setTicker(string ticker) { this.ticker = ticker; }
        public string getDate() { return date; }
        public double getOpen() { return open_market; }
        public void setOpen(double Open) { this.open_market = Open; }
        public double getHigh() { return high; }
        public void setHigh(double High) { this.high = High; }
        public double getLow() { return low; }
        public void setLow(double Low) { this.low = Low; }
        public double getClose() { return close_market; }
        public void setClose(double Close) { this.close_market = Close; }
        public double getVolume() { return volume; }
        public void setVolume(double Volume) { this.volume = Volume; }
        public RootObject getObj()
        {
            return obj;
        }
        public Stock()
        {

        }
        public Stock(String i)
        {
            stock = new List<Stock>();
        }
        public void setStock(String tick, String start, String end)
        {
            String pre = @"https://api.intrinio.com/prices?identifier=";
            this.ticker = tick;
            String Start = "&start_date=" + start;
            String End = "&end_date=" + end;
            // String url2 = @"https://api.intrinio.com/prices?identifier=SNAP&start_date=2017-06-29";
            String url = pre + ticker + Start + End;
            String pageNumber = "&page_number=";
            obj = new RootObject();
            int h = 1;
            for (int j = 1; j <= h; j++)
            {
                pageNumber = pageNumber + j.ToString();

                HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url + pageNumber);

                request.Method = "GET";
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String
                    (Encoding.Default.GetBytes("c248b72786804ad428d98b29bef7c1c3:a0db0996cdc891c8d7732a9fbf6803c1"));
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();

                /* Pipes the stream to a higher level stream reader with the required encoding format.


                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                Console.WriteLine(readStream.ReadToEnd());
                */

                DataContractJsonSerializer serializer =
                 new DataContractJsonSerializer(typeof(RootObject));
                obj = (RootObject)serializer.ReadObject(receiveStream);
                //List<Datum> data = user.data;
                h= obj.total_pages;
                for (int i = 0; i < obj.data.Count; i++)
                {
                    Stock n = new Stock();

                    n.date = obj.data[i].date;
                    n.open_market = obj.data[i].open;
                    n.high = obj.data[i].high;
                    n.low = obj.data[i].low;
                    n.close_market = obj.data[i].close;
                    n.volume = obj.data[i].volume;
                    n.ex_dividend = obj.data[i].ex_dividend;
                    n.split_ratio = obj.data[i].split_ratio;
                    n.adj_open = obj.data[i].adj_open;
                    n.adj_high = obj.data[i].adj_high;
                    n.adj_low = obj.data[i].adj_low;
                    n.adj_close = obj.data[i].adj_close;
                    n.adj_volume = obj.data[i].adj_volume;

                    stock.Add(n);
                }
                response.Close();

                pageNumber = "&page_number=";
            }
            api_call_credits += h;
            
        }

        override public String ToString()
        {
            string s = "";
            for (int i = 0; i < stock.Count; i++)
            {
                s += "Ticker " + stock[i].ticker + " Date " + stock[i].date + " Open Market " + stock[i].open_market + " High " + stock[i].high + " Low " + stock[i].low + " Close_market " + stock[i].close_market + " Volume " + stock[i].volume + "\n";
            }
            return s;
        }



    }
}
