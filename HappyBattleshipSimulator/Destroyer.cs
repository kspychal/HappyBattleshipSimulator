namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Destroyer : Ship
        {
            public Destroyer() : base()
            {
                Size = 2;
                Status = Status.Destroyer;
            }
        }
    }
}

