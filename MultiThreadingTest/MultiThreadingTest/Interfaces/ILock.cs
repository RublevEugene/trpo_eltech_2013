namespace MultiThreadingTest.Interfaces
{
    public interface ILock
    {
        /// <summary>
        /// Lock curent thread
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlock curent thread
        /// </summary>
        void Unlock();

        /// <summary>
        /// Try to lock thread if it is not locked yet.
        /// </summary>
        bool TryLock();

        /// <summary>
        /// Is thread locked.
        /// </summary>
        bool IsLocked { get; }
    }
}