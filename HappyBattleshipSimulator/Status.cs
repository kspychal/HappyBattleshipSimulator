namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public enum Status
        {
            None = '.',
            Carrier = 'a',
            Battleship = 'b',
            Cruiser = 'c',
            Submarine = 'd',
            Destroyer = 'e',
            Hitted = 'x',
            Missed = 'm',
            Sunk = 's'
        }
    }
}

