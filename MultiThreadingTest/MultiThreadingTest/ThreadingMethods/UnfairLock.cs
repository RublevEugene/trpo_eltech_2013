using System.Runtime.CompilerServices;
using System.Threading;
using System;
using MultiThreadingTest.Interfaces;

namespace MultiThreadingTest.ThreadingMethods
{
    public class UnfairLock : ILock
    {
        /// <summary>
        /// Thread catched by Lock
        /// </summary>
        private Thread _currentThread;

        /// <summary>
        /// Cathced count for Lock.
        /// </summary>
        private int _state;

        /// <summary>
        /// Is rentrant use.
        /// </summary>
        private readonly bool _isReentrant;

        public UnfairLock(bool reentrant)
        {
            _currentThread = null;
            _state = 0;
            _isReentrant = reentrant;
        }

        /// <summary>
        /// Is thread locked.
        /// </summary>
        public bool IsLocked
		{
			get
			{
				return _currentThread != null && (_isReentrant == false || _state > 0);
			}
		}

        /// <summary>
        /// Lock curent thread
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Lock()
        {
            while (IsLocked)
            {
                try
                {
                    Monitor.Wait(this); // equivalent of java's wait
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            _currentThread = Thread.CurrentThread;

            if (_isReentrant)
            {
                ++_state;
            }
        }

        /// <summary>
        /// Unlock curent thread
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Unlock()
        {
            if (_currentThread != Thread.CurrentThread)
                return;

            if (_isReentrant && --_state > 0)
                return;

            _currentThread = null;

            Monitor.Pulse(this); // equivalent of java's notify
        }

        /// <summary>
        /// Try to lock thread if it is not locked yet.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryLock()
        {
            if (IsLocked)
                return false;

            Lock();
            return true;
        }
    }
}