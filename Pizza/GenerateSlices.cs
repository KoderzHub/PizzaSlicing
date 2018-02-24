using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pizza {
    public class Pizza {
        List<List<char>> _pizza;
        int Max;
        int MinEach;
        int size;
        List<char> Poss;
        int LeastWaste;
        List<Slice> AllSlices;
        Dictionary<Cell, HashSet<Slice>> Cell;

        int best;
        HashSet<Slice> bestPiece;
        bool FoundBest = false;


        Dictionary<char, int> Total;

        Stopwatch sw = new Stopwatch();
        public Pizza(List<List<char>> Pizza, int Max, int MinEach) {
            this._pizza = Pizza;
            this.Max = Max;
            this.MinEach = MinEach;

            HashSet<char> poss = new HashSet<char>();
            Total = new Dictionary<char, int>();
            foreach (List<char> c in Pizza) {
                foreach(char i in c) {
                    poss.Add(i);
                    if (Total.ContainsKey(i)) {
                        Total[i]++;
                    } else {
                        Total.Add(i, 1);
                    }
                }
            }
            this.Poss = poss.ToList();
          
        
            if (_pizza.Count > 0) {
                size = (_pizza.Count * _pizza[0].Count);
                best = int.MaxValue;
                bestPiece = new HashSet<Slice>();
                AllSlices = new List<Slice>();
               
                Cell = new Dictionary<Cell, HashSet<Slice>>();
                Generator();
            } else {
                size = 0;
            }
        }
        void Generator() {
            sw.Start();
            List<Slice> sc = new List<Slice>();
            for(int r1 = 0; r1 < _pizza.Count; r1++) {
                for(int c1 = 0; c1 < _pizza[r1].Count; c1++) {
                    for (int r2 = r1; r2 < _pizza.Count; r2++) {
                        for (int c2 = c1; c2 < _pizza[r2].Count; c2++) {
                            Dictionary<char, int> map = new Dictionary<char, int>(); 
                            if (CheckSquare(r1, c1, r2, c2,map)) {
                                

                                Slice s = new Slice(r1, c1, r2, c2,map);
                                AllSlices.Add(s);
                                foreach (Cell c in s.AllCells) {
                                    if (Cell.ContainsKey(c)) {
                                        Cell[c].Add(s);
                                        
                                    } else {
                                        Cell.Add(c, new HashSet<Slice> { s });
                                        
                                    }
                                }
                            }


                        }
                    }


                }
            }
            LeastWaste = size - Cell.Count();
            Console.WriteLine("Generator finished after"+sw.ElapsedMilliseconds);
        }
        bool CheckSquare(int r1, int c1, int r2, int c2,Dictionary<char,int> mp) {
            Dictionary<char, bool> bl = new Dictionary<char, bool>();
            foreach(char c in Poss) {
                bl.Add(c, false);
                mp.Add(c, 0);
            }
            if (((r2 - r1 + 1) * (c2 - c1 + 1)) > Max) {
                return false;
            }
            for(int i = r1; i <= r2; i++) {
                for(int j = c1; j <= c2; j++) {
                    mp[_pizza[i][j]]++;
                    bl[_pizza[i][j]] = true;
                }
            }

            return IsAllTrue(bl);
        }
        bool IsAllTrue(Dictionary<char,bool> all) {
            foreach(bool a in all.Values) {
                if (a == false) {
                    return false;
                }
            }
            return true; ;
        }



        void SortSlices(List<Slice> sc, Dictionary<char, int> remaining) {
            //Slice with lowest number of possible ways first
            //Slice containing most cells
            //Slice with best remaining possible pices
            int most = AllSlices.Count;
            foreach (Cell c in Cell.Keys) {
                foreach(Slice s in Cell[c]) {
                    s.Weigth += ((most - Cell[c].Count) * 100);
                }
            }
            Dictionary<char, int> myRem = new Dictionary<char, int>();
            foreach (Slice s in sc) {
                s.Weigth += s.size * 10;
                foreach (Cell c in s.AllCells) {

                }
                List<int> diffs = new List<int>();
                for (int i = 0; i < Poss.Count; i++) {
                    diffs.Add(remaining[Poss[i]] /s.Map[Poss[i]]);
                }
                s.Weigth -= ScoreDiff(diffs);
            }
            sc.Sort((q,p) => p.Weigth.CompareTo(q.Weigth));
            
            

        }
        int ScoreDiff(List<int>diff) {
            double distance = 0;
            double avg = diff.Average();
            foreach(int i in diff) {
                distance += Math.Abs(avg - i);
            }

            return (int) Math.Ceiling(distance);
        }

        void Optimize(List<Slice> remain, HashSet<Slice> storer, Dictionary<char, int> remainingPiece) {

            bool FirstLoop = (storer.Count == 0);
            //TODO end loop when no more legal slices and wastewill be worse than best waste so far
            foreach (Slice first in remain) {
               
                List<Slice> remains = first.Dontverlap(remain);
                HashSet<Slice> store = new HashSet<Slice>();
                foreach(Slice s in storer) {
                    store.Add(s);
                }
                store.Add(first);
                Dictionary<char, int> rPieces = RemoveFrom(remainingPiece, first.Map);

                if (ShouldContinue(rPieces)) {
                    if (remains.Count > 0) {
                        Optimize(remains, store,rPieces);
                        if (FoundBest)
                            return;
                    } else {
                        int sleft = Left(store);
                        if (sleft < best) {
                            best = sleft;
                            bestPiece = store;
                            //Stop where someone is 0
                            if (best == LeastWaste) {
                                FoundBest = true;
                                Console.WriteLine("Optimizer finished after" + sw.ElapsedMilliseconds);
                                return;
                            }
                        }
                    }
                    if (FirstLoop) {
                        foreach (Cell c in first.AllCells) {
                            Cell[c].Remove(first);
                            if (Cell[c].Count == 0) {
                                Cell.Remove(c);
                            }
                        }
                        LeastWaste = size - Cell.Count();
                        if (best == LeastWaste) {
                            // Console.WriteLine("Optimizer finished in" + sw.ElapsedMilliseconds);
                            FoundBest = true;
                            Console.WriteLine("Optimizer finished after" + sw.ElapsedMilliseconds);
                            return;
                        }
                    }
                }

            }

        }
        Dictionary<char, int> RemoveFrom(Dictionary<char, int>  All, Dictionary<char, int> sub) {
            Dictionary<char, int> ans = new Dictionary<char, int>();
            foreach(char c in All.Keys) {
                ans.Add(c, (All[c] - sub[c]));
            }
            return ans;
        }
        bool ShouldContinue(Dictionary<char,int> Left) {
            if (Left.Count == 0)
                return false;
            char min=' ';
            int minscore=-1;
            int totalLeft = 0;
            foreach (char c in Left.Keys) {
                totalLeft += Left[c];
                if (minscore == -1) {
                    minscore = Left[c];
                    min = c;
                } else if (Left[c] < minscore) {
                    min = c;
                    minscore = Left[c];
                }
            }
            
            int noOfSlice = (Left[min] / MinEach);
            int MaxUsage = noOfSlice * Max;
            int MinWaste = totalLeft - MaxUsage;
            if (Left[min] < MinEach) {
                MinWaste = totalLeft;
            }
            if (MinWaste >= best) {
                return false;
            }
            return true;
        }
        public List<Slice> Result() {
            if (size > 0) {
                SortSlices(AllSlices, Total);
                Optimize(AllSlices, new HashSet<Slice>(),Total);
                Console.WriteLine("Optimizer finished after" + sw.ElapsedMilliseconds);
            }
            return bestPiece.ToList();
        }
        int Left(HashSet<Slice> all) {
            int tot = 0;
            foreach(Slice a in all) {
                tot += a.size;
            }
            return size-tot;
        }
    }
}
