using System;
using MultiThreadingTest.Interfaces;
using MultiThreadingTest.ThreadingMethods;

namespace MultiThreadingTest.Helpers
{
    public class Enemy
    {
        private readonly string _name;

        //private readonly ILock _lock = new UnfairLock(false);
        //private readonly ILock _lock = new UnfairLock(true); // with reentrant
        //private readonly ILock _lock = new FairLock(false); 
        private readonly ILock _lock = new FairLock(true); // with reentrant

        public Enemy(string name)
        {
            this._name = name;
        }

        private string Name
        {
            get
            {
                return this._name;
            }
        }

        private bool ImpendingShoot(Enemy bower)
        {
            bool? myLock = false;
            bool? yourLock = false;

            try
            {
                myLock = _lock.TryLock();
                yourLock = bower._lock.TryLock();
            }
            finally
            {
                if (myLock == false || yourLock == false)
                {
                    if (myLock == true)
                    {
                        _lock.Unlock();
                    }
                    if (yourLock == true)
                    {
                        bower._lock.Unlock();
                    }
                }
            }

            return (bool)myLock && (bool)yourLock;
        }

        public virtual void Shoot(Enemy bower)
		{
			if (ImpendingShoot(bower))
			{
				try
				{
					Console.WriteLine("{0} shoot {1} !", this._name, bower.Name);
					bower.BowBack(this);
				}
				finally
				{
					_lock.Unlock();
                    bower._lock.Unlock();
				}
			}
			else
			{
				Console.WriteLine("{0} shoot {1}, but {1} shot earlier", this._name, bower.Name);
			}
		}

        private void BowBack(Enemy bower)
		{
			Console.WriteLine("{0} shoot {1} after {1} shot!", this._name, bower.Name);
		}
    }
}