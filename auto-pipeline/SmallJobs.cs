using RecSys17.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17
{
    class SmallJobs
    {
        public static void TitleLengthStat()
        {
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();

            using (StreamWriter wt = new StreamWriter(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\title_word_len\item_title_tag_len.csv"))
            {
                foreach (var item in itemdict)
                {
                    wt.WriteLine("{0},{1}",item.Value.title_cnt, item.Value.tags.Count);
                }
            }

            using (StreamWriter wt = new StreamWriter(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\title_word_len\user_title_field_len.csv"))
            {
                foreach (var user in userdict)
                {
                    wt.WriteLine("{0},{1}", user.Value.title_cnt, user.Value.edu_fieldofstudies.Count);
                }
            }
        }
        public static void OverlapStat(string infile01, string infile02, string outfile)
        {
            Dictionary<string,string> ids01 = LoadDict(infile01);
            Dictionary<string, string> ids02 = LoadDict(infile02);
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in ids01)
                {
                    if (ids02.ContainsKey(pair.Key))
                    {
                        wt.WriteLine(pair.Value);
                        wt.WriteLine(ids02[pair.Key]);
                    }
                }
            }
            int overlap = ids01.Keys.Intersect(ids02.Keys).Count();
            Console.WriteLine("{0}\t{1}\t{2}", overlap, ids01.Count, ids02.Count);
        }

        private static Dictionary<string, string> LoadDict(string infile01)
        {
            Dictionary<string, string> ids01 = new Dictionary<string, string>();
            using (StreamReader rd = new StreamReader(infile01))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if(!ids01.ContainsKey(words[0]))
                        ids01.Add(words[0], content);
                }
            }
            return ids01;
        }

        private static HashSet<string> LoadHashSet(string infile01)
        {
            HashSet<string> ids01 = new HashSet<string>();
            using (StreamReader rd = new StreamReader(infile01))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    ids01.Add(words[0]);
                }
            }
            return ids01;
        }

        public static void VobcabStat()
        {
            string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\items_noheader.csv";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_tags_stst.csv";
            int idx = 1;
            Dictionary<string, int> word2usercnt = new Dictionary<string, int>();

            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    HashSet<string> tokens = new HashSet<string>(words[idx].Split(',')); ;
                    foreach (var title in tokens)
                    {
                        if (!word2usercnt.ContainsKey(title))
                        {
                            word2usercnt.Add(title, 1);
                        }
                        else
                        {
                            word2usercnt[title]++;
                        }
                    }
                }
            }

            Utils.OutputDict<string, int>(word2usercnt, outfile);

        }

        public static void SelectCandidates()
        {
            DateTime start = DateTime.Now;
            List<string> target_users = FeatureFactory. LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");
            List<string> target_items = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");

            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();
            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            FeatureFactory.BuildUserHistory(userdict, itemdict, interaction_file);


            string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\test_candidates_0" + ".csv";
            int cnt = 0;
            using (StreamWriter wt = new StreamWriter(test_outfile))
            {
                foreach (var item in target_items)
                {
                    if (cnt++ % 100 == 0)
                    {
                        Console.Write("test item {0}, \t{1} \r", cnt, DateTime.Now.Subtract(start).TotalMinutes);
                    }
                    if (long.Parse(item) % 10 != 0)
                    {
                        continue;
                    }

                    foreach (var user in target_users)
                    {
                        if (userdict.ContainsKey(user) && itemdict.ContainsKey(item))
                        {
                            int overlap01 = userdict[user].title.Intersect(itemdict[item].title).Count();
                            int overlap02 = userdict[user].title.Intersect(itemdict[item].tags).Count();
                            int overlap03 = 0, overlap04 = 0;
                            if (userdict[user].interactions != null)
                            {
                                foreach (var e in userdict[user].interactions)
                                {
                                    if (itemdict.ContainsKey(e.Item1))
                                    {
                                        overlap03 += itemdict[e.Item1].title.Intersect(itemdict[item].title).Count();
                                        overlap04 += itemdict[e.Item1].tags.Intersect(itemdict[item].tags).Count();
                                    }
                                }
                            }
                            wt.WriteLine("{0},{1},{2}", user, item, overlap01 + overlap02 + overlap03 + overlap04);
                        }
                    }

                }
            }
        }

        public static void StatActionWithCriteria(int action)
        {
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\action2criteria\" + action + ".csv";
            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();

            FeatureFactory.BuildUserHistory(userdict, itemdict, interaction_file);

            using(StreamReader rd = new StreamReader(interaction_file))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write(cnt / 10000 + " w\r");
                    }
                    string[] words = content.Split('\t');
                    if (int.Parse(words[2]) == action)
                    {
                        if (userdict.ContainsKey(words[0]) && itemdict.ContainsKey(words[1]))
                        {
                            int overlap01 = userdict[words[0]].title.Intersect(itemdict[words[1]].title).Count();
                            int overlap02 = userdict[words[0]].title.Intersect(itemdict[words[1]].tags).Count();
                            int overlap03 = 0, overlap04 = 0;
                            if (userdict[words[0]].interactions != null)
                            {
                                foreach (var e in userdict[words[0]].interactions)
                                {
                                    if (action == e.Item2 && itemdict.ContainsKey(e.Item1))
                                    {
                                        overlap03 += itemdict[e.Item1].title.Intersect(itemdict[words[1]].title).Count();
                                        overlap04 += itemdict[e.Item1].tags.Intersect(itemdict[words[1]].tags).Count();
                                    }
                                }
                            }
                            wt.WriteLine("{0},{1},{2},{3},{4}", overlap01, overlap02, overlap03, overlap04, overlap01 + overlap02 + overlap03 + overlap04);
                        }
                    }
                }
            }
        }

        public static void UserItemStat(string infile, string outfile) //= @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions.csv"
        {
            Console.WriteLine("UserItemStat...");

            HashSet<string> exclu_items = LoadItemHashSet(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv");
             
            HashSet<string> userset = new HashSet<string>();
            HashSet<string> itemset = new HashSet<string>();

            Dictionary<string, int[]> user2impression = new Dictionary<string, int[]>();

            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();

                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split('\t');

                    if (exclu_items.Contains(words[1]))
                    {
                        continue;
                    }

                    if (!userset.Contains(words[0]))
                    {
                        userset.Add(words[0]);
                        user2impression.Add(words[0], new int[6]);
                        Array.Clear(user2impression[words[0]], 0, 6);
                    }
                    user2impression[words[0]][int.Parse(words[2])]++;
                    if (!itemset.Contains(words[1]))
                    {
                        itemset.Add(words[1]);
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))//@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\usre_impression.csv"
            {
                foreach (var pair in user2impression)
                {
                    wt.Write(pair.Key);
                    int sum = pair.Value.Sum();
                    wt.Write("," + sum );
                    for (int i = 0; i < 6; i++)
                    {
                        wt.Write(",{0},{1}", pair.Value[i], pair.Value[i] *1.0 / (sum + 30));
                    }
                    wt.WriteLine();
                }
            }
        }

        private static HashSet<string> LoadItemHashSet(string p)
        {
            HashSet<string> res = new HashSet<string>();
            using (StreamReader rd = new StreamReader(p))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (!res.Contains(words[1]))
                    {
                        res.Add(words[1]);
                    }
                }
            }
            return res;
        }

        public static void TargetItemExplore()
        {
            HashSet<string> target_users = new HashSet<string>(FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv"));
            HashSet<string> target_items = new HashSet<string>(FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv"));

            Dictionary<string, List<Tuple<string, int>>> item2interactionlist = new Dictionary<string, List<Tuple<string, int>>>();

            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\target_item_in_train.txt";
            using (StreamReader rd = new StreamReader(interaction_file))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split('\t');
                    if (target_items.Contains(words[1]) && target_users.Contains(words[0]))
                    {
                        if (words[2] != "0")
                        {
                            if (!item2interactionlist.ContainsKey(words[1]))
                            {
                                item2interactionlist.Add(words[1], new List<Tuple<string, int>>());
                            }
                            item2interactionlist[words[1]].Add(new Tuple<string, int>(words[0], int.Parse(words[2])));
                        }
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in item2interactionlist)
                {
                    wt.Write(pair.Key);
                    foreach (var tuple in pair.Value)
                    {
                        wt.Write("\t{0}:{1}", tuple.Item1, tuple.Item2);
                    }
                    wt.WriteLine();
                }
            }
        }

        private static void CountOverlap(string infile, HashSet<string> set)
        {
            int cnt = 0;
            int hit = 0;
            using (StreamReader rd = new StreamReader(infile))
            {                
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    cnt++;
                    if (set.Contains(content))
                    {
                        hit++;
                    }
                }
            }
            Console.WriteLine("{0} / {1}", hit, cnt);
        }


        public static void OutputPremiumUsers()
        {
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            List<string> target_users = FeatureFactory.LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");

            using (StreamWriter wt = new StreamWriter(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\premium_user.txt"))
            {
                foreach (var uid in target_users)
                {
                    if( userdict.ContainsKey(uid) && userdict[uid].premium=="1"){
                        wt.WriteLine("{0},1", uid);
                    }
                    else
                    {
                        wt.WriteLine("{0},0",uid);
                    }
                }
            }
        }

        public static void SelectNoneZeros(string infile, int idx, string outfile, double r_click, double r_push, bool hasHeader=true)
        {
            Console.WriteLine("SelectNoneZeros");
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\users_adj_schema.csv");  //@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\users_adj_schema.csv"
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\items_adj_schema.csv"); //@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\items_adj_schema.csv"

            HashSet<string> existing_pairs = new HashSet<string>();
            HashSet<string> existing_iids = new HashSet<string>();

            Dictionary<string, int> user2actioncnt = new Dictionary<string, int>();

            Random rng = new Random();

            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                if (hasHeader)
                {
                    content = rd.ReadLine();
                    wt.WriteLine(content.Replace("recsyschallenge_v2017_interactions_final_anonym_training_export.", ""));
                }

                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt); 
                    }
                    string[] words = content.Split('\t');
                    string user = words[0];
                    string item = words[1];

                    if (!userdict.ContainsKey(user) || !itemdict.ContainsKey(item))
                        continue;

                    if (!existing_pairs.Contains(user + "|" + item))
                    {
                        existing_pairs.Add(user + "|" + item);
                    }
                    if (!existing_iids.Contains(item))
                    {
                        existing_iids.Add(item);
                    }

                    if (words[idx] == "1")
                    {
                        if (rng.NextDouble() < r_click)
                        {
                            wt.WriteLine(content);
                        }
                    }
                    else if (words[idx] == "0")
                    {
                        {
                            if (rng.NextDouble() < r_push)
                            {
                                wt.WriteLine(content);
                            }
                        }
                    }
                    else
                    {
                        wt.WriteLine(content);
                    }

                    if (words[idx] != "0")
                    {
                        if (!user2actioncnt.ContainsKey(user))
                        {
                            user2actioncnt.Add(user, 1);
                        }
                        else
                        {
                            user2actioncnt[user]++;
                        }
                    }
                }


                /// gen more negative instances
                List<string> all_uids = new List<string>(userdict.Keys);
                int user_cnt = all_uids.Count;
                List<string> all_iids = null;
                int new_user_cnt = 0; 

                Console.WriteLine("Writing new candidates...");



                //////Dictionary<string, List<string>> user2interest_items = KNN.BuildUserInterestedItems(infile);

                //////Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
                //////Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);


                //////all_iids = Utils.RandomPickup(existing_iids.ToList(), 5000);
                //////new_user_cnt = 500;
                //////cnt = 0;
                //////foreach (var iid in all_iids)
                //////{
                //////    if (cnt++ % 100 == 0)
                //////    {
                //////        Console.Write("HardNeg " + cnt + "\r");
                //////    }

                //////    var top_users = RetrieveTopUserList(iid, all_uids, userdict, itemdict, user_titlefreq, item_titlefreq, new_user_cnt);
                //////    foreach (var tuple in top_users)
                //////    {
                //////        if (!existing_pairs.Contains(tuple.Item1 + "|" + iid))
                //////        {
                //////            wt.WriteLine("{0}\t{1}\t{2}", tuple.Item1, iid, 0);
                //////        }
                //////    }
                //////}

                ////// item-based sampling  
                all_iids = Utils.RandomPickup(existing_iids.ToList(), 10000);  // 100000
                new_user_cnt = 500;   // 500
                cnt = 0;
                foreach (var iid in all_iids)
                {
                    if (cnt++ % 100 == 0)
                    {
                        Console.Write("RandomNeg \t\t\t" + cnt + "\r");
                    }

                    for (int i = 0; i < new_user_cnt; i++)
                    {
                        string uid = all_uids[rng.Next(user_cnt)];
                        if (!existing_pairs.Contains(uid + "|" + iid))
                        {
                            wt.WriteLine("{0}\t{1}\t{2}", uid, iid, 0);

                            //existing_pairs.Add(uid + "|" + iid);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }


                all_iids = Utils.RandomPickup(existing_iids.ToList(), 500);
                new_user_cnt = 10000;  // 30000
                cnt = 0;
                foreach (var iid in all_iids)
                {
                    if (cnt++ % 100 == 0)
                    {
                        Console.Write(cnt + "\t\t\t\r");
                    }

                    for (int i = 0; i < new_user_cnt; i++)
                    {
                        string uid = all_uids[rng.Next(user_cnt)];
                        if (!existing_pairs.Contains(uid + "|" + iid))
                        {
                            wt.WriteLine("{0}\t{1}\t{2}", uid, iid, 0);

                            //existing_pairs.Add(uid + "|" + iid);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }


                //// user-based sampling  
                //List<string> existing_iids_list = new List<string>(existing_iids);
                //int item_cnt = existing_iids_list.Count;
                //var c_all_uid_list = GetTopUsers(user2actioncnt, 1000);
                //int new_item_cnt = 10000;  // 30000
                //cnt = 0;
                //foreach (var uid in c_all_uid_list)
                //{
                //    if (cnt++ % 100 == 0)
                //    {
                //        Console.Write(cnt + "\t\t\t\r");
                //    }

                //    for (int i = 0; i < new_item_cnt; i++)
                //    {
                //        string iid = existing_iids_list[rng.Next(item_cnt)];
                //        if (!existing_pairs.Contains(uid + "|" + iid))
                //        {
                //            wt.WriteLine("{0}\t{1}\t{2}", uid, iid, 0);

                //            //existing_pairs.Add(uid + "|" + iid);
                //        }
                //        else
                //        {
                //            i--;
                //        }
                //    }
                //}

            }
        }

        private static List<string> GetTopUsers(Dictionary<string, int> user2actioncnt, int k)
        {
            List<Tuple<string, int>> user2cnt = new List<Tuple<string, int>>();
            foreach (var pair in user2actioncnt)
            {
                user2cnt.Add(new Tuple<string, int>(pair.Key, pair.Value));
            }
            user2cnt.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            List<string> res = new List<string>();
            int n = user2cnt.Count;
            for (int i = 30; i < k + 30 && i < n; i++)
            {
                res.Add(user2cnt[i].Item1);
            }
            return res;
        }

        public static double GetLRScore(double f0, double f1, double f2, double f3, double f4, double f5, double f6, double f7,
            double f8, double f9, double f10, double f11, double f12, double f13, double f14)
        {
            double output = 0.9188395 * f0 + 0.009892308 * f1 + 0.858565748 * f2 + 0.7979689 * f3 + 0.5827407 * f4 + 0.889054656 * f5 + 2.18628263 * f6 +
                -1.510869 * f7 + 3.03276515 * f8 + 8.870057 * f9 + 20.3693886 * f10 + 1.10062659 * f11 + 4.115251 * f12 + 3.069148 * f13 + 10.4112539 * f14 + -5.08893061;
            return output;
        }

        private static List<Tuple<string,double>> RetrieveTopUserList(string iid, List<string> all_uids, Dictionary<string, User> userdict, Dictionary<string, Item> itemdict,
             Dictionary<string, float> user_titlefreq, Dictionary<string, float> item_titlefreq, int k)
        {

            var item_info = itemdict[iid];

            List<Tuple<string, double>> user_scores = new List<Tuple<string, double>>();

            foreach (var uid in all_uids)
            {
                if (!userdict.ContainsKey(uid))
                {
                    continue;
                }

                var user_info = userdict[uid];
                

                int match = 0;
                foreach (var title in user_info.title)
                {
                    if (item_info.title.Contains(title)) //title != "000" &&
                    {
                        match++;
                    }
                }
                double f0 = match;

                double f1 = user_info.clevel.Equals(item_info.clevel) && !string.IsNullOrEmpty(user_info.clevel) ? 1 : 0;
                double f2 = user_info.indus.Equals(item_info.indus) && !string.IsNullOrEmpty(user_info.indus) ? 1 : 0;
                double f3 = user_info.disc.Equals(item_info.disc) && !string.IsNullOrEmpty(user_info.disc) ? 1 : 0;
                double f4 = user_info.country.Equals(item_info.country) && !string.IsNullOrEmpty(user_info.country) ? 1 : 0;
                double f5 = user_info.region.Equals(item_info.region) && !string.IsNullOrEmpty(user_info.region) ? 1 : 0;

                match = 0;
                foreach (var title in user_info.title)
                {
                    if (item_info.tags.Contains(title)) //title != "000" && 
                    {
                        match++;
                    }
                }
                double f6 = match;
                int u_clevel = 0;
                int j_clevel = 0;
                int.TryParse(user_info.clevel, out u_clevel);
                int.TryParse(item_info.clevel, out j_clevel);
                double f7 = u_clevel - j_clevel;

                double doc_sim = 0;
                foreach (var word in user_info.title)
                {
                    if (!string.IsNullOrEmpty(word) && user_titlefreq.ContainsKey(word) && user_titlefreq[word] >= 20 && item_titlefreq.ContainsKey(word) && item_titlefreq[word] >= 20)
                    {
                        if (item_info.title2cnt.ContainsKey(word))
                        {
                            doc_sim += Math.Sqrt(user_info.title2cnt[word] * item_info.title2cnt[word]) * Math.Log10(1000000.0 / user_titlefreq[word]);
                        }
                    }
                }
                double f8 = doc_sim;

                HashSet<string> user_viewed_titles = new HashSet<string>();
                HashSet<string> user_viewed_tags = new HashSet<string>();
                double nearest_viewed_item = 0, avg_view_item_score = 0, sum_view_item_socre = 0;
                double nearest_viewed_item_tag = 0, avg_view_item_score_tag = 0, sum_view_item_score_tag = 0;
                if (user_info.interactions != null && user_info.interactions.Count > 1)
                {
                    foreach (var tuple in user_info.interactions)
                    {
                        double score_title = 0, score_tag = 0;
                        if (tuple.Item2 != 0 && tuple.Item2 != 4 && tuple.Item1 != iid)
                        {
                            if (itemdict.ContainsKey(tuple.Item1))
                            {
                                var tid = tuple.Item1;
                                foreach (var word in item_info.title)
                                {
                                    if (!user_viewed_titles.Contains(word))
                                    {
                                        user_viewed_titles.Add(word);
                                    }
                                    if (!string.IsNullOrEmpty(word) && item_titlefreq.ContainsKey(word) && item_titlefreq[word] >= 20)
                                    {
                                        if (itemdict[tid].title2cnt.ContainsKey(word))
                                        {
                                            score_title += Math.Sqrt(itemdict[tid].title2cnt[word] * itemdict[iid].title2cnt[word]) * Math.Log10(1000000.0 / item_titlefreq[word]);
                                        }
                                    }
                                }
                                foreach (var word in item_info.tags)
                                {
                                    if (!user_viewed_tags.Contains(word))
                                    {
                                        user_viewed_tags.Add(word);
                                    }
                                    if (itemdict[tid].tags.Contains(word))
                                    {
                                        score_tag += 1;
                                    }
                                }
                            }
                        }

                        sum_view_item_socre += score_title;
                        sum_view_item_score_tag += score_tag;

                        nearest_viewed_item = Math.Max(nearest_viewed_item, score_title);
                        nearest_viewed_item_tag = Math.Max(nearest_viewed_item_tag, score_tag);

                    }

                    avg_view_item_score = sum_view_item_socre / user_info.interactions.Count;
                    avg_view_item_score_tag = sum_view_item_score_tag / user_info.interactions.Count;

                } 

                double f9= nearest_viewed_item, f10 = nearest_viewed_item_tag, f11 = sum_view_item_socre,  f12 = sum_view_item_score_tag,  f13 =avg_view_item_score,  f14 =avg_view_item_score_tag;

                double score = GetLRScore(f0, f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14);

                user_scores.Add(new Tuple<string, double>(uid, score));

            }

            if (user_scores.Count > 0)
            {
                user_scores.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            }

            if (user_scores.Count > k)
            {
                return user_scores.GetRange(0, k);
            }
            else
            {
                return user_scores;
            }
        }

     

        internal static void StatUserInterest(string infile, string outfile)
        {
            Console.WriteLine("statuserinterest...");
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();

            HashSet<string> exclu_items = LoadItemHashSet(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv");


            //string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            //string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_interest.csv";


            Dictionary<string, Dictionary<string, int>> user2indus = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> user2disc = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> user2clevel = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> usercountry = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> user2employment = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();

            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split('\t');
                    string user = words[0];
                    string item = words[1];
                    string type = words[2];

                    if (exclu_items.Contains(item))
                    {
                        continue;
                    }

                    if (!userdict.ContainsKey(user) || !itemdict.ContainsKey(item))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(type) && type != "0" && type != "4")
                    {
                        if (!user2interest_cnt.ContainsKey(user))
                        {
                            user2interest_cnt.Add(user, 1);
                            user2clevel.Add(user, new Dictionary<string, int>());
                            user2disc.Add(user, new Dictionary<string, int>());
                            user2employment.Add(user, new Dictionary<string, int>());
                            user2indus.Add(user, new Dictionary<string, int>());
                        }
                        else
                        {
                            user2interest_cnt[user]++;
                        }

                        Add2Dict(user2clevel[user], itemdict[item].clevel);
                        Add2Dict(user2disc[user], itemdict[item].disc);
                        Add2Dict(user2employment[user], itemdict[item].employment);
                        Add2Dict(user2indus[user], itemdict[item].indus);
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                int bias = 10;
                wt.WriteLine("user,interest_cnt,clever,disc,employment,indus");
                foreach (var user in user2interest_cnt.Keys)
                {
                    wt.Write(user + "," + user2interest_cnt[user]);
                    wt.Write(",{0}", GenKeyValueString(user2clevel[user], user2interest_cnt[user], bias));
                    wt.Write(",{0}", GenKeyValueString(user2disc[user], user2interest_cnt[user], bias));
                    wt.Write(",{0}", GenKeyValueString(user2employment[user], user2interest_cnt[user], bias));
                    wt.Write(",{0}", GenKeyValueString(user2indus[user], user2interest_cnt[user], bias));
                    wt.WriteLine();
                }
            }
        }

        private static string GenKeyValueString(Dictionary<string, int> d, int sum, int bias)
        {
            string res = "";
            foreach (var pair in d)
            {
                res += string.Format("{0}:{1};", pair.Key, pair.Value * 1.0 / (sum + bias));
            }
            if (res.Length > 0)
            {
                res = res.Substring(0, res.Length - 1);
            }
            return res;
        }

        private static void Add2Dict(Dictionary<string, int> d, string v)
        {
            if (!string.IsNullOrEmpty(v))
            {
                if (!d.ContainsKey(v))
                {
                    d.Add(v, 1);
                }
                else
                {
                    d[v]++;
                }
            }
        }

        internal static void ExtractCandidateFromInteractions(string infile, string outfile, int times = 5)
        {
            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.StartsWith("recsys"))
                    {
                        continue;
                    }
                    string[] words = content.Split('\t');
                    if (words[3] == "1" || words[3] == "2" || words[3] == "3" || words[3] == "4" || words[3] == "5")
                    {
                        for (int i = 0; i < times; i++)
                        {
                            wt.WriteLine("{0}\t{1}\t{2}", words[0], words[1], words[3]);
                        }
                    }
                }
            }
        }
    }
}
