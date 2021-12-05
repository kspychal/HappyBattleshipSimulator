using System;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        static Random random = new Random();

        static int Main(string[] args)
        {
            Game game;
            string answer = "y";
            while (answer != "n")
            {
                Console.WriteLine("Start new game? (y/n)");
                answer = Console.ReadLine();
                if (answer == "y")
                {
                    game = new Game();
                    game.Start();
                }
            }
            return 0;
        }
    }
}

