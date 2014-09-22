using Monad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EitherMonad
{
    class Program
    {
        public static Either<int, string> Two()
        {
            return () => 2;
        }

        public static Either<int, string> Error()
        {
            return () => "Error!!";
        }
        
        /// <summary>
        /// Test out the Either monad
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var r =
                from lhs in Two()
                from rhs in Two()
                select lhs + rhs;

            Console.WriteLine("The r is {0}", r.IsRight());
            if (r.IsRight())
            {
                Console.WriteLine("The value is {0}", r.Right());
            }
        }
    }
}
