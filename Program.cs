using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.Timers;
using Microsoft.VisualBasic.FileIO;
using ConsoleApp1;

namespace demo2
{
    class Program
    {

        //private static ConnectionStringSettings testdb = ConfigurationManager.ConnectionStrings["My Database"];
        //public static string testdbstring = testdb.ConnectionString;

        static void Main(string[] args)
        {
            int api=0;

            string testdbstring = @"Database=mytestdb; Data Source=NUMERAXIAL;Initial Catalog = mytestdb; Persist Security Info=True; Trusted_Connection=Yes;";
        SqlConnection connection = new SqlConnection(testdbstring);
            void readStock(String ticker, String Start, String End)
            {
                Stock s = new Stock("new");
                api = s.getApicall();

                if (api > 2400)
                {
                    Thread.Sleep(60000);
                    s.setApicall(0);
                }
                s.setStock(ticker, Start, End);


                /*
                SqlCommand command = new SqlCommand(@"create table Stock (Ticker varchar(255),Date varchar(255), Open_market varchar(255), High varchar(255), Low varchar(255), Close_market varchar(255), Volume varchar(255))", connection);
                        //command.CommandText = @";
                       connection.Open();
                   command.ExecuteNonQuery();
                */
                connection.Open();

                for (int i = 0; i < s.getObj().result_count; i++)
                {
                    string abc = $"insert into [dbo].[Stock](Ticker,Date, Open_market, High, Low, Close_market, Volume) Values('{ticker}','{s.stock[i].getDate()}', {s.stock[i].getOpen()},{s.stock[i].getHigh()},{s.stock[i].getLow()},{s.stock[i].getClose()},{s.stock[i].getVolume()})";
                    SqlCommand cmd = new SqlCommand(abc, connection);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();


            }
            // readStock("SNAP", "2017-06-28");

            Stock searchStock(String ticker)
            {
                SqlDataReader myreader;
                Stock stock = new Stock();

                connection.Open();

                string abc = $"Select *From Stock Where Ticker = '{ticker}'";
                SqlCommand cmd = new SqlCommand(abc, connection);


                myreader = cmd.ExecuteReader();
                for (int i = 0; myreader.Read(); i++)
                {
                    Stock n = new Stock();
                    stock.stock[i]= n;

                    stock.stock[i].setTicker(myreader[0].ToString());
                    stock.stock[i].setDate(myreader[1].ToString());
                    stock.stock[i].setOpen(Double.Parse(myreader[2].ToString()));
                    stock.stock[i].setHigh(Double.Parse(myreader[3].ToString()));
                    stock.stock[i].setLow(Double.Parse(myreader[4].ToString()));
                    stock.stock[i].setClose(Double.Parse(myreader[5].ToString()));
                    stock.stock[i].setVolume(Double.Parse(myreader[6].ToString()));

                }
                connection.Close();
                return stock;
            }

            /*
            string path = @"C:\Users\jshi3\Desktop\S&P500#1.csv";
            string[] readText = File.ReadAllLines(path);
            foreach(string s in readText)
            {
                readStock(s, "2017-06-29");
            }
            */
            void SP500(String file, String start, String end)
            {
                using (TextFieldParser parser = new TextFieldParser(file))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            readStock(field, start, end);
                        }
                    }

                }
            }
            DateTime localDate = DateTime.Now;
            String a = localDate.Year.ToString();
            String b = localDate.Month.ToString();
            if (int.Parse(b) < 10)
            {
                b = "0" + b;
            }
            String c = localDate.Day.ToString();
            if (int.Parse(c) < 10)
            {
                c = "0" + c;
            }
            String date = a + "-" + b + "-" + c;
            Console.WriteLine(date);
            //readStock("TWTR", "", "");
            //readStock("SNAP", "", "");
            
           SP500(@"C:\Users\jshi3\Desktop\S&P500#1.csv", "", "");
            Console.WriteLine(api);

            /*
            Console.WriteLine("Enter you Start Date in format xxxx-xx-xx");
            String a = Console.ReadLine();
            Console.WriteLine("Enter you End Date in format xxxx-xx-xx");
            String b = Console.ReadLine();
            Console.WriteLine("Enter the Stock ticker you want search in database");
            String search = Console.ReadLine();
            Console.WriteLine(searchStock(search));
            Console.ReadLine();
            */

        }
    }
}
    //Task.Run(() =>
    //{
    //    SqlDataAdapter da = new SqlDataAdapter();
    //    DataSet ds = new DataSet();
    //    DataTable dt = new DataTable();

    //    using (SqlConnection connection = new SqlConnection(testDBString))
    //    {
    //        da.SelectCommand = new SqlCommand(String.Format(@"SELECT * FROM Stock_List WHERE COUNTRY = '{0}'", country), connection);
    //        da.Fill(ds, "Country_Table");
    //        dt = ds.Tables["Country_Table"];
    //    }

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        if ((bool)row[8])
    //        {
    //            Console.WriteLine(row, testDBString);
    //        }

    //        else
    //        {
    //            Console.WriteLine(row, testDBString);
    //        }
    //        Thread.Sleep(2138012983);
    //    }
    //});







  