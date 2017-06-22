using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class WordHashing
    {
        public static void BuildWordHashing()
        {
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict(); 

            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\word_hashing.csv";

            Random rng = new Random();
            int topic_cnt = 200;
            Dictionary<string, int> word2idx = new Dictionary<string, int>();
            foreach (var pair in item_titlefreq)
            {
                if (pair.Value >= 20)
                {
                    if (!word2idx.ContainsKey(pair.Key))
                    {
                        word2idx.Add(pair.Key, rng.Next(topic_cnt));
                    }
                }
            }

            int ite_cnt = 50;
            for (int ite = 0; ite < ite_cnt; ite++)
            {
                Console.Write("ite : {0}\r",ite);
                foreach (var item in itemdict.Values)
                {
                    if (item.title.Count > 1)
                    {
                        List<string> words = new List<string>();
                        foreach (var title in item.title)
                        {
                            if (word2idx.ContainsKey(title))
                            {
                                words.Add(title);
                            }
                        }
                        if (words.Count > 1)
                        {
                            List<int> cur_topics = new List<int>();
                            foreach (var word in words)
                            {
                                cur_topics.Add(word2idx[word]);
                            }
                            int new_topic = cur_topics[rng.Next(words.Count)];
                            foreach (var word in words)
                            {
                                if(rng.NextDouble()<0.8)
                                    word2idx[word] = new_topic;
                                else
                                {
                                    word2idx[word] = rng.Next(topic_cnt);
                                }
                            }
                        }
                    }
                }
            }

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var pair in word2idx)
                {
                    wt.WriteLine("{0},{1}",pair.Key,pair.Value);
                }
            }
        }
    }
}
