using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monad;

namespace EitherWithObjectMonand
{
    class Simple
    {
        public static int Count { get; private set; }
        public int GetIt() {
            Count += 1;
            return 5;
        }
    }

    class Program
    {
        public static Either<Simple, string> Two()
        {
            return () => new Simple();
        }

        static void Main(string[] args)
        {
            var r = from obj in Two()
                    select obj.GetIt();

            Console.WriteLine(r.IsRight());
            if (r.IsRight())
            {
                Console.WriteLine(r.Right());
            }
            Console.WriteLine("It was called {0} times", Simple.Count);

            // And a second time, with a more regular functional syntax??

            var r1 = Two().Select(s => s.GetIt());
        }
    }
}
