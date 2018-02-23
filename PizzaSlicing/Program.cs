using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace pizza
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader;
            int r, c, l, h;
            List<List<char>> dataSet = null;
            using (reader = new StreamReader("inp.in"))
            {
                //Read First Line
                var line = reader.ReadLine();
                if (line != null)
                {
                    var sline = line.Split();
                    r = int.Parse(sline[0]);
                    c = int.Parse(sline[1]);
                    l = int.Parse(sline[2]);
                    h = int.Parse(sline[3]);
                    dataSet = new List<List<char>>();
                    for (var i= 0; i < r; i++)
                    {
                        line = reader.ReadLine();
                        dataSet.Add(line.ToCharArray().ToList());
                    }
                }

            }

            if (dataSet != null)
            {
                Pizza p = null;
                List<Slice> slices = p.result();
                StreamWriter writer;
                using (writer = new StreamWriter("out.txt"))
                {
                    writer.WriteLine(slices.Count);
                    foreach (Slice slice in slices)
                    {
                        writer.WriteLine(slice.r1 + " " + slice.c1 + " " + slice.r2 + " " + slice.c2);
                    }

                }
            }
        }
    }
    

}
