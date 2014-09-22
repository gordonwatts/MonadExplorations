using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureMonadTest
{
    public class Future<T>
    {
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

    public static class FutureLINQExtensions
    {
        public static Future<U> Select<U, T>(this Future<T> source, Func<T, U> converter)
        {
            return new Future<U>(() => converter(source.Value));
        }

        /// source, collection, result
        public static Future<VR> SelectMany<TR, UR, VR>(
            this Future<TR> self,
            Func<TR, Future<UR>> selector,
            Func<TR, UR, VR> projector
            )
        {
            return new Future<VR>(() =>
            {
                var resU = selector(self.Value);
                return projector(self.Value, resU.Value);
            });
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

        static void Main(string[] args)
        {
            Console.WriteLine("Create the futures...");
            var v1 = UtilityFunctions.MakeFuture();
            var v2 = UtilityFunctions.MakeFuture();

            Console.WriteLine("Doing first calc");
            //var r1 = v1.Value + v2.Value;
            var r1 = from i1 in v1
                     from i2 in v2
                     select (i1 + i2);

            Console.WriteLine("Doing second calc");
            //var r2 = v1.Value * 2 + v2.Value * 2;
            var r2 = from i1 in v1
                     from i2 in v2
                     select i1 * 2 + i2 * 2;

            Console.WriteLine("Values:");
            //Console.WriteLine("r1 = {0}... r2 = {1}", r1, r2);
            Console.WriteLine("r1 = {0}... r2 = {1}", r1.Value, r2.Value);
        }
    }
}
