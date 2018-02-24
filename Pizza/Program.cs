using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Pizza {
    class Program {
        static void Main(string[] args) {






            StreamReader reader;
            int r = 0, c = 0, l = 0, h = 0;
            List<List<char>> dataSet = null;
            using (reader = new StreamReader(@"..\..\files\medium.in")) {
                //Read First Line
                var line = reader.ReadLine();
                if (line != null) {
                    var sline = line.Split();
                    r = int.Parse(sline[0]);
                    c = int.Parse(sline[1]);
                    l = int.Parse(sline[2]);
                    h = int.Parse(sline[3]);
                    dataSet = new List<List<char>>();
                    for (var i = 0; i < r; i++) {
                        line = reader.ReadLine();
                        dataSet.Add(line.ToCharArray().ToList());
                    }
                }

            }
            if (dataSet != null){
                
                Console.WriteLine(dataSet.Count);
                PizzaC p = new PizzaC(dataSet, h, l);
                List<Slice> slices = p.bestPiece;
                StreamWriter writer;
                using (writer = new StreamWriter(@"..\..\files\out.txt")) {
                    writer.WriteLine(slices.Count);
                    foreach (Slice slice in slices) {
                        Console.WriteLine(slice);
                        writer.WriteLine(slice.r1 + " " + slice.c1 + " " + slice.r2 + " " + slice.c2);
                    }

                }
            }
            Console.Read();
        }
    }


}
