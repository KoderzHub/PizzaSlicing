using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza {
    public class Slice {
        public readonly int r1, r2, c1, c2;
        public readonly int size;
        /// <summary>
        /// R2 must be greater than r1
        /// c2 must be greater than c1
        /// Initializes a new instance of the <see cref="Slice"/> class.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="c1">The c1.</param>
        /// <param name="r2">The r2.</param>
        /// <param name="c2">The c2.</param>
        public Slice(int r1, int c1, int r2, int c2) {

            this.r1 = r1;
            this.r2 = r2;
            this.c1 = c1;
            this.c2 = c2;
            size = ((r2 - r1 + 1) * (c2 - c1 + 1));
        }
        bool Overlap(Slice a) {
            if (r1 > a.r2) {
                return false;
            } else if (r2 < a.r1) {
                return false;
            } else {
                if (c1 > a.c2) {
                    return false;
                } else if (c2 < a.c1) {
                    return false;
                } else {
                    return true;
                }

            }
        }
        public List<Slice> Dontverlap(List<Slice> all) {
            List<Slice> ans = new List<Slice>();
            foreach (Slice s in all) {
                if (!Overlap(s)) {
                    ans.Add(s);
                }
            }
            return ans;
        }


        public HashSet<Cell> AllCells {
            get {
                HashSet<Cell> cells = new HashSet<Cell>();
                for (var i = r1; i <= r2; i++) {
                    for (var j = c1; j <= c2; j++) {
                        cells.Add(new Cell(i, j));
                    }
                }
                return cells;
            }
        }

        public override string ToString() {
            return r1 + " " + c1 + " " + r2 + " " + c2;
        }
        public override bool Equals(object obj) {
            if (!(obj is Slice))
                return false;
            Slice n = obj as Slice;
            return (r1 == n.r1) && (r2 == n.r2) && (c1 == n.c1) && (c2 == n.c2);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

    }
    public class Cell {
        public int x, y;
        public Cell(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public override bool Equals(object obj) {
            if (!(obj is Cell))
                return false;
            Cell n = obj as Cell;
            return (x == n.x) && (y == n.y);
        }
        public override int GetHashCode() {
            return (100000 * x) + y;
        }
    }
}
