using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Gameboard
        {
            protected const int GameboardSize = 10;
            public List<Position> Positions { get; }

            protected Gameboard()
            {
                Positions = new List<Position>();
                for (int i = 0; i < GameboardSize; i++)
                {
                    for (int j = 0; j < GameboardSize; j++)
                    {
                        Positions.Add(new Position(i, j));
                    }
                }
            }

            public Status GetPositionStatus(int x, int y)
            {
                return Positions.ElementAt(x + y * GameboardSize).Status;
            }

            public void SetPositionStatus(int x, int y, Status s)
            {
                Positions.ElementAt(x + y * GameboardSize).Status = s;
            }

            public bool PositionIsShip(int x, int y)
            {
                return GetPositionStatus(x, y) == Status.Battleship || GetPositionStatus(x, y) == Status.Carrier ||
                    GetPositionStatus(x, y) == Status.Cruiser || GetPositionStatus(x, y) == Status.Destroyer ||
                    GetPositionStatus(x, y) == Status.Submarine;
            }

            public void Print()
            {
                for (int i = 0; i < GameboardSize; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < GameboardSize; j++)
                    {
                        Console.Write((char)GetPositionStatus(i, j) + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

