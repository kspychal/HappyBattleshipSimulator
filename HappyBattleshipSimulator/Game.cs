using System;
using System.Threading;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Game
        {
            private Player p1, p2;
            public Game()
            {
                p1 = new Player();
                p2 = new Player();
            }

            public void Start()
            {
                while (!p1.OwnGameboard.ShipsAreSunk() && !p2.OwnGameboard.ShipsAreSunk())
                {
                    Console.Clear();
                    Console.Write("P1 turn\n\nP1 Gameboard:");
                    p1.OwnGameboard.Print();
                    Console.Write("\nP1 Fireboard: ");
                    p1.Fireboard.Shoot(p2.OwnGameboard);
                    if (p2.OwnGameboard.ShipsAreSunk()) break;
                    p1.Fireboard.Print();
                    Thread.Sleep(1500);

                    Console.Clear();
                    Console.Write("P2 turn\n\nP2 Gameboard:");
                    p2.OwnGameboard.Print();
                    Console.Write("\nP2 Fireboard: ");
                    p2.Fireboard.Shoot(p1.OwnGameboard);
                    p2.Fireboard.Print();
                    Thread.Sleep(1500);
                }
                Console.Clear();
                Console.Write("-----P1-----\nGameBoard:");
                p1.OwnGameboard.Print();
                Console.Write("\nFireboard:");
                p1.Fireboard.Print();
                Console.Write("\n\n-----P2-----\nGameBoard:");
                p2.OwnGameboard.Print();
                Console.Write("\nFireboard:");
                p2.Fireboard.Print();
                if (p1.OwnGameboard.ShipsAreSunk()) Console.WriteLine("P2 won");
                if (p2.OwnGameboard.ShipsAreSunk()) Console.WriteLine("P1 won");
            }
        }
    }
}

