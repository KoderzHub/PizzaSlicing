using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Pizza {
    public class PizzaC {
        List<List<char>> _pizza;
        int Max;
        int MinEach;
        int size;
        List<char> Poss;
        int LeastWaste;
        List<Cell> AllCells;

        int best;
        public List<Slice> bestPiece;
        bool FoundBest = false;


        Dictionary<char, int> Total;

        Stopwatch sw = new Stopwatch();
        public PizzaC(List<List<char>> Pizza, int Max, int MinEach) {
            this._pizza = Pizza;
            this.Max = Max;
            this.MinEach = MinEach;

            HashSet<char> poss = new HashSet<char>();
            Total = new Dictionary<char, int>();
            foreach (List<char> c in Pizza) {
                foreach (char i in c) {
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
                bestPiece = new List<Slice>();
                AllCells = new List<Cell>();
                for (int r1 = 0; r1 < _pizza.Count; r1++) {
                    for (int c1 = 0; c1 < _pizza[r1].Count; c1++) {
                        AllCells.Add(new Cell(r1, c1));
                    }
                }
                LeastWaste = size - AllCells.Count();
                ls = new List<Slice>();
                left = new List<Cell>(AllCells);
                Thread t = new Thread(Generator, 30000000);
                 // Generator();
                t.Start();
                t.Join();
            } else {
                size = 0;
            }
        }
        List<Slice> ls;
        int inc = 0;
        List<Cell> left;
        void Generator() {
           
            for(int dse =0;dse<left.Count;dse++) {
                Cell c = left[dse];
                for (int i = Max; i > MinEach*Poss.Count; i--) {
                    List<int> got = GetSlices(i, c.x, c.y);
                    for (int j= 0;j < got.Count; j +=2){
                        int nx = got[j];
                        int  ny = got[j + 1];
                        
                        if (!CheckSquare(c.x, c.y, nx, ny))
                            continue;
                        Slice sc = new Slice(c.x, c.y,nx ,ny);
                        bool Overlaps = false;
                        foreach(Slice ss in ls) {
                            if (sc.Overlap(ss)) {
                                Overlaps = true;
                                break;
                            }
                        }
                        if (Overlaps) {
                            continue;
                        }
                        ls.Add(sc);
                      //  List<Slice> ls2 = new List<Slice>(ls) {
                      //      sc
                      //  };
                        RemoveLeft(left, c.x, c.y, nx, ny);
                        //Stopping condition
                        if (left.Count == LeastWaste) {
                            bestPiece = ls;
                            FoundBest = true;
                            return;
                        }
                        try {
                            inc++;
                            Generator();
                            inc--;
                        } catch (StackOverflowException){
                            inc++;
                        }
                        if (FoundBest)
                            return;

                        if (best > left.Count) {
                              
                            best = left.Count;
                            bestPiece = ls;
                            Scramble(left, ls);
                        }
                        AddLeft(left, c.x, c.y, nx, ny);
                        ls.Remove(sc);
                    }
                }
            }
        }
       
       List<int> GetSlices(int area, int x, int y) {

            //Get Factors
            var facs = new List<int>();
            int nx, ny;
            
            for (int i = 1; i <= Math.Sqrt(area); i++) {
                if (area % i == 0) {
                    nx = (i - 1 + x);
                    ny = (area / i - 1 + y);
                
                    if (nx < _pizza.Count) {
                        if (ny <_pizza[nx].Count) {
                            facs.Add(nx);
                            facs.Add(ny);
                        }
                    }


                    nx = (area / i - 1 + x);
                    ny = (i - 1 + y);

                    if (nx < _pizza.Count) {
                        if (ny < _pizza[nx].Count) {
                            facs.Add(nx);
                            facs.Add(ny);
                        }
                    }
                }
            }
            return facs;
        }
        void RemoveLeft(List<Cell> cells, int r1, int c1, int r2, int c2) {
            
            for (int i = r1; i <= r2; i++) {
                for (int j = c1; j <= c2; j++) {
                    cells.Remove(new Cell(i, j));
                }
            }

        }
        void AddLeft(List<Cell> cells, int r1, int c1, int r2, int c2) {
            
            for (int i = r1; i <= r2; i++) {
                for (int j = c1; j <= c2; j++) {
                    cells.Add(new Cell(i, j));
                }
            }

        }
        bool CheckSquare(int r1, int c1, int r2, int c2) {
            Dictionary<char, bool> bl = new Dictionary<char, bool>();
            foreach (char c in Poss) {
                bl.Add(c, false);
            }
            if (((r2 - r1 + 1) * (c2 - c1 + 1)) > Max) {
                return false;
            }
            for (int i = r1; i <= r2; i++) {
                for (int j = c1; j <= c2; j++) {
                    bl[_pizza[i][j]] = true;
                }
            }

            return IsAllTrue(bl);
        }
        bool IsAllTrue(Dictionary<char, bool> all) {
            foreach (bool a in all.Values) {
                if (a == false) {
                    return false;
                }
            }
            return true; 
        }




        ///Scramble sht
        void Scramble(List<Cell> left, List<Slice> taken) {
            char fw = Lower(left);


        }
        char Lower(List<Cell> left) {
            //TODO Gapping
            Dictionary<char, int> Count = new Dictionary<char, int>();
            char fewest = ' ';
            int fc = left.Count;
            foreach (Cell c in left){
               if (Count.ContainsKey(_pizza[c.x][c.y])) {
                        Count[_pizza[c.x][c.y]]++;
                    } else {
                        Count.Add(_pizza[c.x][c.y], 1);
                    }
                
            }
            foreach(char c in Count.Keys)
            if (fewest == ' ') {
                    fewest = c;
                    fc = Count[c];
                } else {
                    if (Count[c] < fc) {
                        fewest = c;
                        fc = Count[c];
                    }
                }
            return fewest;
        }
    }
    }
