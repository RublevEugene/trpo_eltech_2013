using System;
using System.Threading;
using MultiThreadingTest.Helpers;
using MultiThreadingTest.ThreadingMethods;

namespace MultiThreadingTest
{
    class Program
    {
        static void Main()
        {
            #region SimpeThreadPool
            
            using (var pool = new SimpleThreadPool(2))
            {
                var random = new Random();
                Action<int> randomizer = (index =>
                {
                    Console.WriteLine("{0}: Working on index {1}", Thread.CurrentThread.Name, index);
                    Thread.Sleep(random.Next(20, 400));
                    Console.WriteLine("{0}: Ending {1}", Thread.CurrentThread.Name, index);
                });

                for (var i = 0; i < 40; ++i)
                {
                    var i1 = i;
                    pool.QueueTask(() => randomizer(i1));
                }

                //var victor = new Enemy("Victor");
                //var julio = new Enemy("Julio");
                //var marcello = new Enemy("Marcello");
                //var ellias = new Enemy("Ellias");

                //pool.QueueTask(() => new ShootLoop(victor, julio).Run());
                //pool.QueueTask(() => new ShootLoop(julio, victor).Run());
                //pool.QueueTask(() => new ShootLoop(marcello, ellias).Run());
                //pool.QueueTask(() => new ShootLoop(ellias, marcello).Run());

                Console.ReadKey();
                return;
            }

            #endregion

            #region Fair and UnfairLock

            var peter = new Enemy("Peter");
            var maikl = new Enemy("Maikl");
            new Thread(new ShootLoop(peter, maikl).Run).Start();
            new Thread(new ShootLoop(maikl, peter).Run).Start();

            Console.ReadKey();
            return;

            #endregion
        }
    }
}
