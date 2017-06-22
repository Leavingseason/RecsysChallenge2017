using Microsoft.SCOPE.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScopeRuntime;



public static class MyHelper
{
    public static Dictionary<string, User> userdict = null;

    static MyHelper()
    {
        userdict = BuildUserDict();
    }

    public static double PromotePremiumUsers(string uid, double p, double up_ratio)
    {
        if (userdict.ContainsKey(uid) && userdict[uid].premium == "1")
        {
            return p * (1 + up_ratio);
        }
        else
        {
            return p;
        }
    }

    public static Dictionary<string, User> BuildUserDict()
    {
        Dictionary<string, User> res = new Dictionary<string, User>();
        using (StreamReader rd = new StreamReader(@"users_noheader.csv"))
        {
            string content = null;
            while ((content = rd.ReadLine()) != null)
            {
                if (content.StartsWith("id"))
                {
                    continue;
                }
                User user = new User(content);
                if (!res.ContainsKey(user.id))
                {
                    res.Add(user.id, user);
                }
            }
        }
        return res;
    }

    public static string GetIDIndex(string line)
    {
        int idx = line.IndexOf("#");
        return line.Substring(idx + 1);
    }

    public static string GetUserId(string str)
    {
        int idx = str.IndexOf(",");
        return str.Substring(0, idx);
    }

    public static string GetItemId(string str)
    {
        int idx = str.IndexOf(",");
        return str.Substring(idx + 1);
    }
}


public class User
{
    public string id;
    public HashSet<string> title;
    public Dictionary<string, float> title2cnt;
    public int title_cnt;
    public string clevel;
    public string indus;
    public string disc;
    public string country;
    public string region;
    public string experience_n_entries_class;
    public string experience_years_experience;
    public string experience_years_in_current;
    public string edu_degree;
    public HashSet<string> edu_fieldofstudies;
    public string wtcj;
    public string premium;

    public List<Tuple<string, int>> interactions;
    public Dictionary<string, int> viewed_item_title_words;
    public double viewed_titem_title_cnt;



    public User() { }
    public User(string line)
    {
        string[] words = line.Split('\t');

        id = words[0];
        title = new HashSet<string>();
        title2cnt = new Dictionary<string, float>();
        title_cnt = 0;
        var tokens = words[1].Split(',');
        title_cnt = tokens.Length;
        foreach (var token in tokens)
        {
            title.Add(token);
            if (!title2cnt.ContainsKey(token))
            {
                title2cnt.Add(token, 1.0f / title_cnt);
            }
            else
            {
                title2cnt[token] += 1.0f / title_cnt;
            }
        }
        clevel = words[2];
        disc = words[3];
        indus = words[4];
        country = words[5];
        region = words[6];
        experience_n_entries_class = words[7];
        experience_years_experience = words[8];
        experience_years_in_current = words[9];
        edu_degree = words[10];
        edu_fieldofstudies = new HashSet<string>();
        foreach (var token in words[11].Split(','))
        {
            //if (token != "000")
            {
                edu_fieldofstudies.Add(token);
            }
        }
        wtcj = words[12];
        premium = words[13];

        viewed_titem_title_cnt = 0;
        interactions = null;
        viewed_item_title_words = null;
    }
}



public class TopInstanceSelection : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "uid:string,iid:string,Probability:float"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = 5000;

        string iid = "";

        List<Tuple<string, float>> uid_score_list = new List<Tuple<string, float>>();


        foreach (Row row in input.Rows)
        {
            iid = row[1].String;

            string uid = row[0].String;

            float score = row[2].Float;

            uid_score_list.Add(new Tuple<string, float>(uid, score));
        }

        uid_score_list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        int k = Math.Min(topk, uid_score_list.Count);

        for (int i = 0; i < k; i++)
        {
            outputRow[0].Set(uid_score_list[i].Item1);
            outputRow[1].Set(iid);
            outputRow[2].Set(uid_score_list[i].Item2);
            yield return outputRow;
        }

    }
}


public class SubmissionFormater : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "ItemId:string,TopUserId:string"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = 100;

        string iid = "";

        List<Tuple<string, float>> uid_score_list = new List<Tuple<string, float>>();


        foreach (Row row in input.Rows)
        {
            iid = row[1].String;

            string uid = row[0].String;

            float score = row[2].Float;

            uid_score_list.Add(new Tuple<string, float>(uid, score));
        }

        uid_score_list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        int k = Math.Min(topk, uid_score_list.Count);

        string value = "";
        for (int i = 0; i < k; i++)
        {
            value += "," + uid_score_list[i].Item1;
        }

        if (value.Length > 0)
        {
            outputRow[0].Set(iid);
            outputRow[1].Set(value.Substring(1));
        }
        else
        {
            outputRow[0].Set(iid);
            outputRow[1].Set("");
        }

        yield return outputRow;

    }
}




