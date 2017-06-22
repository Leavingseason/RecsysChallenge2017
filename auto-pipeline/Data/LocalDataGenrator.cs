using RecSys17.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.Data
{
    class LocalDataGenrator
    {

        public static void Pipeline()
        {


            SmallJobs.SelectNoneZeros(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\interactions_grouped.csv", //@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\interactions_grouped.csv",  @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv"
            2, @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\train_interactions_small.csv", 1, 0, false);

//            Utils.ShuffleFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\train_interactions_bigneg.csv",
//@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\train_interactions_bigneg_shuffled.csv");

            

//            SmallJobs.SelectNoneZeros(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv",
//                2, @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_moreneginst_no0.csv", 1, 0, false);

//            Utils.ShuffleFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_moreneginst_no0.csv",
//@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_moreneginst_no0_shuffle.csv");



//             //SplitFileIntoPartsByGroup(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_sample1_0.1_moreneginst_shuffled.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media");

////            //SmallJobs.UserItemStat(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv");

////            //SmallJobs.StatUserInterest(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv");


            //FeatureFactory.GenFeature(
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_moreneginst_no0_shuffle.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train02_morefeature_no0.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv"
            //); 

            //FeatureFactory.GenFeature4LocalTest(
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test02_morefeature.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv",
            //    100000
            //); 

            //FeatureFactory.GenTestFeature(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv");

           // FeatureFactory.GenFeatureSparse(
           //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_moreneginst_no0_shuffle.csv",
           //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\train02_morefeature_no0.csv",
           //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
           //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
           //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv"
           //);

            //FeatureFactory.GenFeature4LocalTestSparse(
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test02_morefeature.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv",
            //    100000
            //);

            //FeatureFactory.GenTestFeatureSparse(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_impression_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\user_interest_part.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv");

           // CorrectUserItemId();

         //  GenTestCandidatePairs();

        }

        public static void GenWord2trigram()
        {
            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            List<string> english_words = new List<string>();
            using (StreamReader rd = new StreamReader(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\dssm\google-10000-english-usa.txt"))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.Length > 1)
                    {
                        english_words.Add(content);
                    }
                }
            }
            Console.WriteLine("english word cnt:{0}",english_words.Count);

            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\dssm\titletokens_englishwords.tsv";

            Dictionary<string, float> tokens = new Dictionary<string, float>();
            
            foreach (var pair in user_titlefreq)
            {
                if (pair.Value >= 20)
                {
                    if (!tokens.ContainsKey(pair.Key))
                    {
                        tokens.Add(pair.Key, pair.Value);
                    }
                }
            }
            foreach (var pair in item_titlefreq)
            {
                if (pair.Value >= 20)
                {
                    if (!tokens.ContainsKey(pair.Key))
                    {
                        tokens.Add(pair.Key, pair.Value);
                    }
                }
            }

            List<Tuple<string, float>> token2freq = new List<Tuple<string, float>>();
            foreach (var pair in tokens)
            {
                token2freq.Add(new Tuple<string, float>(pair.Key, pair.Value));
            }
            token2freq.Sort((a,b)=>b.Item2.CompareTo(a.Item2));

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                for (int i = 0; i < token2freq.Count; i++)
                {
                    wt.Write("{0}\t{1}\n", token2freq[i].Item1, english_words[i]);
                }
            }
        }

        public static void GenOnlineTestCandidatePairs(string date)
        {
            Console.WriteLine("GenOnlineTestCandidatePairs");

            string target_items = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_"+date+".txt";
            string target_users = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_users_" + date + ".txt";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\candidates\test_candidates_" + date + ".txt";

            HashSet<string> items = new HashSet<string>();
            HashSet<string> users = new HashSet<string>();
             

            using(StreamReader rd = new StreamReader(target_items))
            //using (StreamReader rd02 = new StreamReader(target_users))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.StartsWith("id"))
                    {
                        continue;
                    }
                    string iid = content.Substring(0, content.IndexOf("\t"));
                    if (!items.Contains(iid))
                    {
                        items.Add(iid);
                    }
                }
            }

            using (StreamReader rd = new StreamReader(target_users))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (content.StartsWith("id"))
                    {
                        continue;
                    }
                    string uid = content;
                    if (!users.Contains(uid))
                    {
                        users.Add(uid);
                    }
                }
            }

            Console.WriteLine("user cnt = {0}", users.Count);
            Console.WriteLine("item cnt = {0}", items.Count);

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                int ucnt = 0;
                foreach (var uid in users)
                {
                    if (ucnt++ % 100 == 0)
                    {
                        Console.Write("user {0}\r",ucnt);
                    }
                    foreach (var iid in items)
                    {
                        wt.Write("{0}\t{1}\t0\n", uid, iid);
                    }
                }
            } 


        }

        private static void GenTestCandidatePairs()
        {
            //string test_infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_morefeature_format.csv";
            //string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test_complete_0_candidates.tsv";
            //using (StreamReader rd = new StreamReader(test_infile)) 
            //using (StreamWriter wt = new StreamWriter(test_outfile))
            //{
            //    string content = null;
            //    int cc = 0;
            //    while ((content = rd.ReadLine()) != null)
            //    {
            //        if (cc++ % 100000 == 0)
            //        {
            //            Console.WriteLine(cc);
            //        }
            //        string[] words = content.Split(',');
            //        // var tokens = words[2].Split('|');
            //        var user = words[2];
            //        var item = words[3];

            //        wt.WriteLine(user + "\t" + item + "\t" + "0");

            //    }
            //}


            string test_infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test02_morefeature.csv";
            string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test02_local_candidates.tsv";
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
                    
                    var user = words[1].Split(':')[0].Split('_')[1];
                    var item = words[2].Split(':')[0].Split('_')[1];

                    wt.WriteLine(user + "\t" + item + "\t" + words[0]);

                }
            }
        }

        private static void CorrectUserItemId()
        {
            string test_infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_morefeature_format.csv";
            string test_featurefile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test_complete_0_morefeature_format.csv";
            string test_outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\sparse\test_complete_0_morefeature_format_correct.csv";
            using (StreamReader rd = new StreamReader(test_infile))
            using(StreamReader rd02 = new StreamReader(test_featurefile))
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

                    content=rd02.ReadLine();
                    words = content.Split(',');
                    words[1]=string.Format("uid_{0}:1",user);
                    words[2] = string.Format("iid_{0}:1",item);
                    wt.WriteLine(string.Join(",",words)); 
                     
                }
            }
        }


        public static void GenLocalTestFile()
        {
            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test.tsv";
            string outpath = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test";
            string train_file = Path.Combine(outpath, "test_tmp.tsv");
            string test_file = Path.Combine(outpath, "test01.tsv");

            Random rng = new Random();
            double thresh = 0.8;
            Dictionary<string, int> item2side = new Dictionary<string, int>();

            using (StreamReader rd = new StreamReader(interaction_file))
            using (StreamWriter wt01 = new StreamWriter(train_file))
            using (StreamWriter wt02 = new StreamWriter(test_file))
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
                    if (!item2side.ContainsKey(words[1]))
                    {
                        item2side.Add(words[1], rng.NextDouble() < thresh ? 1 : 0);
                    }
                    if (item2side[words[1]] == 1)
                    {
                        wt01.WriteLine(content);
                    }
                    else
                    {
                        wt02.WriteLine(content);
                    }
                }
            }            

        }

        public static void SplitFileIntoPartsByGroup(string infile, string outpath, char spliter = '\t', int key_idx = 1, int k = 2)
        {
            Dictionary<string, int> item2part = new Dictionary<string, int>();
            StreamWriter[] wts = new StreamWriter[k];
            for (int i = 0; i < wts.Length; i++)
            {
                wts[i] = new StreamWriter(Path.Combine(outpath, "test_part" + i + ".csv"));
            }

            Random rng = new Random();

            using (StreamReader rd = new StreamReader(infile))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write("{0} w\r", cnt / 10000);
                    }
                    string[] words = content.Split(spliter);
                    if (!item2part.ContainsKey(words[key_idx]))
                    {
                        item2part.Add(words[key_idx], rng.Next(k));
                    }
                    wts[item2part[words[key_idx]]].WriteLine(content);
                }
            }

            for (int i = 0; i < wts.Length; i++)
            {
                wts[i].Close();
            }
        }

        public static void SplitLocalTestFile()
        {
            string interaction_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test.tsv";
            string outpath = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test";
            int split_cnt = 4;
            StreamWriter[] wts = new StreamWriter[split_cnt];

                for (int i = 0; i < split_cnt; i++)
                {
                    wts[i] = new StreamWriter(Path.Combine(outpath, "test_0" + i + ".tsv"));
                }

            Random rng = new Random();
        
            Dictionary<string, int> item2side = new Dictionary<string, int>();

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
                    if (!item2side.ContainsKey(words[1]))
                    {
                        item2side.Add(words[1], (int)(rng.NextDouble() / (1.0 / split_cnt)));
                    }
                    wts[item2side[words[1]]].WriteLine(content);
                }
            }

            for (int i = 0; i < split_cnt; i++)
            {
                wts[i].Close();
            }

        }


        public static void GenWeightData(string interactionfile, string infile, string outfile) 
        {
            Dictionary<string, int> pair2label = new Dictionary<string, int>();
            using (StreamReader rd = new StreamReader(interactionfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    string key = words[0] + "," + words[1];
                    int label = int.Parse(words[2]);
                    if (label > 0)
                    {
                        pair2label.Add(key, label);
                    }
                }
            }

            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string key = content.Substring(content.IndexOf("#") + 1);
                    float w = 0.1f;
                    if (pair2label.ContainsKey(key))
                    {
                        if (pair2label[key] >= 4)
                        {
                            w = 1;
                        }
                        else if (pair2label[key] >= 2)
                        {
                            w = 0.6f;
                        }
                        else if (pair2label[key] == 1)
                        {
                            w = 0.3f;
                        }
                    }
                    else
                    {
                        if (content.StartsWith("1"))
                        {
                            Console.WriteLine("error:{0}",content);
                        } 
                    }
                    wt.WriteLine("{0} cost:{1} {2}", content.Substring(0, 1), w, content.Substring(2));
                }
            }
        }
    }
}
