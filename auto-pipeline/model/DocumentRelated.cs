using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class DocumentRelated
    {
        public static void GenKeyWords()
        {
            string outfile_useritem = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_useritem.csv";
            string outfile_itemtitle = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_itemtitle.csv";
            string outfile_itemtag = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\keywords\keywords_itemtag.csv";

            string interation_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions_sample1_0.3_moreneginst_shuffled.csv";

            Dictionary<string, User> userdict =FeatureFactory. BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();
            Dictionary<string, List<string>> user2interest_items = KNN.BuildUserInterestedItems(interation_file);

            Dictionary<string, int> useritem_word_cnt = new Dictionary<string, int>();
            Dictionary<string, int> useritem_word_hit = new Dictionary<string, int>();

            Dictionary<string, int> itemtitle_word_cnt = new Dictionary<string, int>();
            Dictionary<string, int> itemtitle_word_hit = new Dictionary<string, int>();

            Dictionary<string, int> itemtag_word_cnt = new Dictionary<string, int>();
            Dictionary<string, int> itemtag_word_hit = new Dictionary<string, int>();

            using (StreamReader rd = new StreamReader(interation_file))
            {
                string content = null;
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                {
                    if (cnt++ % 100000 == 0)
                    {
                        Console.Write((cnt / 10000) + "w\r");
                    }
                    string[] words = content.Split('\t');
                    if (itemdict.ContainsKey(words[1]) && userdict.ContainsKey(words[0]))
                    {
                        HashSet<string> overlap01 = new HashSet<string>(userdict[words[0]].title.Intersect(itemdict[words[1]].title));
                        HashSet<string> overlap02 = new HashSet<string>();
                        HashSet<string> overlap03 = new HashSet<string>();

                        if (user2interest_items.ContainsKey(words[0]))
                        { 
                            foreach (var tid in user2interest_items[words[0]])
                            {
                                if (tid!=words[1] && itemdict.ContainsKey(tid))
                                {
                                    foreach (var ttitle in itemdict[tid].title)
                                    {
                                        if (itemdict[words[1]].title.Contains(ttitle))
                                        {
                                            if (!overlap02.Contains(ttitle))
                                            {
                                                overlap02.Add(ttitle);
                                            }
                                        }
                                    }
                                    foreach (var ttag in itemdict[tid].tags)
                                    {
                                        if (itemdict[words[1]].tags.Contains(ttag))
                                        {
                                            if (!overlap03.Contains(ttag))
                                            {
                                                overlap03.Add(ttag);
                                            }
                                        }
                                    }
                                }
                            } 
                        }

                        UpdateWordStatus(overlap01, useritem_word_cnt, useritem_word_hit, words[2]);
                        UpdateWordStatus(overlap02, itemtitle_word_cnt, itemtitle_word_hit, words[2]);
                        UpdateWordStatus(overlap03, itemtag_word_cnt, itemtag_word_hit, words[2]);
                    }
                }
            }

            Utils.OutputDict02(useritem_word_cnt, useritem_word_hit, outfile_useritem);
            Utils.OutputDict02(itemtitle_word_cnt, itemtitle_word_hit, outfile_itemtitle);
            Utils.OutputDict02(itemtag_word_cnt, itemtag_word_hit, outfile_itemtag);

        }

        private static void UpdateWordStatus(HashSet<string> overlap, Dictionary<string, int> word_cnt, Dictionary<string, int> word_hit, string status)
        {
            foreach (var word in overlap)
            {
                if (!word_cnt.ContainsKey(word))
                {
                    word_cnt.Add(word, 0);
                    word_hit.Add(word, 0);
                }
                word_cnt[word]++;
                if (status != "0" && status != "4")
                {
                    word_hit[word]++;
                }
            }
        }

        public static void PrepareTitleDocuments()
        {
             
            Dictionary<string, float> user_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_title_stat.csv", 0, 1);
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);


            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\word2vec\user_item_title_lines.txt";

            List<string> lines = new List<string>();

            Add2Lines(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\users.csv", user_titlefreq, lines);
            Add2Lines(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\items.csv", item_titlefreq, lines);

            var lines_arr = lines.ToArray();

            Tools.Common.Shuffle<string>(new Random(), lines_arr);

            using (StreamWriter wt = new StreamWriter(outfile))
            {
                foreach (var line in lines_arr)
                {
                    wt.Write(line + "\n");
                }
            }

        }

        private static void Add2Lines(string file, Dictionary<string, float> titlefreq, List<string> lines)
        {
            using (StreamReader rd = new StreamReader(file))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    string line = "";
                    string[] words = content.Split('\t');
                    if (!string.IsNullOrEmpty(words[1]))
                    {
                        string[] tokens = words[1].Split(',');
                        foreach (var token in tokens)
                        {
                            if (titlefreq.ContainsKey(token) && titlefreq[token] > 10)
                            {
                                line += "," + token;
                            }
                        }
                    }
                    if (line.Length > 1)
                    {
                        lines.Add(line.Substring(1));
                    }
                }
            }
        }
    }
}
