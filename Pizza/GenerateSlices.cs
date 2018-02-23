using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza {
    public class Pizza {
        List<List<char>> _pizza;
        int Max;
        int MinEach;
        int size;
        List<char> Poss;
        public Pizza(List<List<char>> Pizza, int Max, int MinEach) {
            this._pizza = Pizza;
            this.Max = Max;
            this.MinEach = MinEach;

            HashSet<char> poss = new HashSet<char>();

            foreach(List<char> c in Pizza) {
                foreach(char i in c) {
                    poss.Add(i);
                }
            }
            this.Poss = poss.ToList();
            if (_pizza.Count > 0) {
                size = (_pizza.Count * _pizza[0].Count);
            } else {
                size = 0;
            }
            best = int.MaxValue;
            bestPiece = new List<Slice>();
        }
        List<Slice> Generator() {
            List<Slice> sc = new List<Slice>();
            for(int r1 = 0; r1 < _pizza.Count; r1++) {
                for(int c1 = 0; c1 < _pizza[r1].Count; c1++) {
                    for (int r2 = r1; r2 < _pizza.Count; r2++) {
                        for (int c2 = c1; c2 < _pizza[r2].Count; c2++) {
                            if (CheckSquare(r1, c1, r2, c2)) {
                                sc.Add(new Slice(r1, c1, r2, c2));
                            }


                        }
                    }


                }
            }
            return sc;

        }
        bool CheckSquare(int r1, int c1, int r2, int c2) {
            Dictionary<char, bool> bl = new Dictionary<char, bool>();
            foreach(char c in Poss) {
                bl.Add(c, false);
            }
            if (((r2 - r1 + 1) * (c2 - c1 + 1)) > Max) {
                return false;
            }
            for(int i = r1; i <= r2; i++) {
                for(int j = c1; j <= c2; j++) {
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

        int best;
        List<Slice> bestPiece;

        void Optimize(List<Slice> remain, List<Slice> storer) {
            foreach(Slice first in remain) {
                List<Slice> remains = first.Dontverlap(remain);
                List<Slice> store = new List<Slice>();
                foreach(Slice s in storer) {
                    store.Add(s);
                }
                store.Add(first);
                if (remains.Count > 0){
                    Optimize(remains, store);
                } else {
                    int sleft = Left(store);
                    if (sleft < best) {
                        best = sleft;
                        bestPiece = store;
                        //Stop where someone is 0
                        if (best == 0) {
                            return;
                        }
                    }
                }
                   
                
            }
        }
        public List<Slice> Result() {
            if (size > 0) {
                Optimize(Generator(), new List<Slice>());
            }
            return bestPiece;
        }
        int Left(List<Slice> all) {
            int tot = 0;
            foreach(Slice a in all) {
                tot += a.size;
            }
            return size-tot;
        }
    }
}
