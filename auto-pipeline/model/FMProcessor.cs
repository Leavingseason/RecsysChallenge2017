using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecSys17.model
{
    class FMProcessor
    {
        public static void AppendPredFile(string idfile, string predfile, string outfile)
        {
            using(StreamReader rd01 = new StreamReader(idfile))
            using(StreamReader rd02 = new StreamReader(predfile))
            using (StreamWriter wt = new StreamWriter(outfile))
            {
                string content = null;
                while ((content = rd01.ReadLine() )!= null)
                {
                    wt.Write("{0},{1}\n",content,rd02.ReadLine().Split(' ')[1]);
                }
            }
        }
    }
}
