using System.Collections.Generic;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class OwnGameboard : Gameboard
        {
            public List<Ship> Ships { get; }

            public OwnGameboard() : base()
            {
                Ships = new List<Ship>();
                Ships.Add(new Carrier());
                Ships.Add(new Battleship());
                Ships.Add(new Cruiser());
                Ships.Add(new Submarine());
                Ships.Add(new Destroyer());
                foreach (Ship ship in Ships) randomlyPlaceShip(ship);
            }

            public bool ShipsAreSunk()
            {
                foreach (Ship ship in Ships)
                {
                    if (ship.Status != Status.Sunk) return false;
                }
                return true;
            }

            private void randomlyPlaceShip(Ship ship)
            {
                int randomX, randomY;
                bool isVertical;
                do
                {
                    bool flag = false;
                    isVertical = random.Next(2) == 1;
                    randomX = random.Next(0, GameboardSize);
                    randomY = random.Next(0, GameboardSize);
                    if (isVertical && randomY + ship.Size > GameboardSize) continue;
                    if (!isVertical && randomX + ship.Size > GameboardSize) continue;
                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (isVertical)
                        {
                            if (GetPositionStatus(randomX, randomY + i) != Status.None)
                            {
                                flag = true;
                                break;
                            }
                        }
                        else
                        {
                            if (GetPositionStatus(randomX + i, randomY) != Status.None)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) continue;
                    break;
                } while (true);

                for (int i = 0; i < ship.Size; i++)
                {
                    if (isVertical)
                    {
                        SetPositionStatus(randomX, randomY + i, ship.Status);
                    }
                    else
                    {
                        SetPositionStatus(randomX + i, randomY, ship.Status);
                    }
                }
            }
        }
    }
}

