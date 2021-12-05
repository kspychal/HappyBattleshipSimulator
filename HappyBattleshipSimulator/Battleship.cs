namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Battleship : Ship
        {
            public Battleship() : base()
            {
                Size = 4;
                Status = Status.Battleship;
            }
        }
    }
}

