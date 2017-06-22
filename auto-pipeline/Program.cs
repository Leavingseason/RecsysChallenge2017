using RecSys17.Data;
using RecSys17.model;
using RecSys17.OneML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecSys17
{
    class Program
    {
        static void Main(string[] args)
        { 
            //FeatureFactory.FilterByLR();

            //Tools.FileSpliter.SplitFiles(@"D:\competitions\Recsys17\my\train-test\test.csv", @"D:\competitions\Recsys17\my\train-test\split", 10, false);

            //MakeSubmission();
            

            //PostProcessSubmissionFile(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\ft30_no15.txt", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\ft30_no15_post.txt");

            //EarlyJobs();

            //Pipeline.Run();


            /////local train test
           // PrepareData();
            

            //MakeSubmission();


            //var userdict = FeatureFactory.BuildUserDict();
            //var itemdict = FeatureFactory.BuildItemDict();
            //using (StreamWriter wt = new StreamWriter(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\feature_stat\columns.csv"))
            //    for (int i = 46; i < 58; i++)
            //    {
            //        if (i == 2 || (i >= 17 && i <= 38))
            //            continue;
            //        SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test02_morefeature.csv",
            //                   @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\rule.out.txt", new int[] { 2 }, i, -100, ',', false, userdict, itemdict, null); //, (x) => { return double.Parse(x[16]) + double.Parse(x[17]); }
            //        Console.Write("col:{0}\t", i);
            //        int[] scores = Evaluation.Score02(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\rule.out.txt",
            //            @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv", userdict, itemdict);
            //        wt.Write("{0}", i);
            //        for (int k = 0; k < scores.Length; k++)
            //        {
            //            wt.Write(",{0}", scores[k]);
            //        }
            //        wt.WriteLine();
            //        wt.Flush();
            //    }





                //Utils.SelectSubSet(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\feature\train_interactions_sample1_0.1.csv", 
                //                    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\stat\subfeature.csv", 
                //                    new List<int>(new int[] { 10,11,12}), 1000000);

                // DocumentRelated.PrepareTitleDocuments();

                // FeatureFactory.GenTestFeature();

                //            FeatureFactory.AppendHeader(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\feature\test_interactions - Copy.csv",
                //@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\feature\test_interactions_hasheader.csv");

                //Tools.Evaluation.CalcLiftCurve(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\feature\TLC104\8.inst.txt",
                //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\feature\TLC104\8.lift.csv");

                // KNN.PredictByViewDocsim();

                // KNN.PredictByUserDocsim();
                // KNN.PredictByViewDocsim();
            //KNN.PredictByViewDocsim();

           


           // KNN.PredictFromClosestJobs();

                //Tools.FileMerger.MergeFiles(new string[] { @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train_pairs_part0.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train_pairs_part1.csv" },
                //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train_feature_merged.csv", false);


                //SmallJobs.StatActionWithCriteria(1);
                //SmallJobs.SelectCandidates();

            //DocumentRelated.GenKeyWords();

//            SubmissionHelper.Ensemble(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\user_viewed_doc_localtest.txt",
//@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\user_doc_localtest.txt", 
//@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\ensemble.txt");


            //Random rng = new Random();
            //var userdict = FeatureFactory.BuildUserDict();
            //var itemdict = FeatureFactory.BuildItemDict();
            ////Evaluation.Score(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ft-local-test_submit.txt",
            ////    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv", null, null, false);

            //SubmissionHelper.GenSubFileFromTLC(
            //           @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\0.inst.txt",
            //          @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\FT.csv", new int[] { 0,1 }, 4, 0, '\t', true, userdict, itemdict);
            //Evaluation.Score(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\FT.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv", userdict, itemdict);

            //SubmissionHelper.GenSubFileFromTLC(
            //           @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\ranking\TLC\13.inst.txt",
            //          @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\ranking\TLC\ranking.csv", new int[] { 0, 1 }, 3, 0, '\t', true, userdict, itemdict);
            //Evaluation.Score(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\FT.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv", userdict, itemdict);

            ////SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test.csv",
            ////           @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\rule.out.txt", 2, 3, -100, ',', false, userdict, itemdict, (x) => { return double.Parse(x[14]) + 0.2 * double.Parse(x[15]); });

            //SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test02_morefeature_20w.csv",
            //           @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\randon.out.txt", new int[] { 2 }, 14, 0.9, ',', false, userdict, itemdict, (x) => { return rng.NextDouble(); });
            //Evaluation.Score(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\randon.out.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv", userdict, itemdict);

            //Evaluation.StatRecall(
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ft-local-test.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ft-local-test_recall.txt"
            //    );

            //Tools.Evaluation.CalcLiftCurve(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ft-local-test.txt",
            //   @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ft-local-test_lift.csv");

          // EnsembleExp();

            //SmallJobs.OutputPremiumUsers();


/////////////////////// online data alignment
           string date = "2017-06-12";

           Submit_full_pipeline(date);

       //   Submit_full_pipeline_cos14(date);

//////////////////////////////////////////////////////////////////





               //FMProcessor.AppendPredFile(
               //    @"D:\tmp\recsys2017\offline\test_id_FM.csv",
               //    @"D:\tmp\recsys2017\alphaFM-master\data\output_20.txt",
               //    @"D:\tmp\recsys2017\alphaFM-master\data\FM_pred_20.csv"
               //    );


               //Console.WriteLine("TRAIN02");
               //string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\Small_IDs_FM\training_part01_svmlight.txt";
               //string outfile01 = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\training_part01_FM.txt";
               //string outfile02 = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\training_part01_FM_id.txt";
               //CleanOnlineData.PrepareFMFile(infile, outfile01, outfile02);
               //Console.WriteLine("TEST");
               //infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\Small_IDs_FM\training_part02_svmlight.txt";
               //outfile01 = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\training_part02_FM.txt";
               //outfile02 = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\training_part02_FM_id.txt";
               //CleanOnlineData.PrepareFMFile(infile, outfile01, outfile02);



               //   ItemProfile.BuildFeatureFile();

               //  WordHashing.BuildWordHashing();


               //Console.WriteLine(Tools.Common.ParseTime(double.Parse("1493580123")));


              // LocalDataGenrator.Pipeline();

               //LocalDataGenrator.GenWeightData(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\Small_IDs_FM\interactions_grouped.csv",
               //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\Small_IDs_FM\training_part02_svmlight.txt",
               //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\offline\Small_IDs_FM\training_part02_svmlight_weighted.txt");


               //            SmallJobs.OverlapStat(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_adj.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_2017-04-30.txt", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\items_overlap.csv");
               //            SmallJobs.OverlapStat(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\users.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_users_2017-04-30.txt", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\users_overlap.csv");

               //    LocalDataGenrator.GenWord2trigram();


               //  SmallJobs.VobcabStat();
            
               //   SmallJobs.TitleLengthStat();

               //VeryItem2Submit();


               ////DateTime start = DateTime.Parse("2017-04-30");
               ////DateTime end = DateTime.Parse("2017-05-19");
               ////for (var curdate = start; curdate.CompareTo(end) <= 0; curdate = curdate.AddDays(1))
               ////{
               ////    Console.WriteLine(curdate);
               ////    MergeItems(
               ////    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_" + curdate.ToString("yyyy-MM-dd") + ".txt",
               ////        @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv"
               ////        );
               ////}

               ////DocumentClustering.PrepareFeatureFile();
               ////Tools.AutoExperiments.RunCmd(@"D:\tools\TLC_3.6.65.0\maml.exe", @"Test weight={} group={} name={} sf=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\TLC\-1.summary.txt dout=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\TLC\-1.inst.txt data=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\training.txt in=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\TLC.model.zip", false);
               ////DocumentClustering.TestGenClusterIdFeature();
               ////Console.WriteLine("Complete TestGenClusterIdFeature.");

               Console.WriteLine("Mission Complete.");
            Console.ReadKey();
        }

        private static void AppendHeader(string infile, string outfile, string headerfile)
        {
            string header = null;
            List<string> w = new List<string>();
            using (StreamReader rd = new StreamReader(headerfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        w.Add(content);
                    }
                }
            }
            header = string.Join(",", w.ToArray());

            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                wt.WriteLine(header);
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    wt.WriteLine(content);
                }
            }
        }

        private static void Submit_full_pipeline(string date)
        {
            while (true)
            {
                FileInfo new_file_flag = new FileInfo(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_" + date + ".txt");
                if (!new_file_flag.Exists)
                {
                    Console.WriteLine("Not ready.");
                    Thread.Sleep(1000 * 60 * 10);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Ready.");
            Thread.Sleep(1000 * 60 * 10);

            CleanOnlineData.AdjustItemColumns(
            @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_" + date + ".txt",
            @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_" + date + ".txt"
          );

            MergeItems(
            @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_" + date + ".txt",
                @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv"
                );
            Console.WriteLine("Complete Item updating.");

            LocalDataGenrator.GenOnlineTestCandidatePairs(date);
            Console.WriteLine("Complete generating test candidates.");

            DocumentClustering.PrepareFeatureFile();
            Tools.AutoExperiments.RunCmd(@"D:\tools\TLC_3.6.65.0\maml.exe", @"Test weight={} group={} name={} sf=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\TLC\-1.summary.txt dout=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\TLC\-1.inst.txt data=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\training.txt in=\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\TLC.model.zip", false);
            DocumentClustering.TestGenClusterIdFeature();
            Console.WriteLine("Complete TestGenClusterIdFeature.");

            using (StreamWriter wt = new StreamWriter(@"D:\for-recsys-2017\flags\ready_" + date))
            {
                wt.WriteLine("ready from cos09");
            }

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/cluster_id_mapping.tsv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy  -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\cluster_id_mapping.tsv https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/cluster_id_mapping.tsv", false);

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/relevance/local/users/v-lianji/cluster_id_mapping.tsv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy  -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\cluster_id_mapping.tsv https://MS-confidential/relevance/local/users/v-lianji/cluster_id_mapping.tsv", false);
            Console.WriteLine("Complete uploading cluster_id_mapping.");

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/items_noheader.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/items_noheader.csv", false);

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/relevance/local/users/v-lianji/items_noheader.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv https://MS-confidential/relevance/local/users/v-lianji/items_noheader.csv", false);
            Console.WriteLine("Complete uploading item_noheader.");

            //Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/test_candidates_"
            //    + date + @".txt", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\candidates\test_candidates_"
                + date + @".txt https://MS-confidential/MSRWW.adhoc/local/users/v-lianji/test_candidates_"
                + date + @".txt", false);

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\candidates\test_candidates_"
             + date + @".txt https://MS-confidential/relevance/local/users/v-lianji/test_candidates_"
             + date + @".txt", false);
            Console.WriteLine("Complete uploading test candidates.");


            Console.WriteLine("Running Scope jobs...");
            ////  //ExeScopeJob("Daily_Auto_ExtractFeatures", date, "daily_extract_test_sparse");
            ////  //ExeScopeJob("Daily_Auto_PrepareFeatureFile", "", "daily_extract_test_svmlight");
            ////  //ExeScopeJob("Daily_Auto_Localmodel_tlc3_pipeline", "", "daily_testing");
            ExeScopeJob("Online_pipeline_20170601\\ExtractFeatures", date, "daily_extract_test_sparse");
            ExeScopeJob("Online_pipeline_20170601\\PrepareFeatureFile", "", "daily_extract_test_svmlight");
            ExeScopeJob("Online_pipeline_20170601\\Localmodel_tlc3_pipeline", "", "daily_testing");


            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v0.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v0.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v1.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v1.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v2.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v2.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v3.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v3.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v4.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v4.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v5.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v5.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v6.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v6.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v7.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v7.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/relevance/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v_userside.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online\recsys17-pred-highdim-submit_v_userside.csv", false);


            Tools.AutoExperiments.RunCmd(@"python", @"D:\for-recsys-2017\online_submit_auto.py", false);

            CleanOnlineData.AppendLossPairs(DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd"), 1);

            Tools.AutoExperiments.RunCmd(@"python", @"D:\for-recsys-2017\online_submit_auto_-1.py", false);
        }

        private static void Submit_full_pipeline_cos14(string date)
        {
            while (true)
            {
                FileInfo new_file_flag = new FileInfo(@"D:\for-recsys-2017\flags\ready_" + date);
                if (!new_file_flag.Exists)
                {
                    Console.WriteLine("Not ready.");
                    Thread.Sleep(1000 * 60);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Ready.");
            Thread.Sleep(10000);


            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/office.engineering/local/users/v-lianji/cluster_id_mapping.tsv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy  -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\clustering\online\cluster_id_mapping.tsv https://MS-confidential/office.engineering/local/users/v-lianji/cluster_id_mapping.tsv", false);

            Console.WriteLine("Complete uploading cluster_id_mapping.");

            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-delete  https://MS-confidential/office.engineering/local/users/v-lianji/items_noheader.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\data_online\online\items_noheader.csv https://MS-confidential/office.engineering/local/users/v-lianji/items_noheader.csv", false);

            Console.WriteLine("Complete uploading item_noheader.");



            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -text  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\candidates\test_candidates_"
                + date + @".txt https://MS-confidential/office.engineering/local/users/v-lianji/test_candidates_"
                + date + @".txt", false);

            Console.WriteLine("Complete uploading test candidates.");


            Console.WriteLine("Running Scope jobs...");
            //////ExeScopeJob("Daily_Auto_ExtractFeatures", date, "daily_extract_test_sparse", "https://MS-confidential/office.engineering/");
            //////ExeScopeJob("Daily_Auto_PrepareFeatureFile", "", "daily_extract_test_svmlight", "https://MS-confidential/office.engineering/");
            //////ExeScopeJob("Daily_Auto_Localmodel_tlc3_pipeline", "", "daily_testing", "https://MS-confidential/office.engineering/");

            ExeScopeJob("Online_pipeline_20170601\\ExtractFeatures", date, "daily_extract_test_sparse", "https://MS-confidential/office.engineering/");
            ExeScopeJob("Online_pipeline_20170601\\PrepareFeatureFile", "", "daily_extract_test_svmlight", "https://MS-confidential/office.engineering/");
            ExeScopeJob("Online_pipeline_20170601\\Localmodel_tlc3_pipeline", "", "daily_testing", "https://MS-confidential/office.engineering/");



            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v0.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v0.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v1.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v1.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v2.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v2.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v3.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v3.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v4.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v4.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v5.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v5.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v6.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v6.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v7.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v7.csv", false);
            Tools.AutoExperiments.RunCmd(@"https://MS-confidential.exe", @"-copy -overwrite -text  https://MS-confidential/office.engineering/my/RecSys2017/pipeline/results/recsys17-pred-highdim-submit_v_userside.csv  \\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\online_2\recsys17-pred-highdim-submit_v_userside.csv", false);


            Tools.AutoExperiments.RunCmd(@"python", @"D:\for-recsys-2017\online_submit_auto_2.py", false);

            //CleanOnlineData.AppendLossPairs(DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd"), 1);

            //Tools.AutoExperiments.RunCmd(@"python", @"D:\for-recsys-2017\online_submit_auto_-1.py", false);
        }


        private static void ExeScopeJob(string scriptname, string date, string friend_name, string vc = "https://MS-confidential/relevance/")
        {
            string guid = Guid.NewGuid().ToString();
            if (!string.IsNullOrEmpty(date))
            {
                Tools.AutoExperiments.RunCmd("D:\\tools\\ScopeSDK\\scope.exe", "-submit -i \"D:\\My Projects\\LyncOnlineAnalyse\\DataMiningWithScope\\RecSys17\\" + scriptname + ".script\" -params cur_date=\"\"\"" + date + "\"\"\" -vc  " + vc + " -f " + friend_name + " -jobId " + guid, false);
            }
            else
            {
                Tools.AutoExperiments.RunCmd("D:\\tools\\ScopeSDK\\scope.exe", "-submit -i \"D:\\My Projects\\LyncOnlineAnalyse\\DataMiningWithScope\\RecSys17\\" + scriptname + ".script\"  -vc  " + vc + " -f " + friend_name + " -jobId " + guid, false);
            }

            Thread.Sleep(1000 * 60 * 10);
            
            bool finish = false;
            while (true)
            {
                Tools.AutoExperiments.RunCmd("D:\\tools\\ScopeSDK\\scope.exe", "-jobstatus " + guid + " -vc  " + vc + "  -overwrite -o D:\\tmp\\scope_job_status" + guid + ".txt ", false);

                finish = QueryJobStatus("D:\\tmp\\scope_job_status" + guid + ".txt");
                Console.WriteLine("Job_{1} = isfinished={0}", finish, friend_name);
                if (finish)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000 * 60 * 10);
                }
            }
        }

        private static bool QueryJobStatus(string p)
        {
            string content = null;
            using (StreamReader rd = new StreamReader(p))
            {
                content = rd.ReadToEnd();
            }
            return   content.Contains("state=\"CompletedSuccess\"");
        }

        private static void VeryItem2Submit()
        {
            string date = "2017-05-15";
            string target_items = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_items_adj_" + date + ".txt";
            string target_users = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\target_users_" + date + ".txt";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\candidates\test_candidates_" + date + ".txt";

            HashSet<string> items = new HashSet<string>();
            HashSet<string> users = new HashSet<string>();


            using (StreamReader rd = new StreamReader(target_items))
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

            string testfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\second-stage\online-schedule\pull-data\interactions\interaction_2017-05-16.txt";
            using (StreamReader rd = new StreamReader(testfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (users.Contains(words[0]) && items.Contains(words[1]))
                    {
                        Console.WriteLine(content);
                    }
                }
            }

        }

        private static void MergeItems(string newfile, string file)
        {
            List<string> lines = new List<string>();

            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    lines.Add(content + "\n");
                }
            }

            using (StreamReader rd = new StreamReader(newfile))
            {
                string content = rd.ReadLine();
                while ((content = rd.ReadLine()) != null)
                {
                    lines.Add(content + "\n");
                }
            }

            using (StreamWriter wt = new StreamWriter(file))
            {
                foreach (var line in lines)
                {
                    wt.Write(line);
                }
            }
        }

        private static void EnsembleExp()
        {

            SubmissionHelper.Ensemble(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\pred_f13.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\pred_f15.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\output\ensemble01.txt", 1, 100, 0);


            SubmissionHelper.Ensemble(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\user_viewed.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\output\ensemble01.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\output\ensemble02.txt", 1, 100, 50);

            SubmissionHelper.Ensemble(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\output\ensemble02.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\pred_f11.txt",
@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\output-cosmos\ensemble\output\ensemble03.txt", 1, 100, 100);


        }




        public static void PrepareData()
        {
            Console.WriteLine("PrepareData");
            string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            string outfile_test = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\test_interactions.csv";
            string outfile_train = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\train_interactions.csv";
            Dictionary<string, int> item2poscnt = LoadItem2PosCnt(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv");
            int test_item_cnt = 1000;
            int pos_thresh = 5;

            List<string> candidates = new List<string>();
            foreach (var pair in item2poscnt)
            {
                if (pair.Value > pos_thresh)
                {
                    candidates.Add(pair.Key);
                }
            }

            Random rng = new Random();
            HashSet<string> selected_items = new HashSet<string>();
            while ((test_item_cnt-- > 0))
            {
                int idx = rng.Next(candidates.Count);
                selected_items.Add(candidates[idx]);
                candidates.RemoveAt(idx);
            }

            using (StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt01 = new StreamWriter(outfile_train))
            using (StreamWriter wt02 = new StreamWriter(outfile_test))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (selected_items.Contains(words[1]))
                    {
                        wt02.WriteLine(content);
                    }
                    else
                    {
                        wt01.WriteLine(content);
                    }
                }
            }

        }

        private static Dictionary<string, int> LoadItem2PosCnt(string file)
        {
            HashSet<string>　interest_actions = new HashSet<string>(new string[]{"1","2","3","4","5"});
            Dictionary<string, int> res = new Dictionary<string, int>();
            using (StreamReader rd = new StreamReader(file))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    if (words[2] != "0")
                    {
                        if (!res.ContainsKey(words[1]))
                        {
                            res.Add(words[1], 1);
                        }
                        else
                        {
                            res[words[1]]++;
                        }
                    }
                }
            }
            return res;
        }

        private static void EarlyJobs()
        {
            //SmallJobs.VobcabStat();

            //SmallJobs.UserItemStat();
            //SmallJobs.StatUserInterest();

            //SmallJobs.TargetItemExplore();

            //LocalDataGenrator.GenLocalTestFile();
            //LocalDataGenrator.SplitLocalTestFile();

            //StatItemRecordCnt(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\item_record_cnt.csv", 1);
            //StatItemRecordCnt(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\user_record_cnt.csv", 0);

            //StatMatchUserTitleOverlap("0");
            //StatMatchUserTitleOverlap("1");
            //StatMatchUserTitleOverlap("2");
            //StatMatchUserTitleOverlap("3");
            //StatMatchUserTitleOverlap("4");
            //StatMatchUserTitleOverlap("5");


            //Tools.FileSpliter.SampleInstances(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train_format.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\train_format_sampling0.2.csv", 0.2, false);

            LocalDataGenrator.SplitFileIntoPartsByGroup(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_format.csv",
                @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\split",
                ',', 3, 5);
        }

        private static void StatMatchUserTitleOverlap(string type)
        {
            string infile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\interactions_grouped.csv";
            string outfile = @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\stat\tmp\"+type+".csv";

            Dictionary<string, User> userdict =FeatureFactory. BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();

            int sample_cnt = 100000; 

            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                wt.WriteLine("title,title_tag,clevel,indus,country");
                int cnt = 0;
                while ((content = rd.ReadLine()) != null)
                { 
                    string[] words = content.Split('\t');
                    if (words[2].Equals(type) && userdict.ContainsKey(words[0]) && itemdict.ContainsKey(words[1]))
                    {
                        if (cnt++ >= sample_cnt)
                        {
                            break;
                        }
                        if (cnt % 10000 == 0)
                        {
                            Console.WriteLine(cnt);
                        }
                        var user = userdict[words[0]];
                        var item = itemdict[words[1]];
                        wt.WriteLine("{0},{1},{2},{3},{4}",user.title.Intersect(item.title).Count(),
                            user.title.Intersect(item.tags).Count(),
                            user.clevel==item.clevel ?1:0,
                            user.indus==item.indus ?1:0,
                            user.country==item.country ?1:0
                            );
                    }
                }
            }
        }

        private static void StatItemRecordCnt(string infile,string outfile,int idx)
        {
            Dictionary<string, int> iid2cnt = new Dictionary<string, int>();
            using (StreamReader rd = new StreamReader(infile))
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
                    if (!iid2cnt.ContainsKey(words[idx]))
                    {
                        iid2cnt.Add(words[idx], 1);
                    }
                    else
                    {
                        iid2cnt[words[idx]]++;
                    }
                }

            }

            Utils.OutputDict<string, int>(iid2cnt, outfile);
        }

        private static void MakeSubmission()
        {
            //SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\TLC2\4.inst.txt", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\FT_1_0.2_2000_V4.txt", 0, 2, 0.1);
            //SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank.inst.txt", @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank.txt", 0, 3, -100);
            //SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\0.inst.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank_0.txt", new int[] { 0, 1 }, 3, -100);

            SubmissionHelper.GenSubFileFromTLC(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\LR_local_complete_0.inst.txt",
                @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\lr.localmodel02.txt", new int[] { 0 }, 2, -100, '\t', true);

            //SubmissionHelper.GenSubFileFromTLCWithAlignment(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\0.inst.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\test_complete_0_format.csv",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank_0.txt");

            //ReplaceGroupId(@"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank_0.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\train-test\inter-media\feature\TLC\fastrank_0_submit.txt",
            //    @"\\mlsdata\e$\Users\v-lianji\mlsdata\Recsys17\submit\fastrank_0.txt");

        }

        private static void ReplaceGroupId(string infile, string outfile, string reffile)
        {
            using(StreamReader rd01 = new StreamReader(infile))
            using(StreamReader rd02 = new StreamReader(reffile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = "";
                while ((content = rd01.ReadLine()) != null)
                {
                    string[] tokens = content.Split('\t');
                    string[] tokens02 = rd02.ReadLine().Split('\t');
                    wt.Write("{0}\t{1}\n", tokens02[0], tokens[1]);
                }
            }
        }

        public static void PostProcessSubmissionFile(string infile, string outfile)
        {
            Dictionary<string, User> userdict = FeatureFactory.BuildUserDict();
            Dictionary<string, Item> itemdict = FeatureFactory.BuildItemDict();

            using(StreamReader rd = new StreamReader(infile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                while ((content = rd.ReadLine()) != null)
                {
                    string[] words = content.Split('\t');
                    string[] tokens = words[1].Split(',');
                    if (tokens.Length > 0)
                    {
                        wt.Write("{0}",words[0]);
                        int cnt=0;
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (userdict[tokens[i]].title.Intersect(itemdict[words[0]].title).Count() > 0)
                            {
                                if (cnt == 0)
                                {
                                    wt.Write("\t{0}",tokens[i]);
                                }
                                else
                                {
                                    wt.Write(",{0}", tokens[i]);
                                }
                                cnt++;
                            }
                        }
                        wt.Write("\n");
                    }
                }
            }
        }
    }
}
