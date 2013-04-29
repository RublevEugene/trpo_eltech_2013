using System;
using System.Threading;
using MultiThreadingTest.Interfaces;

namespace MultiThreadingTest.Helpers
{
    public class ShootLoop : Runnable
    {
        private const int MaxIterations = 100;
        private readonly Enemy _bower;
        private readonly Enemy _bowee;

        public ShootLoop(Enemy bower, Enemy bowee)
        {
            this._bower = bower;
            this._bowee = bowee;
        }

        public void Run()
        {
            var random = new Random();

            for (var i = 0; i < MaxIterations; ++i)
            {
                try
                {
                    Thread.Sleep(random.Next(10));
                }
                catch (Exception ex)
                {
                }

                _bowee.Shoot(_bower);
            }
        }
    }
}