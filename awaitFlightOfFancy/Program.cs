using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awaitFlightOfFancy
{
    class Future<T> {
        public Future(Func<T> getter)
        {
            // Wait until the last minute before we run it.
            _generator = getter;
            _evaluated = false;
        }

        private Func<T> _generator;
        private bool _evaluated;
        T _value;

        public T Value
        {
            get
            {
                if (_evaluated)
                {
                    return _value;
                }
                _evaluated = true;
                _value = _generator();
                return _value;
            }
        }
    }

class Program
    {
        class UtilityFunctions
        {
            public static int ExpensiveCalculation()
            {
                Console.WriteLine("We are doing an expensive calc!");
                return 5;
            }

            public static Future<int> MakeFuture()
            {
                return new Future<int>(() => ExpensiveCalculation());
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Create the futures...");
            var v1 = UtilityFunctions.MakeFuture();
            var v2 = UtilityFunctions.MakeFuture();

            var a2 = await v1 + await v2;
        }
    }
}