public class OnlineSubmissionFormater : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "ItemId:string,TopUserId:string"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = 250;

        string iid = "";
        string uid = "";
        float score = 0;

        List<Tuple<string, string, float>> score_list = new List<Tuple<string, string, float>>();


        foreach (Row row in input.Rows)
        {
            iid = row[1].String;

            uid = row[0].String;

            score = row[2].Float;

            score_list.Add(new Tuple<string, string, float>(uid, iid, score));
        }

        score_list.Sort((a, b) => b.Item3.CompareTo(a.Item3));

        Dictionary<string, List<string>> iid2uids = new Dictionary<string, List<string>>();
        HashSet<string> visited_uids = new HashSet<string>();
        foreach (var tuple in score_list)
        {
            uid = tuple.Item1;
            iid = tuple.Item2;
            if (!visited_uids.Contains(uid))
            {
                if (!iid2uids.ContainsKey(iid))
                {
                    iid2uids.Add(iid, new List<string>());
                }
                if (iid2uids[iid].Count < topk)
                {
                    iid2uids[iid].Add(uid);
                    visited_uids.Add(uid);
                }
            }
        }


        foreach (var pair in iid2uids)
        {
            outputRow[0].Set(pair.Key);
            string res = "";
            foreach (var tuid in pair.Value)
            {
                res += "," + tuid;
            }
            if (res.Length <= 0)
            {
                res = ", ";
            }
            outputRow[1].Set(res.Substring(1));
            yield return outputRow;
        }

    }
}


public class TopKSelector : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "UserId:string,ItemId:string,Probability:float,holder:string"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = 3000;

        string iid = "";

        List<Tuple<string, float>> uid_score_list = new List<Tuple<string, float>>();


        foreach (Row row in input.Rows)
        {
            iid = row[1].String;

            string uid = row[0].String;

            float score = row[2].Float;

            uid_score_list.Add(new Tuple<string, float>(uid, score));
        }

        uid_score_list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        int k = Math.Min(topk, uid_score_list.Count);


        for (int i = 0; i < k; i++)
        {
            outputRow[0].Set(uid_score_list[i].Item1);
            outputRow[1].Set(iid);
            outputRow[2].Set(uid_score_list[i].Item2);
            outputRow[3].Set("1");
            yield return outputRow;
        }

    }
}

public class TopKSelectorUserSide : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "UserId:string,ItemId:string,Probability:float"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = 500;

        string uid = "";

        List<Tuple<string, float>> iid_score_list = new List<Tuple<string, float>>();


        foreach (Row row in input.Rows)
        {
            uid = row[0].String;

            string iid = row[1].String;

            float score = row[2].Float;

            iid_score_list.Add(new Tuple<string, float>(iid, score));
        }

        iid_score_list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        int k = Math.Min(topk, iid_score_list.Count);


        for (int i = 0; i < k; i++)
        {
            outputRow[1].Set(iid_score_list[i].Item1);
            outputRow[0].Set(uid);
            outputRow[2].Set(iid_score_list[i].Item2);
            yield return outputRow;
        }

    }
}


public class UserTopSecondSelector : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "uid:string,iid:string,Probability:float,Place:int"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        int topk = int.Parse(args[0]);

        string uid = "";

        List<Tuple<string, float>> iid_score_list = new List<Tuple<string, float>>();


        foreach (Row row in input.Rows)
        {
            uid = row[0].String;

            string iid = row[1].String;

            float score = row[2].Float;

            iid_score_list.Add(new Tuple<string, float>(iid, score));
        }

        if (iid_score_list.Count < 1)
        {
            outputRow[0].Set("");
            outputRow[1].Set("");
            outputRow[2].Set(0);
            yield return outputRow;
        }
        else
        {
            iid_score_list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            for (int i = 0; i < topk; i++)
            {
                outputRow[0].Set(uid);
                outputRow[1].Set(iid_score_list[i].Item1);
                outputRow[2].Set(iid_score_list[i].Item2);
                outputRow[3].Set((i + 1).ToString());
                yield return outputRow;
            }

        }

    }
}
