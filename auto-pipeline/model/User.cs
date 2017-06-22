using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class User
    {
        public string id;
        public HashSet<string> title;
        public Dictionary<string, float> title2cnt;
        public int title_cnt;
        public string clevel;
        public string indus;
        public string disc;
        public string country;
        public string region;
        public string experience_n_entries_class;
        public string experience_years_experience;
        public string experience_years_in_current;
        public string edu_degree;
        public HashSet<string> edu_fieldofstudies;
        public string wtcj;
        public string premium ;

        public List<Tuple<string, int>> interactions;
        public Dictionary<string, int> viewed_item_title_words;
        public double viewed_titem_title_cnt;



        public User(){}
        public User(string line)
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
                    title2cnt.Add(token, 1.0f / title_cnt);
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
            region = words[6];
            experience_n_entries_class = words[7];
            experience_years_experience = words[8];
            experience_years_in_current = words[9];
            edu_degree = words[10];
            edu_fieldofstudies = new HashSet<string>();
            foreach (var token in words[11].Split(','))
            {
                //if (token != "000")
                {
                    edu_fieldofstudies.Add(token);
                }
            }
            wtcj = words[12];
            premium = words[13];

            viewed_titem_title_cnt = 0;
            interactions = null;
            viewed_item_title_words = null;
        }

        public void AddViewItem(Item it, int action){
            if (interactions == null)
            {
                interactions = new List<Tuple<string, int>>();
                viewed_item_title_words = new Dictionary<string, int>();
            }

            foreach (var pair in it.title2cnt)
            {
                int tcnt = (int)Math.Round(pair.Value * it.title_cnt);
                viewed_titem_title_cnt += tcnt;
                if (!viewed_item_title_words.ContainsKey(pair.Key))
                {
                    viewed_item_title_words.Add(pair.Key, tcnt);
                }
                else
                {
                    viewed_item_title_words[pair.Key] += tcnt;
                }
            }

            interactions.Add(new Tuple<string, int>(it.id, action));
        }
    }
}
