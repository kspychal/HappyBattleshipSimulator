namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Carrier : Ship
        {
            public Carrier() : base()
            {
                Size = 5;
                Status = Status.Carrier;
            }
        }
    }
}

