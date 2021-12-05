namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Submarine : Ship
        {
            public Submarine() : base()
            {
                Size = 3;
                Status = Status.Submarine;
            }
        }
    }
}

