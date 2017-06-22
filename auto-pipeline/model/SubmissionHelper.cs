using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class SubmissionHelper
    {
        public static void Ensemble(string infile01, string infile02, string outfile, int gap, int k , int start )
        {
            Dictionary<string, List<string>> iid2rec01 = LoadSubFile(infile01);
            Dictionary<string, List<string>> iid2rec02 = LoadSubFile(infile02);
 
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in iid2rec01)
                {
                    if (!iid2rec02.ContainsKey(pair.Key))
                    {
                        wt.Write("{0}\t",pair.Key);
                        wt.Write("{0}\n", string.Join(",", pair.Value.ToArray())); 
                    }
                    else
                    {
                        List<string> merge_list = MergeTwoList(pair.Value, iid2rec02[pair.Key], gap, k, start);
                        wt.Write("{0}\t", pair.Key);
                        wt.Write("{0}\n", string.Join(",", merge_list.ToArray())); 
                    } 
                }

                foreach (var pair in iid2rec02)
                {
                    if (!iid2rec01.ContainsKey(pair.Key))
                    {
                        wt.Write("{0}\t", pair.Key);
                        wt.Write("{0}\n", string.Join(",", pair.Value.ToArray())); 
                    }
                }
            }
        }

        private static List<string> MergeTwoList(List<string> list1, List<string> list2, int gap, int max_k, int start)
        {
            HashSet<string> visited = new HashSet<string>();
            List<string> res = new List<string>();
            int t01 = 0, t02 = 0;
            int cnt01 = list1.Count, cnt02 = list2.Count;

            while (t01 < cnt01 && t01 < start)
            {
                res.Add(list1[t01]);
                visited.Add(list1[t01]);
                t01++;
            }

            while (t01 < cnt01 && t02 < cnt02)
            {
                for (int i = 0; i < gap && i+t01<cnt01; i++)
                {
                    if (!visited.Contains(list1[t01 + i]))
                    {
                        res.Add(list1[t01 + i]);
                        visited.Add(list1[t01 + i]);
                    }
                }
                if (!visited.Contains(list2[t02])){
                    res.Add(list2[t02]);
                    visited.Add(list2[t02]);
                }
                t01 += gap;
                t02 += 1;
            }

            while ((t01 < cnt01))
            {
                if (!visited.Contains(list1[t01]))
                {
                    res.Add(list1[t01]);
                    visited.Add(list1[t01]);
                }
                t01++;
            }

            while ((t02 < cnt02))
            {
                if (!visited.Contains(list2[t02]))
                {
                    res.Add(list2[t02]);
                    visited.Add(list2[t02]);
                }
                t02++;
            }

            if (res.Count > max_k)
            {
                res = res.GetRange(0, max_k);
            }

            return res;
        }

        private static Dictionary<string, List<string>> LoadSubFile(string infile)
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();

            using (StreamReader rd = new StreamReader(infile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(new char[] { ' ', '\t' });
                    if (words.Length < 2)
                    {
                        continue;
                    }
                    
                    string[] tokens = words[1].Split(',');

                    res.Add(words[0], new List<string>());
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        res[words[0]].Add(tokens[i]);
                    }
                }
            }

            return res;
        }

        public static void GenSubFileFromTLCWithAlignment(string infile, string reffile, string outfile)
        {
            int topk = 100;
            var userdict = FeatureFactory.BuildUserDict();
            var itemdict = FeatureFactory.BuildItemDict();

            Dictionary<string, List<Tuple<string, double>>> item2userscore = new Dictionary<string, List<Tuple<string, double>>>();
            int cnt = 0;
            using (StreamReader rd01 = new StreamReader(infile))
            using (StreamReader rd02 = new StreamReader(reffile))
            {
                string content = rd01.ReadLine();
                while ((content = rd01.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write(cnt + "\r");
                    }
                    string[] words = content.Split('\t');
                    double score = double.Parse(words[3]);

                    string[] tokens = rd02.ReadLine().Split(',');
                    string uid = tokens[2];
                    string iid = tokens[3];



                    if (!item2userscore.ContainsKey(iid))
                    {
                        item2userscore.Add(iid, new List<Tuple<string, double>>());
                    }

                    item2userscore[iid].Add(new Tuple<string, double>(uid, score));
                }
            }

            cnt = 0;
            using (StreamWriter wt = new StreamWriter(outfile))
                foreach (var iid in item2userscore.Keys)
                {
                    if (cnt++ % 1000 == 0)
                    {
                        Console.WriteLine("Item {0}", cnt);
                    }
                    var list = item2userscore[iid];
                    //if (list.Count < 500)
                    //{
                    //    continue;
                    //}
                    list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
                    int k = Math.Min(topk, list.Count);
                    wt.Write("{0}\t", iid);
                    for (int i = 0; i < k - 1; i++)
                    {
                        wt.Write("{0},", list[i].Item1);
                    }
                    if (k > 0)
                        wt.Write("{0}\n", list[k - 1].Item1);
                    else
                        wt.Write("\n");
                }

        }

        public static void GenSubFileFromTLC(string infile, string outfile, int[] name_idx, int value_idx, double thre = 0.1, char spliter = '\t', bool hasHeader = true,
            Dictionary<string, User> userdict = null, Dictionary<string, Item> itemdict = null, Func<string[], double> get_score = null)
        {
            int topk = 100;
            if(userdict==null)
                userdict = FeatureFactory.BuildUserDict();
            if (itemdict==null)
                itemdict = FeatureFactory.BuildItemDict();

            Dictionary<string, List<Tuple<string, double>>> item2userscore = new Dictionary<string, List<Tuple<string, double>>>();
            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = null;
                if(hasHeader)
                    rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write(cnt+"\r");
                    }
                    string[] words = content.Split(spliter);

                    string[] tokens = null;
                    if (name_idx.Length == 1)
                    {
                        tokens = words[name_idx[0]].Split('|');
                    }
                    else //if (name_idx.Length == 2)
                    {
                        tokens = new string[name_idx.Length];
                        for (int t = 0; t < name_idx.Length; t++)
                        {
                            tokens[t] = words[name_idx[t]];
                        }
                    }
                    double score = double.Parse(words[value_idx]);
                    if (get_score != null)
                    {
                        score = get_score(words);
                    }

                    if (!item2userscore.ContainsKey(tokens[1]))
                    {
                        item2userscore.Add(tokens[1], new List<Tuple<string, double>>());
                    }

                    if (score > thre ) //    && userdict[tokens[0]].title.Intersect(itemdict[tokens[1]].title).Count()>0)
                    {
                        item2userscore[tokens[1]].Add(new Tuple<string, double>(tokens[0], score));
                    }
                }
            }

            cnt = 0;
            using(StreamWriter wt = new StreamWriter(outfile))
            foreach (var iid in item2userscore.Keys)
            {
                if (cnt++ % 1000 == 0)
                {
                    Console.WriteLine("Item {0}", cnt);
                }
                var list = item2userscore[iid];
                //if (list.Count < 500)
                //{
                //    continue;
                //}
                list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
                int k = Math.Min(topk, list.Count);
                wt.Write("{0}\t",iid);
                for (int i = 0; i < k - 1; i++)
                {
                    wt.Write("{0},", list[i].Item1);
                }
                if (k > 0)
                    wt.Write("{0}\n", list[k - 1].Item1);
                else
                    wt.Write("\n");
            }

        }
    }
}
