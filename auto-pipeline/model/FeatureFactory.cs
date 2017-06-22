using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class FeatureFactory
    {
       
        public static void AppendHeader(string infile, string outfile)
        {
            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                wt.WriteLine(GenHeader());
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 1000000 == 0)
                    {
                        Console.WriteLine(cnt / 10000 + "W");
                    }
                    wt.WriteLine(content);
                }
            }
        }

        public static void GenFeature4LocalTest(string infile, string outfile,
            string user_impression_file, string user_interest_file, string interaction_file, int candi_cnt = 20000)
        {
            Console.WriteLine("GenFeature4LocalTest");
            bool is_reg = false;

            Random rng = new Random();

            HashSet<string> existing_pairs = new HashSet<string>();
            HashSet<string> existing_iids = new HashSet<string>();
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    string key = words[0] + "|" + words[1];
                    if (!existing_pairs.Contains(key))
                    {
                        existing_pairs.Add(key);
                    }
                    if (!existing_iids.Contains(words[1]))
                    {
                        existing_iids.Add(words[1]);
                    }
                }
            }

            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();
            BuildUserHistory(userdict, itemdict, interaction_file);

            KeywordMgr keywordMgr = new KeywordMgr();

            List<string> all_uids = new List<string>(userdict.Keys);
            int user_cnt = all_uids.Count;

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();

            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression = LoadUserImpression(user_impression_file);

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine("training instance {0}", cnt);
                    }

                    string[] words = content.Split('\t');
                    if (!userdict.ContainsKey(words[0]) || !itemdict.ContainsKey(words[1]))
                    {
                        continue;
                    }

                    string label = GetLabel(is_reg, words[2]);


                    wt.Write("{0},{1}", label, words[0] + "|" + words[1]);


                    GenerateFeatures(wt, words[0], words[1], userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);

                    wt.WriteLine();

                }

                Console.WriteLine("Writing new candidates...");
                cnt = 0;
                foreach (var iid in existing_iids)
                {
                    Console.WriteLine(cnt++);
                    for (int i = 0; i < candi_cnt; i++)
                    {
                        string uid = all_uids[rng.Next(user_cnt)];
                        if (!existing_pairs.Contains(uid + "|" + iid))
                        {
                            if (!userdict.ContainsKey(uid) || !itemdict.ContainsKey(iid))
                            {
                                continue;
                            }

                            wt.Write("0,0,{0}", uid + "|" + iid);

                            GenerateFeatures(wt, uid, iid, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);

                            wt.WriteLine();
                        }
                    }
                }
            }

        }


        public static void GenFeature4LocalTestSparse(string infile, string outfile,
            string user_impression_file, string user_interest_file, string interaction_file, int candi_cnt = 20000)
        {
            Console.WriteLine("GenFeature4LocalTest");
            bool is_reg = false;

            Random rng = new Random();

            HashSet<string> existing_pairs = new HashSet<string>();
            HashSet<string> existing_iids = new HashSet<string>();
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    string key = words[0] + "|" + words[1];
                    if (!existing_pairs.Contains(key))
                    {
                        existing_pairs.Add(key);
                    }
                    if (!existing_iids.Contains(words[1]))
                    {
                        existing_iids.Add(words[1]);
                    }
                }
            }

            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();
            BuildUserHistory(userdict, itemdict, interaction_file);

            KeywordMgr keywordMgr = new KeywordMgr();

            List<string> all_uids = new List<string>(userdict.Keys);
            int user_cnt = all_uids.Count;

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();

            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression = LoadUserImpression(user_impression_file);

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine("training instance {0}", cnt);
                    }

                    string[] words = content.Split('\t');
                    if (!userdict.ContainsKey(words[0]) || !itemdict.ContainsKey(words[1]))
                    {
                        continue;
                    }

                    string label = GetLabel(is_reg, words[2]);


                    wt.Write("{0},uid_{1}:1,iid_{2}:1", label, words[0], words[1]);


                    GenerateFeaturesSparse(wt, words[0], words[1], userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);

                    wt.WriteLine();

                }

                Console.WriteLine("Writing new candidates...");
                cnt = 0;
                foreach (var iid in existing_iids)
                {
                    Console.WriteLine(cnt++);
                    for (int i = 0; i < candi_cnt; i++)
                    {
                        string uid = all_uids[rng.Next(user_cnt)];
                        if (!existing_pairs.Contains(uid + "|" + iid))
                        {
                            if (!userdict.ContainsKey(uid) || !itemdict.ContainsKey(iid))
                            {
                                continue;
                            }

                            wt.Write("{0},uid_{1}:1,iid_{2}:1", 0, uid, iid);

                            GenerateFeaturesSparse(wt, uid, iid, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);

                            wt.WriteLine();
                        }
                    }
                }
            }

        }

        public static void GenFeatureSparse(string infile, string outfile,
            string user_impression_file, string user_interest_file, string interaction_file)
        {
            Console.WriteLine("GenFeature");


            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();

            BuildUserHistory(userdict, itemdict, interaction_file);

            KeywordMgr keywordMgr = new KeywordMgr();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();

            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression = LoadUserImpression(user_impression_file);

            //HashSet<string> existing_pairs = new HashSet<string>();
            //HashSet<string> existing_iids = new HashSet<string>();

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine("training instance {0}", cnt);
                    }

                    string[] words = content.Split('\t');
                    if (!userdict.ContainsKey(words[0]) || !itemdict.ContainsKey(words[1]))
                    {
                        continue;
                    }

                    //if (!existing_pairs.Contains(words[0] + "|" + words[1]))
                    //{
                    //    existing_pairs.Add(words[0] + "|" + words[1]);
                    //}
                    //if (!existing_iids.Contains(words[1]))
                    //{
                    //    existing_iids.Add(words[1]);
                    //}

                    if (words[2] == "4")
                    {
                        continue;
                    }
                    string label = GetLabel(false, words[2]);

                    wt.Write("{0},uid_{1}:1,iid_{2}:1", label, words[0], words[1]);


                    GenerateFeaturesSparse(wt, words[0], words[1], userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);
                     
                    wt.WriteLine();

                }



                ///// gen more negative instances
                //List<string> all_uids = new List<string>(userdict.Keys);
                //int user_cnt = all_uids.Count;
                //List<string> all_iids = RandomPickup(existing_iids.ToList(), 100000);
                //int new_user_cnt = 500;
                //Random rng = new Random();

                //Console.WriteLine("Writing new candidates...");
                //cnt = 0;
                //foreach (var iid in all_iids)
                //{
                //    if (cnt++ % 100 == 0)
                //    {
                //        Console.WriteLine(cnt);
                //    }

                //    for (int i = 0; i < new_user_cnt; i++)
                //    {
                //        string uid = all_uids[rng.Next(user_cnt)];
                //        if (!existing_pairs.Contains(uid + "|" + iid))
                //        {
                //            wt.Write("0,0,{0}", uid + "|" + iid);

                //            GenerateFeatures(wt, uid, iid, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt);

                //            wt.WriteLine();

                //            existing_pairs.Add(uid + "|" + iid);
                //        }
                //        else
                //        {
                //            i--;
                //        }
                //    }
                //}
            }

        }
      
        public static void GenFeature(string infile, string outfile, 
            string user_impression_file, string user_interest_file, string interaction_file)
        {
            Console.WriteLine("GenFeature");
             
           
            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();
             
            BuildUserHistory(userdict, itemdict, interaction_file);

            KeywordMgr keywordMgr = new KeywordMgr();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();

            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression =  LoadUserImpression(user_impression_file);

            //HashSet<string> existing_pairs = new HashSet<string>();
            //HashSet<string> existing_iids = new HashSet<string>();

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine("training instance {0}", cnt);
                    }

                    string[] words = content.Split('\t');
                    if (!userdict.ContainsKey(words[0]) || !itemdict.ContainsKey(words[1]))
                    {
                        continue;
                    }

                    //if (!existing_pairs.Contains(words[0] + "|" + words[1]))
                    //{
                    //    existing_pairs.Add(words[0] + "|" + words[1]);
                    //}
                    //if (!existing_iids.Contains(words[1]))
                    //{
                    //    existing_iids.Add(words[1]);
                    //}

                    //if (words[2] == "4")
                    //{
                    //    continue;
                    //}
                    string label = GetLabel(false, words[2]);

                    wt.Write("{0},{1}", label, words[0] + "," + words[1]);


                    GenerateFeatures(wt, words[0], words[1], userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);

                    wt.WriteLine();

                }



                ///// gen more negative instances
                //List<string> all_uids = new List<string>(userdict.Keys);
                //int user_cnt = all_uids.Count;
                //List<string> all_iids = RandomPickup(existing_iids.ToList(), 100000);
                //int new_user_cnt = 500;
                //Random rng = new Random();

                //Console.WriteLine("Writing new candidates...");
                //cnt = 0;
                //foreach (var iid in all_iids)
                //{
                //    if (cnt++ % 100 == 0)
                //    {
                //        Console.WriteLine(cnt);
                //    }

                //    for (int i = 0; i < new_user_cnt; i++)
                //    {
                //        string uid = all_uids[rng.Next(user_cnt)];
                //        if (!existing_pairs.Contains(uid + "|" + iid))
                //        {
                //            wt.Write("0,0,{0}", uid + "|" + iid);

                //            GenerateFeatures(wt, uid, iid, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt);

                //            wt.WriteLine();

                //            existing_pairs.Add(uid + "|" + iid);
                //        }
                //        else
                //        {
                //            i--;
                //        }
                //    }
                //}
            }
              
        }

        public static void BuildUserHistory(Dictionary<string, User> userdict, Dictionary<string, Item> itemdict, string interaction_file)
        {
            using (StreamReader rd = new StreamReader(interaction_file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');

                    if (userdict.ContainsKey(words[0]) && itemdict.ContainsKey(words[1]))
                    {
                        if (words[2] != "0")
                        {
                            userdict[words[0]].AddViewItem(itemdict[words[1]], int.Parse(words[2]));
                        }
                    }
                }
            }
        }

        public static void GenTestFeatureSparse(string user_impression_file, string user_interest_file, string interaction_file)
        {
            Console.WriteLine("GenTestFeature"); 

            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();
            BuildUserHistory(userdict, itemdict, interaction_file);
            KeywordMgr keywordMgr = new KeywordMgr();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();


            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression = LoadUserImpression(user_impression_file);


            List<string> target_users = LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");
            List<string> target_items = LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");


            string test_infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_morefeature_format.csv";
            string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test_complete_0_morefeature_format.csv";
            using (StreamReader rd = new StreamReader(test_infile))
            using (StreamWriter wt = new StreamWriter(test_outfile))
            {
                string content = null;
                int cc = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cc++ % 100000 == 0)
                    {
                        Console.WriteLine(cc);
                    }
                    string[] words = content.Split(',');
                    // var tokens = words[2].Split('|');
                    var user = words[2];
                    var item = words[3];
                    if (userdict.ContainsKey(user) && itemdict.ContainsKey(item)
                        //&& (userdict[user].title.Intersect(itemdict[item].title).Count() > 0  || userdict[user].country == itemdict[item].country) //|| userdict[user].disc.Equals(itemdict[item].disc) || userdict[user].title.Intersect(itemdict[item].tags).Count() > 0
                            )
                    {
                        wt.Write("{0},uid_{1}:1,iid_{2}:1", 0, user, item);
                        GenerateFeaturesSparse(wt, user, item, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);
                        wt.WriteLine();
                    }
                }
            }

            
        }

        public static void GenTestFeature(string user_impression_file, string user_interest_file, string interaction_file)
        {
            Console.WriteLine("GenTestFeature");
            bool is_reg = false;

            Dictionary<string, User> userdict = BuildUserDict();
            Dictionary<string, Item> itemdict = BuildItemDict();
            BuildUserHistory(userdict, itemdict, interaction_file);
            KeywordMgr keywordMgr = new KeywordMgr();

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            Dictionary<string, Dictionary<string, float>> user2indus = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2disc = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2clevel = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, Dictionary<string, float>> user2employment = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, int> user2interest_cnt = new Dictionary<string, int>();


            //LoadUserInterest(user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, user_interest_file);

            Dictionary<string, float[]> user2impression =  LoadUserImpression(user_impression_file);


            List<string> target_users = LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetUsers.csv");
            List<string> target_items = LoadListFromFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\targetItems.csv");


            string test_infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_morefeature_format.csv";
            string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test_complete_0_morefeature_format.csv";
            using (StreamReader rd = new StreamReader(test_infile))
            using (StreamWriter wt = new StreamWriter(test_outfile))
            {
                string content = null;
                int cc = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cc++ % 100000 == 0)
                    {
                        Console.WriteLine(cc);
                    }
                    string[] words = content.Split(',');
                   // var tokens = words[2].Split('|');
                    var user = words[2];
                    var item = words[3];
                    if (userdict.ContainsKey(user) && itemdict.ContainsKey(item)
                        //&& (userdict[user].title.Intersect(itemdict[item].title).Count() > 0  || userdict[user].country == itemdict[item].country) //|| userdict[user].disc.Equals(itemdict[item].disc) || userdict[user].title.Intersect(itemdict[item].tags).Count() > 0
                            )
                    {
                        wt.Write("0,0,{0},{1}", user, item);
                        GenerateFeatures(wt, user, item, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);
                        wt.WriteLine();
                    }
                }
            }

            //int cnt = 0;
            //Random rng = new Random();
            //DateTime start = DateTime.Now;
            //int mod_val = 0;
            //string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_morefeature" + ".csv";
            //using (StreamWriter wt = new StreamWriter(test_outfile))
            //{
            //    foreach (var item in target_items)
            //    {
            //        if (cnt++ % 100 == 0)
            //        {
            //            Console.WriteLine("test item {0}, \t{1}", cnt, DateTime.Now.Subtract(start).TotalMinutes);
            //        }

            //        //if (rng.NextDouble() > 0.1)
            //        //{
            //        //    continue;
            //        //}

            //        foreach (var user in target_users)
            //        {
            //            if (userdict.ContainsKey(user) && itemdict.ContainsKey(item)
            //                //&& (userdict[user].title.Intersect(itemdict[item].title).Count() > 0  || userdict[user].country == itemdict[item].country) //|| userdict[user].disc.Equals(itemdict[item].disc) || userdict[user].title.Intersect(itemdict[item].tags).Count() > 0
            //                )
            //            {
            //                wt.Write("0,0,{0}|{1}", user, item);
            //                GenerateFeatures(wt, user, item, userdict, itemdict, user2impression, user_titlefreq, item_titlefreq, user2clevel, user2disc, user2employment, user2indus, user2interest_cnt, keywordMgr);
            //                wt.WriteLine();
            //            }
            //        }

            //    }
            //}
        }

        private static string GetLabel(bool is_reg, string words)
        { 
            string label_reg ="";
            string label_cls = "";
            if (words == "0")
                {
                    label_reg = "0";
                }
                else if (words == "4")
                {
                    label_reg = "0";
                }
                else if (words == "1")
                {
                    label_reg = "10";
                }
                else if (words == "2")
                {
                    label_reg = "15";
                }
                else if (words == "3")
                {
                    label_reg = "20";
                }
                else
                {
                    label_reg = "30";
                }

            if (words == "0" || words == "4")
            {
                label_cls = "0";
            }
            else
            {
                label_cls = "1";
            }
            return label_cls; // +"," + label_reg;
        }
 
        

        public static void LoadUserInterest(Dictionary<string, Dictionary<string, float>> user2clevel, Dictionary<string, Dictionary<string, float>> user2disc,
            Dictionary<string, Dictionary<string, float>> user2employment, Dictionary<string, Dictionary<string, float>> user2indus,
            Dictionary<string, int> user2interest_cnt, string infile)
        {
            //string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_interest.csv";
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    user2clevel.Add(words[0], new Dictionary<string, float>());
                    user2disc.Add(words[0], new Dictionary<string, float>());
                    user2employment.Add(words[0], new Dictionary<string, float>());
                    user2indus.Add(words[0], new Dictionary<string, float>());

                    user2interest_cnt.Add(words[0], int.Parse(words[1]));

                    BuildDict(user2clevel[words[0]], words[2]);
                    BuildDict(user2disc[words[0]], words[3]);
                    BuildDict(user2employment[words[0]], words[4]);
                    BuildDict(user2indus[words[0]], words[5]);
                }
            }
        }

        private static void BuildDict(Dictionary<string, float> d, string l)
        {
            if (!string.IsNullOrEmpty(l))
            {
                string[] words = l.Split(';');
                foreach (var word in words)
                {
                    var tokens = word.Split(':');
                    d.Add(tokens[0], float.Parse(tokens[1]));
                }
            }
        }

        public static List<string> LoadListFromFile(string p)
        {
            List<string> res = new List<string>();
            using (StreamReader rd = new StreamReader(p))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    res.Add(content);
                }
            }
            return res;
        }

        public static string GenHeader()
        {
            string res = "label,reglabel,id";
           // res += ",user_impre00,user_impre01,user_impre02,user_impre03,user_impre04,user_impre05,user_impre06";
            res += ",match_title,match_clevel,match_indus,match_disc,match_country,match_region,match_tag";
           
            res += ",level_gap";
            res += ",doc_sim";
           // res += ",user_interest00,user_interest01,user_interest02,user_interest03";
            //res += ",time_days";
            res += ",nearest_viewed_item, nearest_viewed_item_tag, sum_view_item_socre, sum_view_item_score_tag, avg_view_item_score, avg_view_item_score_tag";


            res += ",item_is_paid,item_employment,item_title_cnt,item_tag_cnt,user_title_cnt";
            res += ",user_experience_n_entries_class,user_experience_years_experience,user_experience_years_in_current,user_edu_degree,user_wtcj,user_premium,user_disc,user_indus,item_disc,item_indus";
            res += ",user_country,user_region,user_country,user_region,user_clevel,item_clevel";
            res += ",user_impressionCnt";
            res += ",keyword00,keyword01,keyword02,keyword03,keyword04,keyword05";
            res += ",keyword10,keyword11,keyword12,keyword13,keyword14,keyword15";
            res += ",keyword20,keyword21,keyword22,keyword23,keyword24,keyword25";
            return res;
        }

        private static void GenerateFeatures(StreamWriter wt, string uid, string iid, Dictionary<string, User> userdict, Dictionary<string, Item> itemdict, Dictionary<string, float[]> user2impression
            , Dictionary<string, float> user_titlefreq, Dictionary<string, float> item_titlefreq
            , Dictionary<string, Dictionary<string, float>> user2clevel, Dictionary<string, Dictionary<string, float>> user2disc, Dictionary<string, Dictionary<string, float>> user2employment, Dictionary<string, Dictionary<string, float>> user2indus, Dictionary<string, int> user2interest_cnt
            , KeywordMgr keywordMgr)
        {
            DateTime now = DateTime.Parse("2017-03-21");

            //if (user2impression.ContainsKey(uid) && user2impression[uid][0] > 30)
            //{
            //    for (int i = 0; i < 7; i++)
            //    {
            //        wt.Write(",{0}", i == 0 ? (int)user2impression[uid][i] / 100 : user2impression[uid][i]);
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < 7; i++)
            //    {
            //        wt.Write(",{0}", "-1");
            //    }
            //}
             

            var user_info = userdict[uid];
            var item_info = itemdict[iid];

            int match = 0;
            foreach (var title in user_info.title)
            {
                if (item_info.title.Contains(title)) //title != "000" &&
                {
                    match++;
                }
            }
            wt.Write(",{0}", match);

            wt.Write(",{0}", user_info.clevel.Equals(item_info.clevel) && !string.IsNullOrEmpty(user_info.clevel) ? 1 : 0); //&& user_info.clevel!="0"
            wt.Write(",{0}", user_info.indus.Equals(item_info.indus) && !string.IsNullOrEmpty(user_info.indus) ? 1 : 0); //&& user_info.indus != "23"
            wt.Write(",{0}", user_info.disc.Equals(item_info.disc) && !string.IsNullOrEmpty(user_info.disc) ? 1 : 0);//&& user_info.disc != "23"
            wt.Write(",{0}", user_info.country.Equals(item_info.country) && !string.IsNullOrEmpty(user_info.country) ? 1 : 0);
            wt.Write(",{0}", user_info.region.Equals(item_info.region) && !string.IsNullOrEmpty(user_info.region) ? 1 : 0);

            match = 0;
            foreach (var title in user_info.title)
            {
                if (item_info.tags.Contains(title)) //title != "000" && 
                {
                    match++;
                }
            }
            wt.Write(",{0}", match);

        
            int u_clevel = 0;
            int j_clevel = 0;
            int.TryParse(user_info.clevel, out u_clevel);
            int.TryParse(item_info.clevel, out j_clevel);
            wt.Write(",{0}", u_clevel - j_clevel);

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
            wt.Write(",{0}", doc_sim);


            //if (!user2interest_cnt.ContainsKey(uid) || user2interest_cnt[uid] < 10)  // 15
            //{
            //    wt.Write(",{0},{1},{2},{3}", 0, 0, 0, 0);
            //}
            //else
            //{
            //    if (user2clevel[uid].ContainsKey(item_info.clevel))
            //    {
            //        wt.Write(",{0}", user2clevel[uid][item_info.clevel]);
            //    }
            //    else
            //    {
            //        wt.Write(",0");
            //    }
            //    if (user2disc[uid].ContainsKey(item_info.disc))
            //    {
            //        wt.Write(",{0}", user2disc[uid][item_info.disc]);
            //    }
            //    else
            //    {
            //        wt.Write(",0");
            //    }
            //    if (user2employment[uid].ContainsKey(item_info.employment))
            //    {
            //        wt.Write(",{0}", user2employment[uid][item_info.employment]);
            //    }
            //    else
            //    {
            //        wt.Write(",0");
            //    }
            //    if (user2indus[uid].ContainsKey(item_info.indus))
            //    {
            //        wt.Write(",{0}", user2indus[uid][item_info.indus]);
            //    }
            //    else
            //    {
            //        wt.Write(",0");
            //    }
            //}

            int[] top_key_stat_thre = new int[] { 10, 50, 100, 200, 500, 1000 };
            int[] top_key_stat01 = new int[top_key_stat_thre.Length];
            int[] top_key_stat02 = new int[top_key_stat_thre.Length];
            int[] top_key_stat03 = new int[top_key_stat_thre.Length];
            Array.Clear(top_key_stat01, 0, top_key_stat_thre.Length);
            Array.Clear(top_key_stat02, 0, top_key_stat_thre.Length);
            Array.Clear(top_key_stat03, 0, top_key_stat_thre.Length);

            CountOverlap(user_info.title, item_info.title, top_key_stat_thre, top_key_stat01, keywordMgr.useritem_index);

            HashSet<string> user_viewed_titles = new HashSet<string>();
            HashSet<string> user_viewed_tags = new HashSet<string>();

            //wt.Write(",{0}", item_info.create_at.Subtract(now).TotalDays * -1);
            double nearest_viewed_item = 0, avg_view_item_score = 0, sum_view_item_socre = 0 ;
            double nearest_viewed_item_tag = 0, avg_view_item_score_tag = 0, sum_view_item_score_tag = 0 ;
            if (user_info.interactions != null  && user_info.interactions.Count>1)
            {
                foreach (var tuple in user_info.interactions)
                {
                    double score_title = 0, score_tag = 0;
                    if (tuple.Item2 != 0 && tuple.Item2 != 4 && tuple.Item1!= iid)
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


            CountOverlap(user_viewed_titles, item_info.title, top_key_stat_thre, top_key_stat02, keywordMgr.itemitem_title_index);
            CountOverlap(user_viewed_tags, item_info.tags, top_key_stat_thre, top_key_stat03, keywordMgr.itemitem_tag_index);


            wt.Write(",{0},{1},{2},{3},{4},{5}", nearest_viewed_item, nearest_viewed_item_tag, sum_view_item_socre, sum_view_item_score_tag, avg_view_item_score, avg_view_item_score_tag);


            wt.Write(",c{0},c{1},{2},{3},{4}", item_info.is_paid, item_info.employment, item_info.title_cnt, item_info.tags.Count(), user_info.title_cnt);
            wt.Write(",{0},{1},{2},{3},c{4},c{5},c{6},c{7},c{8},c{9}", user_info.experience_n_entries_class, user_info.experience_years_experience, user_info.experience_years_in_current
                , user_info.edu_degree, user_info.wtcj, user_info.premium, user_info.disc, user_info.indus, item_info.disc, item_info.indus);
            wt.Write(",c{0},c{1},c{2},c{3},c{4},c{5}", user_info.country, user_info.region, item_info.country, item_info.region, user_info.clevel, item_info.clevel);

            if (user2impression.ContainsKey(uid) && user2impression[uid][0] >= 20)
            {
                wt.Write(",{0}", user2impression[uid][0]);
            }
            else
            {
                wt.Write(",0");
            }

            for (int i = 0; i < top_key_stat01.Length; i++)
            {
                wt.Write(",{0}", top_key_stat01[i]);
            }
            for (int i = 0; i < top_key_stat02.Length; i++)
            {
                wt.Write(",{0}", top_key_stat02[i]);
            }
            for (int i = 0; i < top_key_stat03.Length; i++)
            {
                wt.Write(",{0}", top_key_stat03[i]);
            }

        }

        private static void GenerateFeaturesSparse(StreamWriter wt, string uid, string iid, Dictionary<string, User> userdict, Dictionary<string, Item> itemdict, Dictionary<string, float[]> user2impression
           , Dictionary<string, float> user_titlefreq, Dictionary<string, float> item_titlefreq
           , Dictionary<string, Dictionary<string, float>> user2clevel, Dictionary<string, Dictionary<string, float>> user2disc, Dictionary<string, Dictionary<string, float>> user2employment, Dictionary<string, Dictionary<string, float>> user2indus, Dictionary<string, int> user2interest_cnt
           , KeywordMgr keywordMgr)
        {
            DateTime now = DateTime.Parse("2017-03-21");
             

            var user_info = userdict[uid];
            var item_info = itemdict[iid];

            int match = 0;
            foreach (var title in user_info.title)
            {
                if (item_info.title.Contains(title)) //title != "000" &&
                {
                    match++;
                }
            }
            wt.Write(",ttcnt:{0}", match);

            wt.Write(",cle:{0}", user_info.clevel.Equals(item_info.clevel) && !string.IsNullOrEmpty(user_info.clevel) ? 1 : 0); //&& user_info.clevel!="0"
            wt.Write(",ine:{0}", user_info.indus.Equals(item_info.indus) && !string.IsNullOrEmpty(user_info.indus) ? 1 : 0); //&& user_info.indus != "23"
            wt.Write(",die:{0}", user_info.disc.Equals(item_info.disc) && !string.IsNullOrEmpty(user_info.disc) ? 1 : 0);//&& user_info.disc != "23"
            wt.Write(",coe:{0}", user_info.country.Equals(item_info.country) && !string.IsNullOrEmpty(user_info.country) ? 1 : 0);
            wt.Write(",rege:{0}", user_info.region.Equals(item_info.region) && !string.IsNullOrEmpty(user_info.region) ? 1 : 0);

            match = 0;
            foreach (var title in user_info.title)
            {
                if (item_info.tags.Contains(title)) //title != "000" && 
                {
                    match++;
                }
            }
            wt.Write(",tagm:{0}", match);


            int u_clevel = 0;
            int j_clevel = 0;
            int.TryParse(user_info.clevel, out u_clevel);
            int.TryParse(item_info.clevel, out j_clevel);
            wt.Write(",cga:{0}", u_clevel - j_clevel);

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
            wt.Write(",docsim:{0}", doc_sim);


           
            int[] top_key_stat_thre = new int[] { 10, 50, 100, 200, 500, 1000 };
            int[] top_key_stat01 = new int[top_key_stat_thre.Length];
            int[] top_key_stat02 = new int[top_key_stat_thre.Length];
            int[] top_key_stat03 = new int[top_key_stat_thre.Length];
            Array.Clear(top_key_stat01, 0, top_key_stat_thre.Length);
            Array.Clear(top_key_stat02, 0, top_key_stat_thre.Length);
            Array.Clear(top_key_stat03, 0, top_key_stat_thre.Length);

            CountOverlap(user_info.title, item_info.title, top_key_stat_thre, top_key_stat01, keywordMgr.useritem_index);

            HashSet<string> user_viewed_titles = new HashSet<string>();
            HashSet<string> user_viewed_tags = new HashSet<string>();

            //wt.Write(",{0}", item_info.create_at.Subtract(now).TotalDays * -1);
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


            CountOverlap(user_viewed_titles, item_info.title, top_key_stat_thre, top_key_stat02, keywordMgr.itemitem_title_index);
            CountOverlap(user_viewed_tags, item_info.tags, top_key_stat_thre, top_key_stat03, keywordMgr.itemitem_tag_index);


            wt.Write(",nvi:{0},nvit:{1},svis:{2},svist:{3},avis:{4},avist:{5}", nearest_viewed_item, nearest_viewed_item_tag, sum_view_item_socre, sum_view_item_score_tag, avg_view_item_score, avg_view_item_score_tag);


            wt.Write(",ipa_{0}:1,empl_{1}:1,ititc:{2},itagc:{3},utitc:{4}", item_info.is_paid, item_info.employment, item_info.title_cnt, item_info.tags.Count(), user_info.title_cnt);
            wt.Write(",enec:{0},eye:{1},eyic:{2},edu:{3},wtcj_{4}:1,premi_{5}:1,udisc_{6}:1,uind_{7}:1,idisc_{8}:1,iind_{9}:1", user_info.experience_n_entries_class, user_info.experience_years_experience, user_info.experience_years_in_current
                , user_info.edu_degree, user_info.wtcj, user_info.premium, user_info.disc, user_info.indus, item_info.disc, item_info.indus);
            wt.Write(",ucout_{0}:1,ureg_{1}:1,icout_{2}:1,ireg_{3}:1,ucle_{4}:1,icle_{5}:1", user_info.country, user_info.region, item_info.country, item_info.region, user_info.clevel, item_info.clevel);


            if (user2impression.ContainsKey(uid) && user2impression[uid][0] >= 20)
            {
                wt.Write(",uimpre:{0}", user2impression[uid][0]);
            }
            else
            {
                wt.Write(",uimpre:0");
            }

            for (int i = 0; i < top_key_stat01.Length; i++)
            {
                wt.Write(",tks01_{0}:{1}",i, top_key_stat01[i]);
            }
            for (int i = 0; i < top_key_stat02.Length; i++)
            {
                wt.Write(",tks02_{0}:{1}",i, top_key_stat02[i]);
            }
            for (int i = 0; i < top_key_stat03.Length; i++)
            {
                wt.Write(",tks03_{0}:{1}", i, top_key_stat03[i]);
            } 

            foreach (var word in user_info.title2cnt.Keys)
            {
                if (user_titlefreq.ContainsKey(word) && user_titlefreq[word] >= 20)
                {
                    wt.Write(",utwd_{0}:{1}", word, user_info.title2cnt[word]);
                }
            }

            foreach (var word in item_info.title2cnt.Keys)
            {
                if (item_titlefreq.ContainsKey(word) && item_titlefreq[word] >= 20)
                {
                    wt.Write(",itwd_{0}:{1}", word, item_info.title2cnt[word]);
                }
            }

            foreach (var field in user_info.edu_fieldofstudies)
            {
                wt.Write(",ufie_{0}:1", field); ;
            }


            ///interaction features
            wt.Write(",uicl_{0}_{1}:1", user_info.clevel, item_info.clevel);
            wt.Write(",uidisp_{0}_{1}:1",user_info.disc,item_info.disc);
            wt.Write(",uiinds_{0}_{1}:1",user_info.indus,item_info.indus);
            wt.Write(",uenec_icl_{0}_{1}:1",user_info.experience_n_entries_class,item_info.clevel);
            wt.Write(",uey_icl_{0}_{1}:1",user_info.experience_years_experience,item_info.clevel);
            wt.Write(",uedy_icl_{0}_{1}:1",user_info.edu_degree,item_info.clevel);
            wt.Write(",ucl_iem_{0}_{1}:1", user_info.clevel, item_info.employment);

        }
         

        private static void CountOverlap(HashSet<string> hashSet01, HashSet<string> hashSet02, int[] thre, int[] stat, Dictionary<string, int> index)
        {
            foreach (var word in hashSet01)
            {
                if (hashSet02.Contains(word))
                {
                    for (int i = 0; i < thre.Length; i++)
                    {
                        if (index.ContainsKey(word) && index[word] < thre[i])
                        {
                            stat[i]++;
                        }
                    }
                }
            }
        }

        private static Dictionary<string, float[]> LoadUserImpression(string file)//= @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\usre_impression.csv"
        {
            Dictionary<string, float[]> res = new Dictionary<string, float[]>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    float[] values = new float[7];
                    values[0] = float.Parse(words[1]);
                    for (int i = 0; i < 6; i++)
                    {
                        values[i + 1] = float.Parse(words[3 + i * 2]); 
                    }
                    res.Add(words[0], values);
                }
            }
            return res;
        }

        public static Dictionary<string, User> BuildUserDict(string file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\users.csv")
        {
            Console.WriteLine("BuildUserDict");
            Dictionary<string, User> res = new Dictionary<string, User>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.StartsWith("id"))
                    {
                        continue;
                    }
                    User user = new User(content);
                    if (!res.ContainsKey(user.id)) {
                        res.Add(user.id, user);
                    } 

                }
            }
            return res;
        }

        public static Dictionary<string, Item> BuildItemDict(string file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\items.csv")
        {
            Console.WriteLine("BuildItemDict");
            Dictionary<string, Item> res = new Dictionary<string, Item>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.StartsWith("id"))
                    {
                        continue;
                    }
                    Item item = new Item(content);
                    if(!res.ContainsKey(item.id))
                        res.Add(item.id, item); 
                }
            }
            return res;
        }
    }
}
