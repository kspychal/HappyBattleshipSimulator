namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Cruiser : Ship
        {
            public Cruiser() : base()
            {
                Size = 3;
                Status = Status.Cruiser;
            }
        }
    }
}

