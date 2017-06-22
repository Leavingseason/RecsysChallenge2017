using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class ItemProfile
    {
        public static void BuildFeatureFile()
        {
            string label_file = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\itemprofile\offline_item_popularity.csv";
            string outfile_like = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\itemprofile\features\offline_training_like.csv";
            string outfile_hate = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\itemprofile\features\offline_training_hate.csv";

            Dictionary<string, Item> itemdict = FeatureFactory. BuildItemDict();  
             
            Dictionary<string, float> item_titlefreq = Utils.LoadDict(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_title_stat.csv", 0, 1);

            string keymapfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\word_id_mapping.csv";

            Dictionary<string, int> keymapper = DocumentClustering.LoadKeymapfile(keymapfile);

            using (StreamReader rd = new StreamReader(label_file))
            using(StreamWriter wt_like = new StreamWriter(outfile_like))
            using (StreamWriter wt_hate = new StreamWriter(outfile_hate))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split(',');
                    if (itemdict.ContainsKey(words[0]) && float.Parse(words[1])>0)
                    {
                        float book_ratio = float.Parse(words[3]);
                        float reply_ratio = float.Parse(words[4]);
                        float delete_ratio = float.Parse(words[5]);

                        string featureline = "";
                        List<int> titles = new List<int>();
                        foreach (var title in itemdict[words[0]].title)
                        {
                            if (keymapper.ContainsKey(title))
                            {
                                titles.Add(keymapper[title]);
                            }
                        }
                        titles.Sort();
                        foreach (var idx in titles)
                        {
                            featureline += " " + idx + ":1";
                        }

                        if (itemdict[words[0]].clevel == "0")
                        {
                            featureline += " " + (1+keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "1")
                        {
                            featureline += " " + (2 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "2")
                        {
                            featureline += " " + (3 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "3")
                        {
                            featureline += " " + (4 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "4")
                        {
                            featureline += " " + (5 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "5")
                        {
                            featureline += " " + (6 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "6")
                        {
                            featureline += " " + (7 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].clevel == "7")
                        {
                            featureline += " " + (8 + keymapper.Count) + ":1";
                        }

                        if (itemdict[words[0]].employment == "0")
                        {
                            featureline += " " + (9 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].employment == "1")
                        {
                            featureline += " " + (10 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].employment == "2")
                        {
                            featureline += " " + (11 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].employment == "3")
                        {
                            featureline += " " + (12 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].employment == "4")
                        {
                            featureline += " " + (13 + keymapper.Count) + ":1";
                        }
                        else if (itemdict[words[0]].employment == "5")
                        {
                            featureline += " " + (14 + keymapper.Count) + ":1";
                        }

                        featureline += " " + (15 + keymapper.Count) + ":" + itemdict[words[0]].tags.Count;
                        

                        if (delete_ratio > 0.1 || delete_ratio < 0.04)
                        {
                            int label = delete_ratio > 0.1 ? 1 : 0;
                            wt_hate.Write(label);
                            wt_hate.WriteLine(featureline);
                        }

                        if (book_ratio > 0.03 || reply_ratio > 0.03 || (book_ratio<0.02 && reply_ratio<0.02))
                        {
                            int label = book_ratio > 0.03 || reply_ratio > 0.03?1:0;
                            wt_like.Write(label);
                            wt_like.WriteLine(featureline);
                        }
                    }
                }
            }



        }
    }
}
