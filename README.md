This is our source code for <b>Recsys Challenge 2017</b> http://2017.recsyschallenge.com/. 
The official rank of our team is 5th, and our final model ranked 2 for the last 2 consecutive weeks. It is a huge pity that we didn't use the best model in the first two weeks (in the most of other competitions, updating the best model before the last minute of deadline is enough, however, this is not true in this competiton, which is different from what we were expected.).

The code is written with Microsoft's internal big data platform named COSMOS and the language is Scope. If you are interested in running it, you can try the public version in Azure, which is called Data Lake and U-SQL https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-data-lake-tools-get-started .

Scripts under folder 'models' are scripts for extracting features, training model, making predictions, and many more post processing.
The final features are in sparse format as SVMLight.

Programs under folder 'auto-pipeline' are c# source code for our automatic pipeline.

RecsysChallenge2017.pdf is our workshop paper, "Practical Lessons for Job Recommendations in the Cold-Start Scenario".  

