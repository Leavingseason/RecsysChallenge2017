This is our source code for <b>Recsys Challenge 2017</b> http://2017.recsyschallenge.com/. 
The official rank of our team is 5th, and our final model ranked 2 for the last 2 consecutive weeks. It is a huge pity that we didn't use the best model in the first two weeks (in the most of other competitions, updating the best model before the last minute of deadline is enough, however, this is not true in this competiton, which is different from what we were expected.).

The code is written with Microsoft's internal big data platform named COSMOS and the language is Scope. If you are interested in running it, you can try the public version in Azure, which is called Data Lake and U-SQL https://docs.microsoft.com/en-us/azure/data-lake-analytics/data-lake-analytics-data-lake-tools-get-started .

Scripts under folder 'models' are scripts for extracting features, training model, making predictions, and many more post processing.
The final features are in sparse format as SVMLight.

Programs under folder 'auto-pipeline' are c# source code for our automatic pipeline.

RecsysChallenge2017.pdf is our workshop paper, "Practical Lessons for Job Recommendations in the Cold-Start Scenario".  
https://dl.acm.org/citation.cfm?id=3124794
@inproceedings{Lian:2017:PLJ:3124791.3124794,
 author = {Lian, Jianxun and Zhang, Fuzheng and Hou, Min and Wang, Hongwei and Xie, Xing and Sun, Guangzhong},
 title = {Practical Lessons for Job Recommendations in the Cold-Start Scenario},
 booktitle = {Proceedings of the Recommender Systems Challenge 2017},
 series = {RecSys Challenge '17},
 year = {2017},
 isbn = {978-1-4503-5391-5},
 location = {Como, Italy},
 pages = {4:1--4:6},
 articleno = {4},
 numpages = {6},
 url = {http://doi.acm.org/10.1145/3124791.3124794},
 doi = {10.1145/3124791.3124794},
 acmid = {3124794},
 publisher = {ACM},
 address = {New York, NY, USA},
 keywords = {cold start, content-based filtering, job recommendations, recommendation systems, recsys challenge 2017},
} 
