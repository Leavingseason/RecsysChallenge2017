using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.Data
{
    class CleanOnlineData
    {
        public static void AdjustItemColumns(string infile, string outfile)
        {
            string[] headers = "id	title	career_level	discipline_id	industry_id	country	is_payed	region	latitude	longitude	employment	tags	created_at".Split('\t');
            Dictionary<string, int> newheader2idx = new Dictionary<string, int>();
            Dictionary<int, string> idx2header = new Dictionary<int, string>();
            for (int i = 0; i < headers.Length; i++)
            {
                idx2header.Add(i, headers[i]);
            }



            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine().Replace("recsyschallenge_vlive_2017_items.", "").Replace("recsyschallenge_vlive_2017_train_items_final.", "");
                string[] words = content.Split('\t');
                for (int i = 0; i < words.Length; i++)
                {
                    newheader2idx.Add(words[i], i);
                }

                string res = "";
                for (int i = 0; i < idx2header.Count; i++)
                {
                    res += "\t" + words[newheader2idx[idx2header[i]]];
                }
                wt.Write(res.Substring(1) + "\n");

                while ((content = rd.ReadLine()) != null)
                {
                    words = content.Split('\t');
                    res = "";
                    for (int i = 0; i < idx2header.Count; i++)
                    {
                        res += "\t" + words[newheader2idx[idx2header[i]]];
                    }
                    wt.Write(res.Substring(1) + "\n");
                }
            }
        }

        public static void AppendLossPairs(string date ,int user_max_cnt = 10)
        {
           ;
            //string target_users_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_users_2017-05-04.txt";//
            string accept_pair_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\accepted_pairs\accepted_pairs_"+date+".txt";
            string user_side_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v_userside.csv";

            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v-1.csv";

            string path = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online";
            string[] tried_files = new string[] {  
            Path.Combine(path,"recsys17-pred-highdim-submit_v1.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v2.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v3.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v4.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v5.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v6.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit_v7.csv"),
            Path.Combine(path,"recsys17-pred-highdim-submit.csv")
            };


            Dictionary<string, int> item2cnt = new Dictionary<string, int>();
            Dictionary<string, int> existing_usrs = new Dictionary<string, int>();

            Dictionary<string, List<string>> item2newusers = new Dictionary<string, List<string>>();

            List<Tuple<string, string, float>> user_item_scores = new List<Tuple<string, string, float>>();
            using (StreamReader rd = new StreamReader(user_side_file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    user_item_scores.Add(new Tuple<string, string, float>(words[0], words[1], float.Parse(words[2])));
                }
            }


            using (StreamReader rd = new StreamReader(accept_pair_file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    string[] tokens = words[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    item2cnt.Add(words[0].Trim(), tokens.Length);

                    foreach (var token in tokens)
                    {
                        if (!existing_usrs.ContainsKey(token))
                        {
                            existing_usrs.Add(token, 1);
                        }
                    }
                }
            }

            HashSet<string> tried_pairs = new HashSet<string>();
            foreach (var file in tried_files)
            {
                using (StreamReader rd = new StreamReader(file))
                {
                    string content = null;
                    while ((content = rd.ReadLine()) != null)
                    {
                        string[] words = content.Split('\t');
                        string[] tokens = words[1].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var token in tokens)
                        {
                            tried_pairs.Add(token + ":" + words[0]);
                        }
                    }
                }
            }

            user_item_scores.Sort((a, b) => b.Item3.CompareTo(a.Item3));

            foreach (var tuple in user_item_scores)
            {
                if (!tried_pairs.Contains(tuple.Item1 + ":" + tuple.Item2) && (!existing_usrs.ContainsKey(tuple.Item1) || existing_usrs[tuple.Item1]<user_max_cnt) && (!item2cnt.ContainsKey(tuple.Item2) || item2cnt[tuple.Item2] < 250))
                {
                    if (!item2cnt.ContainsKey(tuple.Item2))
                    {
                        item2cnt.Add(tuple.Item2, 0);
                    }
                    item2cnt[tuple.Item2]++;
                    if (!item2newusers.ContainsKey(tuple.Item2))
                    {
                        item2newusers.Add(tuple.Item2, new List<string>());
                    }
                    item2newusers[tuple.Item2].Add(tuple.Item1);

                    if(!existing_usrs.ContainsKey(tuple.Item1))
                        existing_usrs.Add(tuple.Item1,0);
                    existing_usrs[tuple.Item1]++;
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in item2newusers)
                {
                    wt.Write("{0}\t{1}\n", pair.Key, string.Join(",", pair.Value.ToArray()));
                }
            }
        }

        public static void PrepareFMFile(string infile, string outfile01, string outfile02)
        {  
            using(StreamReader rd = new StreamReader(infile))
            using(StreamWriter wt01 = new StreamWriter(outfile01))
            using (StreamWriter wt02 = new StreamWriter(outfile02))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write("{0}\r",cnt);
                    }
                    int idx = content.IndexOf("#");
                    wt01.Write(content.Substring(0,idx)+"\n");
                    wt02.Write(content.Substring(idx+1)+"\n");
                }
            }
        }
    }
}
