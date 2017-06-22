using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class DocumentClustering
    {

        public static void TestGenClusterIdFeature()
        {
            string candi_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train02_candidates_localgen.csv";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\train_feature_as_clusterid.csv";

            string TLC_cluster_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\TLC\-1.inst.txt";
            string TLC_training_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\training.txt";
            string cluster_out_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\cluster_id_mapping.tsv";

            Dictionary<string, int> id2cluster = new Dictionary<string, int>();
            using(StreamReader rd01= new StreamReader(TLC_cluster_file))
            using (StreamReader rd02 = new StreamReader(TLC_training_file))
            {
                string content = rd01.ReadLine();
                while ((content = rd02.ReadLine()) != null)
                {
                    string id = content.Substring(content.IndexOf("#") + 1);
                    string[] words = rd01.ReadLine().Split('\t');
                    id2cluster.Add(id, int.Parse(words[2]));
                }
            }

            //using(StreamReader rd= new StreamReader(candi_file))
            //using (StreamWriter wt = new StreamWriter(outfile))
            //{
            //    string content = null;
            //    while ((content = rd.ReadLine()) != null)
            //    {
            //        string[] words = content.Split('\t');
            //        string uid = "uid_" + words[0];
            //        string iid = "iid_" + words[1];
            //        if (id2cluster.ContainsKey(uid) && id2cluster.ContainsKey(iid))
            //        {
            //            wt.WriteLine("{0},{1},{2},{3},{4},{5}", words[2]=="0" || words[2]=="4"?"0":"1", words[0], words[1], id2cluster[uid], id2cluster[iid], id2cluster[uid] == id2cluster[iid] ? 1 : 0);
            //        }
            //    }
            //}

            using (StreamWriter wt = new StreamWriter(cluster_out_file))
            {
                foreach (var pair in id2cluster)
                {
                    wt.WriteLine("{0}\t{1}",pair.Key,pair.Value);
                }
            }

        }

        /// <summary>
        ///  preapre svmlight feature for TLC kmeans clustering
        /// </summary>
        public static void PrepareFeatureFile()
        {
            bool reset_keymap = false;
            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            string keymapfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\word_id_mapping.csv";

            if (reset_keymap)
            {
                BuildKeyMapping(keymapfile,user_titlefreq,item_titlefreq);
            }

            Dictionary<string, int> keymapper = LoadKeymapfile(keymapfile);


            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\users_adj_schema.csv");
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv");

            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\training.txt";
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in userdict)
                {
                    List<Tuple<string, int>> words = new List<Tuple<string, int>>();
                    if (pair.Value.title.Count > 0)
                    {
                        foreach (var word in pair.Value.title)
                        {
                            if (keymapper.ContainsKey(word))
                            {
                                words.Add(new Tuple<string, int>(word, keymapper[word]));
                            }
                        }
                        if (words.Count > 4)
                        {
                            words.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                            string res = "";
                            foreach (var tuple in words)
                            {
                                res += " " + tuple.Item2+":1";
                            }
                            wt.WriteLine("0" + res + "#uid_" + pair.Key);
                        }
                    }
                }

                foreach (var pair in itemdict)
                {
                    List<Tuple<string, int>> words = new List<Tuple<string, int>>();
                    if (pair.Value.title.Count > 0)
                    {
                        foreach (var word in pair.Value.title)
                        {
                            if (keymapper.ContainsKey(word))
                            {
                                words.Add(new Tuple<string, int>(word, keymapper[word]));
                            }
                        }
                        if (words.Count > 4)
                        {
                            words.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                            string res = "";
                            foreach (var tuple in words)
                            {
                                res += " " + tuple.Item2 + ":1";
                            }
                            wt.WriteLine("0" + res + "#iid_" + pair.Key);
                        }
                    }
                }

            }

        }

        public static Dictionary<string, int> LoadKeymapfile(string keymapfile)
        {
            Dictionary<string, int> keymapper = new Dictionary<string, int>();
            using (StreamReader rd = new StreamReader(keymapfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    keymapper.Add(words[0], int.Parse(words[1]));
                }
            }
            return keymapper;
        }

        private static void BuildKeyMapping(string keymapfile, Dictionary<string, float> user_titlefreq, Dictionary<string, float> item_titlefreq)
        {
            Dictionary<string, int> word2idx = new Dictionary<string, int>();
            foreach (var pair in user_titlefreq)
            {
                if (pair.Value >= 20)
                {
                    if (!word2idx.ContainsKey(pair.Key))
                    {
                        word2idx.Add(pair.Key, word2idx.Count + 1);
                    }
                }
            }

            foreach (var pair in item_titlefreq)
            {
                if (pair.Value >= 20)
                {
                    if (!word2idx.ContainsKey(pair.Key))
                    {
                        word2idx.Add(pair.Key, word2idx.Count + 1);
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(keymapfile))
            {
                foreach (var pair in word2idx)
                {
                    wt.WriteLine("{0},{1}",pair.Key,pair.Value);
                }
            }
        }
    }
}
