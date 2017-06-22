using Microsoft.SCOPE.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScopeRuntime;

public static class MyHelper
{
    public static Random rng = new Random();
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


    public static string GetUserIdFromLast(string str)
    {
        int idx = str.IndexOf("#");
        return str.Substring(idx + 1).Split(',')[0];
    }

    public static int GetRandomInt(int k)
    {
        return rng.Next(k);
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
        int topk = 250;

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
        int topk = 1000;

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


public class ShuffleLinesReducer : Reducer
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema input)
    {
        ScopeRuntime.Diagnostics.DebugStream.WriteLine("requestdColumns: {0}", string.Join(",", requestedColumns));
        return new Schema(
            "Line:string"
            );
    }

    public override IEnumerable<Row> Reduce(RowSet input, Row outputRow, string[] args)
    {
        List<string> lines = new List<string>();


        foreach (Row row in input.Rows)
        {
            lines.Add(row[1].String);
        }
        string[] array = lines.ToArray();

        int n = array.Length;
        Random rng = new Random();

        while (n > 1)
        {
            int k = rng.Next(n--);
            string tmp = array[n];
            array[n] = array[k];
            array[k] = tmp;
        }

        n = array.Length;
        if (n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                outputRow[0].Set(array[i]);
                yield return outputRow;
            }
        }

    }
}
