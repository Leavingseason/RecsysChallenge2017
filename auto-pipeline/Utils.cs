using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17
{
    class Utils
    {
        public static void OutputDict<T1, T2>(Dictionary<T1, T2> dict, string outfile)
        {
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in dict)
                {
                    wt.WriteLine("{0},{1}", pair.Key, pair.Value);
                }
            }
        }

        public static Dictionary<string, float> LoadDict(string infile, int keyIdx, int valueIdx)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    if(!string.IsNullOrEmpty(words[valueIdx]))
                        result.Add(words[keyIdx], float.Parse(words[valueIdx]));
                }
            }
            return result;
        }


        public static void OverlapStat(string file01, string file02, int colidx)
        {
            HashSet<string> values01 = LoadValue2Hashset(file01, colidx);
            HashSet<string> values02 = LoadValue2Hashset(file02,colidx);

            int hit = values01.Intersect(values02).Count();

            Console.WriteLine("{0}\t{1}\t{2}", hit, values01.Count, values02.Count);
        }

        private static HashSet<string> LoadValue2Hashset(string file, int colidx)
        {
            HashSet<string> res = new HashSet<string>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split(',');
                    if (!res.Contains(words[colidx]))
                    {
                        res.Add(words[colidx]);
                    }
                }
            }
            return res;
        }

        public static void SelectSubSet(string infile, string outfile, string[] col_names)
        {
            HashSet<string> selectedFeatures = new HashSet<string>(col_names);
             

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                string[] headers = content.Split(',');
                HashSet<int> selectedFeatureIdx = new HashSet<int>();
                int dim = headers.Length;
                wt.Write(headers[0] + "," + headers[1]);
                for (int i = 2; i < dim; i++)
                {
                    if (selectedFeatures.Contains(headers[i]))
                    {
                        selectedFeatureIdx.Add(i);
                        wt.Write("," + headers[i]);
                    } 
                }
                wt.WriteLine();

                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split(',');
                    wt.Write(words[0] + "," + words[1]);
                    for (int i = 2; i < dim; i++)
                    {
                        if (selectedFeatureIdx.Contains(i))
                        {
                            wt.Write("," + words[i]);
                        }
                    }
                    wt.WriteLine();
                }
            }
        }

        public static void ShuffleFile(string infile, string outfile)
        {
            Console.WriteLine("ShuffleFile...");
            List<string> lines = new List<string>();
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 1000000 == 0)
                    {
                        Console.Write(cnt + "\r"); 
                    }
                    lines.Add(content);
                }
            }

            var arr = lines.ToArray();
            Tools.Common.Shuffle<string>(new Random(), arr);

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var line in arr)
                {
                    wt.WriteLine(line);
                }
            }
        }

        public static List<string> RandomPickup(List<string> list, int k)
        {
            if (list.Count <= k)
            {
                return new List<string>(list);
            }

            int cnt = list.Count;
            Random rng = new Random();
            for (int i = 0; i < k; i++)
            {
                int idx = rng.Next(cnt - i);
                string tmp = list[idx];
                list[idx] = list[cnt - 1 - i];
                list[cnt - 1 - i] = tmp;
            }

            return list.GetRange(cnt - k, k);
        }

        public static void SelectSubSet(string infile, string outfile, List<int> selectedFeatureIdx, int topk = 100000)
        { 
            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;   
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    if (cnt > topk)
                    {
                        break;
                    }
                    string[] words = content.Split(',');
                    wt.Write(words[0] + "," + words[1]);
                    foreach(var idx in selectedFeatureIdx)
                    {
                        wt.Write("," + words[idx]);
                    }
                    wt.WriteLine();
                }
            }
        }


        public static void SelectFeatureSubset(string infile, string outfile, string featureRankFile, int k, double r)
        {
            Random rng = new Random((int)DateTime.Now.Ticks);

            /// load features ranks
            List<Tuple<string, double>> feature2importance = LoadFeature2Importance(featureRankFile);

            /// select features
            HashSet<string> selectedFeatures = new HashSet<string>();
            for (int i = 0; i < k; i++)
            {
                selectedFeatures.Add(feature2importance[i].Item1);
            }

            int cnt = 0;
            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = rd.ReadLine();
                string[] headers = content.Split(',');
                HashSet<int> selectedFeatureIdx = new HashSet<int>();
                int dim = headers.Length;
                wt.Write(headers[0] + "," + headers[1]);
                for (int i = 2; i < dim; i++)
                {
                    if (selectedFeatures.Contains(headers[i]))
                    {
                        selectedFeatureIdx.Add(i);
                        wt.Write("," + headers[i]);
                    }
                    else
                    {
                        if (rng.NextDouble() < r)
                        {
                            selectedFeatureIdx.Add(i);
                            wt.Write("," + headers[i]);
                        }
                    }
                }
                wt.WriteLine();

                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 10000 == 0)
                    {
                        Console.WriteLine(cnt);
                    }
                    string[] words = content.Split(',');
                    wt.Write(words[0] + "," + words[1]);
                    for (int i = 2; i < dim; i++)
                    {
                        if (selectedFeatureIdx.Contains(i))
                        {
                            wt.Write("," + words[i]);
                        }
                    }
                    wt.WriteLine();
                }
            }
        }

        public static List<Tuple<string, double>> LoadFeature2Importance(string featureRankFile)
        {
            List<Tuple<string, double>> feature2importance = new List<Tuple<string, double>>();
            double t;
            using (StreamReader rd = new StreamReader(featureRankFile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Replace("\"", "").Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2 || !double.TryParse(words[1], out t))
                    {
                        continue;
                    }
                    feature2importance.Add(new Tuple<string, double>(words[0], double.Parse(words[1])));
                }
            }
            return feature2importance;
        }


        internal static void StatColLabelCorre()
        {
            throw new NotImplementedException();
        }

        internal static void StatColLabelCorre(string infile, string outfile, int label_idx, int col_idx)
        {
            Dictionary<string, int> value2cnt = new Dictionary<string, int>();
            Dictionary<string, int> value2poscnt = new Dictionary<string, int>();
            using (StreamReader rd = new StreamReader(infile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    if (!value2cnt.ContainsKey(words[col_idx]))
                    {
                        value2cnt.Add(words[col_idx],0);
                        value2poscnt.Add(words[col_idx],0);
                    }
                    value2cnt[words[col_idx]]++;
                    if (words[label_idx].Equals("1") || words[label_idx].Equals("True"))
                    {
                        value2poscnt[words[col_idx]]++;
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in value2cnt)
                {
                    wt.WriteLine("{0},{1},{2},{3}", pair.Key, pair.Value, value2poscnt[pair.Key], value2poscnt[pair.Key] * 1.0 / pair.Value);
                }
            }
        }

        internal static void OutputDict02(Dictionary<string, int> word_cnt, Dictionary<string, int> word_hit, string outfile)
        {
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in word_cnt)
                {
                    if (pair.Value > 0)
                    {
                        wt.WriteLine("{0},{1},{2},{3}", pair.Key, pair.Value, word_hit[pair.Key], word_hit[pair.Key] * 1.0 / pair.Value);
                    }
                }
            }
        }
    }
}
