
/*
 * This is the engine for our Tamagotchi. For simplicity reasons it's
 * implemented as a Singleton.
 *
 * This is where the verbs for our Tamagotchi is implement, and internally
 * the class keeps track of a single Tamagotchi, and as we interact with it,
 * or time passes, the state of this single Tamagotchi is mutated.
 *
 * The class is thread safe, which is necessary, due to that we have a Timer,
 * running on a different thread, that modifies the state of the Tamagotchi
 * this class wraps.
 */

using System;
using System.Threading;

namespace tm.lib
{
    public sealed class TmEngine
    {
        // Making sure access to Singleton instance is thread safe
        static Lazy<TmEngine> _instance = new Lazy<TmEngine>(() => new TmEngine());

        // Constant values defining timer ticks, increaments of parameters during ticks, etc.
        const int MILLISECONDS_BETWEEN_TICKS = 5000;
        const int BORED_INCR = 5;
        const int TIRED_INCR = 3;
        const int HUNGRY_INCR = 1;

        // Making sure we have synchronised access to the Tamagotchi instance.
        ReaderWriterLockSlim _locker;

        // The Tamagotchi's actual state.
        Tamagotchi _tamagotchi;

        // Timer callback.
        Timer _timer;

        // Private constructor, which prevents more than a single instance of type.
        private TmEngine()
        {
            _tamagotchi = new Tamagotchi();
            _locker = new ReaderWriterLockSlim();
            _timer = new Timer((f) => {
                _locker.EnterWriteLock();
                try
                {
                    _tamagotchi.Bored = Math.Min(100, _tamagotchi.Bored + BORED_INCR);
                    _tamagotchi.Tired = Math.Min(100, _tamagotchi.Tired + TIRED_INCR);
                    _tamagotchi.Hungry = Math.Min(100, _tamagotchi.Hungry + HUNGRY_INCR);
                    if (StillAlive())
                    {
                        // Resetting timer.
                        _timer.Change(MILLISECONDS_BETWEEN_TICKS, Timeout.Infinite);
                    }
                }
                finally
                {
                    _locker.ExitWriteLock();
                }
            });
            _timer.Change(MILLISECONDS_BETWEEN_TICKS, Timeout.Infinite);
        }

        // Singleton getter. Notice, we're using Lazy, which is thread safe by default.
        public static TmEngine Instance { get { return _instance.Value; } }

        // Retrieves the Tamagotchi's state back to caller.
        // Notice, "Clone" (ensures synchronised access to underlying instance).
        // And prevents direct tampering with state, except through "Interact" method.
        public Tamagotchi State
        {
            get
            {
                // Synchronising access to shared instance.
                _locker.EnterReadLock();
                try
                {
                    return _tamagotchi.Clone() as Tamagotchi;
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }
        }

        // Interact method, completes a "verb" towards your Tamagotchi.
        public bool Interact(TmAction action)
        {
            _locker.EnterWriteLock();
            try
            {
                if (!StillAlive())
                    return false; // No further interation possible.
                switch(action)
                {
                    case TmAction.Abandon:
                        _tamagotchi.IsDead = true;
                        return false;
                    case TmAction.Feed:
                        _tamagotchi.Hungry = Math.Max(0, _tamagotchi.Hungry - 17);
                        _tamagotchi.Full = Math.Min(100, _tamagotchi.Full + 8);
                        break;
                    case TmAction.Play:
                        _tamagotchi.Bored = Math.Max(0, _tamagotchi.Bored - 23);
                        _tamagotchi.Tired = Math.Min(100, _tamagotchi.Tired + 11);
                        break;
                    case TmAction.Sleep:
                        _tamagotchi.Tired = 0;
                        break;
                    case TmAction.Poop:
                        _tamagotchi.Full = 0;
                        break;
                }
                return StillAlive();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        #region [ -- Private helper methods -- ]

        bool StillAlive()
        {
            if (_tamagotchi.IsDead)
                return false;
            if (_tamagotchi.Bored >= 100 ||
                _tamagotchi.Full >= 100 ||
                _tamagotchi.Hungry >= 100 ||
                _tamagotchi.Tired >= 100)
            {
                _tamagotchi.IsDead = true;
                return false;
            }
            return true;
        }

        #endregion
    }
}
