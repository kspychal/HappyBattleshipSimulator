using System;
using System.Collections.Generic;
using System.Linq;

namespace HappyBattleshipSimulator
{
    internal partial class Program
    {
        public class Fireboard : Gameboard
        {
            private List<Position> positionsToCheck;
            public Fireboard() : base()
            {
                positionsToCheck = new List<Position>();
            }

            private Position getPositionFromList(int x, int y)
            {
                return Positions.ElementAt(x + y * GameboardSize);
            }

            public void Shoot(OwnGameboard opponentOwnGameboard)
            {
                if (positionsToCheck.Count() > 0) shootNeighbours(opponentOwnGameboard);
                else shootRandom(opponentOwnGameboard);
            }

            private void shootNeighbours(OwnGameboard opponentOwnGameboard)
            {
                int randomFromPostionsToCheck = random.Next(0, positionsToCheck.Count());
                int _x = positionsToCheck.ElementAt(randomFromPostionsToCheck).X;
                int _y = positionsToCheck.ElementAt(randomFromPostionsToCheck).Y;

                Console.Write($"shooting ({_x},{_y})... ");
                if (opponentOwnGameboard.PositionIsShip(_x, _y))
                {
                    if (_x + 1 < 10 && GetPositionStatus(_x + 1, _y) == Status.None && positionsToCheck.Find(p => p.X == _x + 1 && p.Y == _y) == null)
                        positionsToCheck.Add(new Position(_x + 1, _y));
                    if (_x - 1 > -1 && GetPositionStatus(_x - 1, _y) == Status.None && positionsToCheck.Find(p => p.X == _x - 1 && p.Y == _y) == null)
                        positionsToCheck.Add(new Position(_x - 1, _y));
                    if (_y + 1 < 10 && GetPositionStatus(_x, _y + 1) == Status.None && positionsToCheck.Find(p => p.X == _x && p.Y == _y + 1) == null)
                        positionsToCheck.Add(new Position(_x, _y + 1));
                    if (_y - 1 > -1 && GetPositionStatus(_x, _y - 1) == Status.None && positionsToCheck.Find(p => p.X == _x && p.Y == _y - 1) == null)
                        positionsToCheck.Add(new Position(_x, _y - 1));

                    getPositionFromList(_x, _y).Hit();
                    opponentOwnGameboard.Ships.Find(s => s.Status == opponentOwnGameboard.GetPositionStatus(_x, _y)).Hit();
                    opponentOwnGameboard.SetPositionStatus(_x, _y, Status.Hitted);

                }
                else getPositionFromList(_x, _y).SetStatusMissed();

                positionsToCheck.RemoveAt(randomFromPostionsToCheck);
            }


            private void shootRandom(OwnGameboard opponentGameboard)
            {
                int randomX, randomY;
                do
                {
                    randomX = random.Next(0, GameboardSize);
                    randomY = random.Next(0, GameboardSize);
                } while (GetPositionStatus(randomX, randomY) != Status.None);

                Console.Write($"shooting ({randomX},{randomY})... ");
                if (opponentGameboard.PositionIsShip(randomX, randomY))
                {
                    if (randomX + 1 < 10 && GetPositionStatus(randomX + 1, randomY) == Status.None && positionsToCheck.Find(p => p.X == randomX + 1 && p.Y == randomY) == null)
                        positionsToCheck.Add(new Position(randomX + 1, randomY));
                    if (randomX - 1 > -1 && GetPositionStatus(randomX - 1, randomY) == Status.None && positionsToCheck.Find(p => p.X == randomX - 1 && p.Y == randomY) == null)
                        positionsToCheck.Add(new Position(randomX - 1, randomY));
                    if (randomY + 1 < 10 && GetPositionStatus(randomX, randomY + 1) == Status.None && positionsToCheck.Find(p => p.X == randomX && p.Y == randomY + 1) == null)
                        positionsToCheck.Add(new Position(randomX, randomY + 1));
                    if (randomY - 1 > -1 && GetPositionStatus(randomX, randomY - 1) == Status.None && positionsToCheck.Find(p => p.X == randomX && p.Y == randomY - 1) == null)
                        positionsToCheck.Add(new Position(randomX, randomY - 1));

                    getPositionFromList(randomX, randomY).Hit();
                    opponentGameboard.Ships.Find(ship => ship.Status == opponentGameboard.GetPositionStatus(randomX, randomY)).Hit();
                    opponentGameboard.SetPositionStatus(randomX, randomY, Status.Hitted);
                }
                else
                {
                    getPositionFromList(randomX, randomY).SetStatusMissed();
                }
            }
        }
    }
}

