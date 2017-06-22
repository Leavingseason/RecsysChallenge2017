using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class KNN
    {
        public static void PredictByUserDocsim()
        {
            int topk = 100;
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\knn_user_item_docsim.csv";
           

            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);


            List<string> target_users = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");
            List<string> target_items = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                int cnt = 0;
                foreach (var iid in target_items)
                {
                    List<Tuple<string, double>> user2score = new List<Tuple<string, double>>();
                    foreach (var uid in target_users)
                    {
                        if (userdict.ContainsKey(uid))
                        {
                            double score = GetUserScore(userdict[uid], itemdict[iid], user_titlefreq, item_titlefreq);
                            if (score > 0)
                            {
                                user2score.Add(new Tuple<string, double>(uid, score));
                            }
                        }
                    }
                    Console.WriteLine("{0}\tnum of candi:\t{1}", cnt++, user2score.Count);
                    if (user2score.Count > 0)
                    {
                        user2score.Sort((a, b) => b.Item2.CompareTo(a.Item2));
                        int k = Math.Min(topk, user2score.Count);
                        wt.Write("{0}\t", iid);
                        for (int i = 0; i < k - 1; i++)
                        {
                            wt.Write("{0},", user2score[i].Item1);
                        }
                        wt.Write("{0}\n", user2score[k - 1].Item1);
                    }
                }
            }
        }

        private static double GetUserScore(User user, Item item, Dictionary<string, float> user_titlefreq, Dictionary<string, float> item_titlefreq)
        {
            double doc_sim = 0;
            foreach (var word in user.title)
            {
                if (!string.IsNullOrEmpty(word) && user_titlefreq.ContainsKey(word) && user_titlefreq[word] >= 20 && item_titlefreq.ContainsKey(word) && item_titlefreq[word] >= 20)
                {
                    if (item.title2cnt.ContainsKey(word))
                    {
                        doc_sim += Math.Sqrt(user.title2cnt[word] * item.title2cnt[word]) * Math.Log10(1000000.0 / user_titlefreq[word]);
                    }
                }
            }
            return doc_sim;
        }

        public static void PredictByViewDocsim()
        {
            int topk = 100;
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\knn_tag.csv";
            string trainfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";

            Dictionary<string, List<string>> user2interest_items = BuildUserInterestedItems(trainfile);

            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();
           // Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);


            List<string> target_users = FeatureFactory. LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");
            List<string> target_items = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                int cnt = 0;
                foreach (var iid in target_items)
                {
                    List<Tuple<string, double>> user2score = new List<Tuple<string, double>>();
                    foreach (var uid in target_users)
                    {
                        if (user2interest_items.ContainsKey(uid))
                        {
                            //double score =  GetUserScore(iid, user2interest_items[uid], itemdict, item_titlefreq);
                            double score = GetUserScore_Tag(iid, user2interest_items[uid], itemdict, item_titlefreq);

                            if (score > 0)
                            {
                                user2score.Add(new Tuple<string, double>(uid, score));
                            }
                        }
                    }
                    Console.WriteLine("{0}\tnum of candi:\t{1}", cnt++, user2score.Count);
                    if (user2score.Count > 0)
                    {
                        user2score.Sort((a, b) => b.Item2.CompareTo(a.Item2));
                        int k = Math.Min(topk, user2score.Count);
                        wt.Write("{0}\t", iid);
                        for (int i = 0; i < k - 1; i++)
                        {
                            wt.Write("{0},", user2score[i].Item1);
                        }
                        wt.Write("{0}\n", user2score[k - 1].Item1);
                    }
                }
            }
        }

        public static double GetUserScore(string iid, List<string> history, Dictionary<string, Item> itemdict, Dictionary<string, float> item_titlefreq)
        {
            double score = 0;
            foreach (var tid in history)
            {
                if (itemdict.ContainsKey(tid) && tid != iid)
                {
                    foreach (var word in itemdict[iid].title)
                    {
                        if (!string.IsNullOrEmpty(word) && item_titlefreq.ContainsKey(word) && item_titlefreq[word] >= 20)
                        {
                            if (itemdict[tid].title2cnt.ContainsKey(word))
                            {
                                score += Math.Sqrt(itemdict[tid].title2cnt[word] * itemdict[iid].title2cnt[word]) * Math.Log10(1000000.0 / item_titlefreq[word]);
                            }
                        }
                    }
                }
            }
            return score;
        }

        public static double GetUserScore_Tag(string iid, List<string> history, Dictionary<string, Item> itemdict, Dictionary<string, float> item_titlefreq)
        {
            double score = 0;
            foreach (var tid in history)
            {
                if (itemdict.ContainsKey(tid))
                {
                    foreach (var word in itemdict[iid].tags)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            if (itemdict[tid].tags.Contains(word))
                            {
                                score += 1;
                            }
                        }
                    }
                }
            }
            if (history.Count > 0)
            {
                score /= history.Count;
            }
            return score;
        }

        public static Dictionary<string, List<string>> BuildUserInterestedItems(string file)
        {
            Console.WriteLine("BuildUserInterestedItems...");
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (words[2] != "0" && words[2] != "4")
                    {
                        if (!res.ContainsKey(words[0]))
                        {
                            res.Add(words[0], new List<string>());
                        }
                        res[words[0]].Add(words[1]);
                    }
                }
            }
            Console.WriteLine("BuildUserInterestedItems finished.");
            return res;
        }

        public static void PredictFromClosestJobs()
        {
            int topk = 5;
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\knn_closest_jobs.csv";

            string logfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\knn_closest_jobs_logs.csv";
            string logfile_bestscore = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\knn_closest_jobs_logs_bestscore.csv";

            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";

            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();
            //Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();

           // Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);


            HashSet<string> target_users = new HashSet<string>(FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv"));
            List<string> target_items = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");

            Dictionary<string, HashSet<string>> item2clicked_users = LoadItem2PosUsers(interaction_file, target_users);

            using (StreamWriter wt = new StreamWriter(outfile))
            using(StreamWriter wt_log = new StreamWriter(logfile))
            using (StreamWriter wt_log02 = new StreamWriter(logfile_bestscore))
            {
                int cnt = 0;
                foreach (var iid in target_items)
                {
                    if (cnt++ % 100 == 0)
                    {
                        Console.Write("writing {0}\r", cnt);
                    }
                    HashSet<string> candidates = new HashSet<string>();

                    List<Tuple<string, double>> item2sim = new List<Tuple<string, double>>();
                    foreach (var ciid in item2clicked_users.Keys)
                    {
                        item2sim.Add(new Tuple<string, double>(ciid, GetItemSim(iid, ciid, item_titlefreq, itemdict)));
                    }

                    item2sim.Sort((a, b) => b.Item2.CompareTo(a.Item2));

                    //foreach (var citem in item2sim)
                    //{
                    //    wt_log.WriteLine("{0},{1},{2}", iid, citem.Item1, citem.Item2);
                    //}
                    wt_log02.WriteLine("{0},{1},{2}", iid, item2sim[0].Item1, item2sim[0].Item2);

                    foreach (var tuple in item2sim)
                    {
                        foreach (var user in item2clicked_users[tuple.Item1])
                        {
                            if (!candidates.Contains(user) && tuple.Item2>0)
                            {
                                candidates.Add(user);
                            }
                        }

                        if (candidates.Count >= topk)
                        {
                            break;
                        }
                    }

                   
                    if (candidates.Count > 0)
                    {
                         var candi_list = candidates.ToList();
                         string out_line = iid + "\t";
                        
                        for (int i = 0; i < candi_list.Count  && i< topk; i++)
                        {
                            out_line+=candi_list[i]+","; 
                        }
                        wt.WriteLine(out_line.Substring(0, out_line.Length - 1));
                    }
                }
            }
        }

        private static double GetItemSim(string iid, string ciid, Dictionary<string, float> item_titlefreq, Dictionary<string, Item> itemdict)
        {
            if (!itemdict.ContainsKey(iid) || !itemdict.ContainsKey(ciid))
            {
                return 0;
            }

            Item info_iid = itemdict[iid];
            Item info_ciid = itemdict[ciid];

            if (info_ciid.indus != info_iid.indus || info_ciid.disc != info_iid.disc || info_ciid.country != info_iid.country)
            {
                return 0;
            }

            double res = 0;
            foreach (var word in info_iid.title2cnt.Keys)
            {
                if (item_titlefreq.ContainsKey(word) && info_ciid.title2cnt.ContainsKey(word))
                {
                    res += Math.Log10(1000000.0 / item_titlefreq[word]) * info_iid.title2cnt[word] * info_ciid.title2cnt[word];
                }
            }

            return res;
        }

         

        private static Dictionary<string, HashSet<string>> LoadItem2PosUsers(string interaction_file, HashSet<string> target_users)
        {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            using (StreamReader rd = new StreamReader(interaction_file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (words[2] != "0" && words[2] != "4" && target_users.Contains(words[0]))
                    {
                        if (!res.ContainsKey(words[1]))
                        {
                            res.Add(words[1], new HashSet<string>());
                        }
                        if (!res[words[1]].Contains(words[0]))
                        {
                            res[words[1]].Add(words[0]);
                        }
                    }
                }
            }
            return res;
        }

    }
}
