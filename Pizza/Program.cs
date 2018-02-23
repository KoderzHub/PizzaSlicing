using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza {
    class Program {
        static void Main(string[] args) {
            List<char> col = new List<char> {
                'T','T','T','T','T'
            };
            List<char> col2 = new List<char> {
                'T','M','M','M','T'
            };
            List<char> col3 = new List<char> {
                'T','T','T','T','T'
            };
            List<List<char>> piz = new List<List<char>> {
                col,col2,col3
            };
            Pizza p = new Pizza(piz, 6, 1);
            foreach (Slice s in p.Result()) {
                Console.WriteLine(s);
            }
            Console.Read();
        }
    }
}
