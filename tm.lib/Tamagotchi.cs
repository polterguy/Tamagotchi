
/*
 * This is the basic state machine wrapper, that wraps all states for
 * a single Tamagotchi. It contains almost no business logic at all, but
 * only serves as a helper to keep track of all needs for a Tamagotchi.
 *
 * I have implemented ICloneable on the class, to easily make sure I never
 * return the underlying Tamagotchi from the library, but rather a copy of it,
 * to make access to the thing easily synchronised across multiple threads.
 *
 * Notice, I could have accomplished the same effect by making the class immutable,
 * but this is what I chose for simplicity reasons. Besides, I got to show off
 * some thread synchronisation skills by choosing this method.
 *
 * Besides from the above mentioned ICloneable, the class is almost a POD class.
 */
using System;

namespace tm.lib
{
    public class Tamagotchi : ICloneable
    {
        public int Hungry { get; set; }

        public int Bored { get; set; }

        public int Tired { get; set; }

        public int Full { get; set; }

        public bool IsDead { get; set; }

        public object Clone()
        {
            var clone = new Tamagotchi();
            clone.Hungry = Hungry;
            clone.Bored = Bored;
            clone.Tired = Tired;
            clone.Full = Full;
            clone.IsDead = IsDead;
            return clone;
        }
    }
}
