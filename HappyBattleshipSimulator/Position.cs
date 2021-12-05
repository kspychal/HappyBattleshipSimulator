using System;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Position
        {
            public int X { get; }
            public int Y { get; }
            public Status Status { get; set; }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
                Status = Status.None;
            }

            public void Hit()
            {
                Console.Write("hit");
                Status = Status.Hitted;
            }

            public void SetStatusMissed()
            {
                Console.Write("miss");
                Status = Status.Missed;
            }
        }
    }
}

