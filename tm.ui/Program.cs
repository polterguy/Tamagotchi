
/*
 * This is the UI of our Tamagotchi, since there was no explicit UI requirements,
 * I chose to implement the UI as a simple Console application, only relying
 * upon polling the Engine to retrieve state, and invoking the Engine to
 * modify state.
 */
using System;
using tm.lib;

namespace tm.ui
{
    class Program
    {
        // Main, loops until Tamagotchi dies, or is abandoned.
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Magic Tamagotchi");
            while (true)
            {
                var tamagotchi = TmEngine.Instance.State;
                if (tamagotchi.IsDead)
                    break;
                WriteState(tamagotchi);
                if (!TmEngine.Instance.Interact(GetAction()))
                {
                    WriteState(TmEngine.Instance.State);
                    break;
                }
            }
            Console.WriteLine("Ohh no, your Tamagotchi died!");
            Console.WriteLine("R.I.P. litte fella :(");
        }

        static void WriteState(Tamagotchi gotchi)
        {
            Console.WriteLine();
            Console.WriteLine("**********************************************");
            Console.WriteLine($"Hungry: {gotchi.Hungry}");
            Console.WriteLine($"Bored: {gotchi.Bored}");
            Console.WriteLine($"Tired: {gotchi.Tired}");
            Console.WriteLine($"Full: {gotchi.Full}");
            Console.WriteLine("**********************************************");
            Console.WriteLine();
        }

        static TmAction GetAction()
        {
            int no = 0;
            while (no++ < 3)
            {
                Console.WriteLine("Choose one option below");
                Console.WriteLine("1. Feed");
                Console.WriteLine("2. Play");
                Console.WriteLine("3. Put to bed");
                Console.WriteLine("4. Bring to toilet");
                Console.WriteLine("5. Check state");
                Console.WriteLine("6. Abandon (Quits program)");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        return TmAction.Feed;
                    case '2':
                        return TmAction.Play;
                    case '3':
                        return TmAction.Sleep;
                    case '4':
                        return TmAction.Poop;
                    case '5':
                        return TmAction.CheckState;
                    case '6':
                        return TmAction.Abandon;
                }
                Console.WriteLine("Sorry, I don't understand ...?");
                Console.WriteLine();
            }
            Console.WriteLine("I give up trying to understand you ...");
            return TmAction.Abandon;
        }
    }
}
