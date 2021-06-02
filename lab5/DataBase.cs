using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    class RawDataItem
    {
        public String Name { get; set; }
        public int Group { get; set; }
        public String Part { get; set; }
        public float Price { get; set; }
        public float Count { get; set; }
        public float Sum
        {
            get { return Count * Price; }
        }
    }
    class SummaryDataItem
    {
        public String GroupName { get; set; }
        public float GroupSum { get; set; }
    }
    class Utils
    {
        private static Dictionary<int, String> dict;
        static Utils()
        {
            if (dict == null)
            {
                dict = new Dictionary<int, string>(5);
                dict.Add(0, "Продукты");
                dict.Add(1, "Бытовая химия");
                dict.Add(2, "Одежда");
                dict.Add(3, "Фрукты");
                dict.Add(4, "Полиграфия");
            }
        }
        public static String GetGroupByNumber(int number)
        {
            if (dict.ContainsKey(number))
                return dict[number];
            else
                return "???";
        }

    }
    interface DataInterface
    {
        List<RawDataItem> GetRawData();
        List<SummaryDataItem> GetSummaryData();
    }
    class DataStorage : DataInterface
    {
        public bool IsReady
        {
            get
            {
                if (rawdata == null)
                    return false;
                else
                    return true;
            }
        }
        private List<RawDataItem> rawdata;
        private List<SummaryDataItem> sumdata;
        private char devider = '%';
        public DataStorage() { }

        private bool InitData(string datapath)
        {
            rawdata = new List<RawDataItem>();

            try
            {
                StreamReader sr = new StreamReader(datapath, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(devider);
                    var item = new RawDataItem()
                    {
                        Name = items[0].Trim(),
                        Part = items[1].Trim(),
                        Group = Convert.ToInt32(items[2].Trim()),
                        Price = Convert.ToSingle(items[3].Trim()),
                        Count = Convert.ToSingle(items[4].Trim())
                    };
                    rawdata.Add(item);
                }
                sr.Close();
                BuildSummary();
            }
            catch (IOException ex)
            {
                return false;
            }
            return true;
        }
        private void BuildSummary()
        {
            Dictionary<int, float> tmp = new Dictionary<int, float>();
            foreach (var item in rawdata)
            {
                if (tmp.ContainsKey(item.Group))
                    tmp[item.Group] += item.Sum;
                else
                    tmp[item.Group] = item.Sum;

            }
            sumdata = new List<SummaryDataItem>();
            foreach (var item in tmp)
            {
                sumdata.Add(new SummaryDataItem()
                {
                    GroupName = Utils.GetGroupByNumber(item.Key),
                    GroupSum = item.Value
                });
            }
        }
        public static DataStorage DataCreator(String path)
        {
            DataStorage d = new DataStorage();
            if (d.InitData(path))
                return d;
            else
                return null;
        }
        public List<RawDataItem> GetRawData()
        {
            if (this.IsReady)
                return rawdata;
            else 
                return null;
        }

        public List<SummaryDataItem> GetSummaryData() {
            if (this.IsReady)
                return sumdata;
            else
                return null;

        }
    }
}