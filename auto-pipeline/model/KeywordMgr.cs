using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class KeywordMgr
    {
        public Dictionary<string, int> useritem_index;
        public Dictionary<string, int> itemitem_title_index;
        public Dictionary<string, int> itemitem_tag_index;

        public KeywordMgr()
        {
            useritem_index = BuildIndex(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_useritem.csv");
            itemitem_title_index = BuildIndex(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_itemtitle.csv");
            itemitem_tag_index = BuildIndex(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_itemtag.csv"); 
        } 


        private Dictionary<string, int> BuildIndex(string file)
        {
            int thre = 100;
            List<Tuple<string, double>> index = new List<Tuple<string, double>>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    if (int.Parse(words[1]) >= thre)
                    {
                        index.Add(new Tuple<string, double>(words[0], double.Parse(words[3])));
                    }
                }
            }
            index.Sort((a, b) => b.Item2.CompareTo(a.Item2));
             
            Dictionary<string, int> res = new Dictionary<string, int>();
            for (int i = 0; i < index.Count; i++)
            {
                res.Add(index[i].Item1, i);
            }
            return res;
        }
         
    }
}
