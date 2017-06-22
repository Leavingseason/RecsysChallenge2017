using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class Evaluation
    {

        public static void StatRecall(string predfile, string outfile)
        {
            Dictionary<string, List<Tuple<string, double, int>>> item2predictions = new Dictionary<string, List<Tuple<string, double,int>>>();
            HashSet<string> posset = new HashSet<string>();
            int cnt = 0;
            using (StreamReader rd = new StreamReader(predfile))
            {
                string content = null;
                 
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write(cnt + "\r");
                    }
                    string[] words = content.Split('\t');

                    string[] tokens =words[0].Split('|');
                     
                    double score = double.Parse(words[3]);

                    if (!item2predictions.ContainsKey(tokens[1]))
                    {
                        item2predictions.Add(tokens[1], new List<Tuple<string, double,int>>());
                    }
                    item2predictions[tokens[1]].Add(new Tuple<string, double,int>(tokens[0], score, int.Parse(words[1])));

                    if (words[1] == "1")
                    {
                        posset.Add(words[0]);
                    }
                }
            }

            Console.WriteLine("Poscnt : {0}", posset.Count);

            foreach (var iid in item2predictions.Keys)
            {
                item2predictions[iid].Sort((a, b) => b.Item2.CompareTo(a.Item2));
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                int hit = 0;
                for (int k = 1; k < 20000; k++)
                {
                    foreach (var pair in item2predictions)
                    {
                        if (pair.Value.Count >= k)
                        {
                            if (pair.Value[k - 1].Item3 == 1)
                            {
                                hit++;
                            }
                        }
                    }
                    wt.WriteLine("{0},{1}", k, hit * 1.0 / posset.Count);
                }
            }

        }

        public static double RandomScore(string infile, string gtfile, Dictionary<string, User> userdict, Dictionary<string, Item> itemdict)
        {
            int topk = 100; 

            Dictionary<string, List<string>> item2userset = new Dictionary<string, List<string>>();
            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split('\t')[0].Split('|');
                    if (!item2userset.ContainsKey(words[1]))
                    {
                        item2userset.Add(words[1], new List<string>());
                    }
                    item2userset[words[1]].Add(words[0]);
                }
            }


            Random rng = new Random(); 

            Dictionary<string, Dictionary<string, int>> item2user2status = LoadGTFile(gtfile, false);

            double res = 0;
            int line_cnt = 0;
            foreach (var pair in item2user2status)
            {
                line_cnt++;
                if (line_cnt % 100 == 0)
                {
                    Console.WriteLine(line_cnt);
                }
                int success_user_cnt = 0;
                for (int i = 0; i < topk; i++)
                {
                    string uid = item2userset[pair.Key][rng.Next(item2userset[pair.Key].Count)];
                    int cur_user_sucess = UserSucess(pair.Key, uid, item2user2status);
                    if (cur_user_sucess > 0)
                    {
                        success_user_cnt++;
                    }
                    res += cur_user_sucess * (IsPremiumUser(uid, userdict));
                }
                res += ItemSucess(success_user_cnt, pair.Key, itemdict);
            }
             
             
            Console.WriteLine("{0}\t{1}\t{2}", line_cnt, res, res / line_cnt);
            return res;
        }

        public static double Score(string subfile, string gtfile, Dictionary<string, User> userdict = null, Dictionary<string, Item> itemdict = null, bool isFeatureMode = false)
        { 

            //if (userdict == null)
            //    userdict = FeatureFactory.BuildUserDict();
            //if (itemdict == null)
            //    itemdict = FeatureFactory.BuildItemDict();

            Dictionary<string, Dictionary<string, int>> item2user2status = LoadGTFile(gtfile, isFeatureMode);

            double res = 0;
            int line_cnt = 0;
            using (StreamReader rd = new StreamReader(subfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(new char[] { ' ', '\t' });
                    if (words.Length < 2)
                    {
                        continue;
                    }
                    line_cnt++;
                    string[] tokens = words[1].Split(',');
                    int success_user_cnt = 0;
                    for (int j = 0; j < tokens.Length; j++)
                    {
                        var token = tokens[j];
                        int cur_user_sucess = UserSucess(words[0], token, item2user2status);
                        if (cur_user_sucess > 0)
                        {
                            success_user_cnt++;
                        }
                        res += cur_user_sucess > 0 ? 1 : 0;  //cur_user_sucess * (IsPremiumUser(token, userdict));

                    }

                   // res += ItemSucess(success_user_cnt, words[0], itemdict);
                }
            }
            Console.WriteLine("{0}\t{1}\t{2}", line_cnt, res, res / line_cnt);
            return res;
        }

        public static int[] Score02(string subfile, string gtfile, Dictionary<string, User> userdict = null, Dictionary<string, Item> itemdict = null)
        {
            int[] hit_cnt = new int[100  + 1];
            Array.Clear(hit_cnt, 0, hit_cnt.Length);

            if (userdict == null)
                userdict = FeatureFactory.BuildUserDict();
            if (itemdict == null)
                itemdict = FeatureFactory.BuildItemDict();

            Dictionary<string, Dictionary<string, int>> item2user2status = LoadGTFile(gtfile, false);

            int res = 0;
            int line_cnt = 0;
            using (StreamReader rd = new StreamReader(subfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(new char[] { ' ', '\t' });
                    if (words.Length < 2)
                    {
                        continue;
                    }
                    line_cnt++;
                    string[] tokens = words[1].Split(',');

                    for (int j = 0; j < tokens.Length && j<100; j++)
                    {
                        var token = tokens[j];
                        int cur_user_sucess = UserSucess(words[0], token, item2user2status) > 0 ? 1 : 0;

                        hit_cnt[j] += cur_user_sucess;
                        res += cur_user_sucess;
                    } 
                }
            }
            Console.WriteLine("{0}\t{1}\t{2}", line_cnt, res, res*1.0 / line_cnt);

            hit_cnt[hit_cnt.Length - 1] = res;
            return hit_cnt;
        }


        private static double ItemSucess(int success_user_cnt, string iid, Dictionary<string, Item> itemdict)
        {
            if (success_user_cnt <= 0)
            {
                return 0;
            }

            if (itemdict.ContainsKey(iid) && itemdict[iid].is_paid == "1")
            {
                return 50;
            }

            return 25;
        }

        private static int IsPremiumUser(string token, Dictionary<string, User> userdict)
        {
            if (userdict.ContainsKey(token) && userdict[token].premium == "1")
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private static int UserSucess(string iid, string uid, Dictionary<string, Dictionary<string, int>> item2user2status)
        {
            int score = 0;
            if (item2user2status.ContainsKey(iid))
            {
                if (item2user2status[iid].ContainsKey(uid))
                {
                    if (item2user2status[iid][uid] == 1)
                    {
                        score = 1;
                    }
                    else if (item2user2status[iid][uid] == 2 || item2user2status[iid][uid] == 3)
                    {
                        score = 5;
                    }
                    else if (item2user2status[iid][uid] == 5)
                    {
                        score = 20;
                    }
                    else if (item2user2status[iid][uid] == 4)
                    {
                        score = -10;
                    }
                }
            }
            return score;
        }

        private static Dictionary<string, Dictionary<string, int>> LoadGTFile(string gtfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test.tsv", bool isFeatureMode =false)
        {
            Dictionary<string, Dictionary<string, int>> res = new Dictionary<string, Dictionary<string, int>>();
            char spliter = isFeatureMode ? ',' : '\t';
            using (StreamReader rd = new StreamReader(gtfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(spliter);
                    string uid = null, iid = null;
                    int status = 0;
                    if (isFeatureMode)
                    {
                        status = int.Parse(words[0]);
                        string[] tokens = words[1].Split('|');
                        uid = tokens[0];
                        iid = tokens[1];
                    }
                    else
                    {
                        status = int.Parse(words[2]);
                        uid = words[0];
                        iid = words[1];
                    }
                    if (status > 0)
                    {
                        if (!res.ContainsKey(iid))
                        {
                            res.Add(iid, new Dictionary<string, int>());
                        }
                        res[iid].Add(uid, status);
                    }
                }
            }
            return res;
        }
    }
}
