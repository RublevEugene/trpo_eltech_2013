using System.Collections.Generic;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
using MultiThreadingTest.Interfaces;

namespace MultiThreadingTest.ThreadingMethods
{
    public class FairLock : ILock
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

        /// <summary>
        /// The object list-tokens belonging to a thread waiting for lock.
        /// </summary>
        private readonly IList<bool?> _threadsQueue;

        public FairLock(bool reentrant)
        {
            _currentThread = null;
            _state = 0;
            _isReentrant = reentrant;
            _threadsQueue = new List<bool?>();
        }

        /// <summary>
        /// Make operation in the thread.
        /// </summary>
        public bool IsLocked
		{
			get
			{
				return _threadsQueue.Count > 0 || _currentThread != null && (_isReentrant == false || _state > 0);
			}
		}

        /// <summary>
        /// Lock curent thread
        /// </summary>
        public void Lock()
        {
            var isNotified = new bool?(false);

            lock (this)
            {
                _threadsQueue.Add(isNotified);
            }

            while (true)
            {
                lock (this)
                {
                    var isLocked = _currentThread != null && (_isReentrant == false || _state > 0) || _threadsQueue.Count > 0 && _threadsQueue[0] != isNotified;

                    if (isLocked == false)
                    {
                        _threadsQueue.Remove(isNotified);

                        _currentThread = Thread.CurrentThread;

                        if (_isReentrant)
                        {
                            ++_state;
                        }

                        return;
                    }
                }

                try
                {
                    while (isNotified == false)
                    {
                        Monitor.Wait(this); // equivalent of java's wait
                    }
                    isNotified = false;
                }
                catch (Exception)
                {
                    lock (this)
                    {
                        _threadsQueue.Remove(isNotified);
                    }
                }
            }
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

            if (_threadsQueue.Count == 0)
                return;

            _threadsQueue[0] = true;

            Monitor.Pulse(this); // equivalent of java's notify
        }
    }
}