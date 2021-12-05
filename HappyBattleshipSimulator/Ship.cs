using System;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Ship
        {
            public int Size { protected set; get; }
            protected int Hits = 0;
            public Status Status { protected set; get; }

            public void Hit()
            {
                Hits++;
                if (Hits >= Size)
                {
                    Console.Write($" and sink - {Status}");
                    this.Status = Status.Sunk;
                }
            }
        }
    }
}

