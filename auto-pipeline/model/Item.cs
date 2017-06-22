using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class Item
    {
        public string id;
        public HashSet<string> title;
        public int title_cnt;
        public Dictionary<string, float> title2cnt;
        public string clevel;
        public string indus;
        public string disc;
        public string country;
        public string region;
        public string is_paid;
        public string employment;
        public HashSet<string> tags;
        public DateTime create_at;

        public Item(){}
        public Item(string line)
        {
            string[] words = line.Split('\t');

            id = words[0];
            title = new HashSet<string>();
            title2cnt = new Dictionary<string, float>();
            title_cnt = 0;
            var tokens = words[1].Split(',');
            title_cnt = tokens.Length;
            foreach (var token in tokens)
            {
                title.Add(token);
                if (!title2cnt.ContainsKey(token))
                {
                    title2cnt.Add(token, 1.0f/title_cnt);
                }
                else
                {
                    title2cnt[token] += 1.0f / title_cnt;
                }
            }
            clevel = words[2];
            disc = words[3];
            indus = words[4];
            country = words[5];
            is_paid = words[6];
            region = words[7];
            employment = words[10];
            tags = new HashSet<string>();
            foreach (var token in words[11].Split(','))
            {
               // if (token != "000")
                {
                    tags.Add(token);
                }
            }
            if (!string.IsNullOrEmpty(words[12]) && words[12]!="null")
            {
                if (words[12].Contains("-"))
                {
                    create_at = DateTime.Parse(words[12]);
                }
                else
                {
                    create_at = Tools.Common.ParseTime(double.Parse(words[12]));
                }
            }
            else
            {
                create_at = DateTime.Parse("2017-01-01");
            }
        }
    }
}
