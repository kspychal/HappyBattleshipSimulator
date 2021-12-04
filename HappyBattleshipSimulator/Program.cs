using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HappyBattleshipSimulator
{
    internal class Program
    {
        static Random random = new Random();
        public enum Status
        {
            none = '.',
            carrier = 'a',
            battleship = 'b',
            cruiser = 'c',
            submarine = 'd',
            destroyer = 'e',
            hitted = 'x',
            missed = 'm',
            sunk = 's'
        }

        public class Position
        {
            int x, y;
            public Status status;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
                status = Status.none;
            }

            public void hit()
            {
                status = Status.hitted;
            }

            public void setStatusMissed()
            {
                this.status = Status.missed;
            }

            public int getX()
            {
                return x;
            }

            public int getY()
            {
                return y;
            }

            public Status getStatus()
            {
                return status;
            }
        }

        public class Ship
        {
            protected int hits;
            public Ship()
            {
                hits = 0;
            }

            protected int size;
            protected Status status;

            public void hit()
            {
                hits++;
                if (hits >= size)
                {
                    this.status = Status.sunk;
                    Console.WriteLine("---------------------------------------------------------------------sunk-------------------------------------------------------------------" + this.getSize() + ":" + hits);
                }
            }
            

            public int getSize() { return size; }
            public Status getStatus() { return status; }
        }

        protected class Carrier : Ship
        {
            public Carrier() : base()
            {
                size = 5;
                status = Status.carrier;
            }
        }

        protected class Battleship : Ship
        {
            public Battleship() : base()
            {
                size = 4;
                status = Status.battleship;
            }
        }

        protected class Cruiser : Ship
        {
            public Cruiser() : base()
            {
                size = 3;
                status = Status.cruiser;
            }
        }

        protected class Submarine : Ship
        {
            public Submarine() : base()
            {
                size = 3;
                status = Status.submarine;
            }
        }

        protected class Destroyer : Ship
        {
            public Destroyer() : base()
            {
                size = 2;
                status = Status.destroyer;
            }
        }

        public class Gameboard
        {
            protected const int gameboardSize = 10;
            public List<Position> positions = new List<Position>();

            protected Gameboard()
            {
                for (int i = 0; i < gameboardSize; i++)
                {
                    for (int j = 0; j < gameboardSize; j++)
                    {
                        positions.Add(new Position(i, j));
                    }
                }

            }

            public void print()
            {
                Console.WriteLine();
                for (int i = 0; i < gameboardSize; i++)
                {
                    Console.Write(" " + (char)('A' + i));
                }
                for (int i = 0; i < gameboardSize; i++)
                {
                    Console.WriteLine();
                    for (int j = 0; j < gameboardSize; j++)
                    {
                        Console.Write(" " + ((char)positions.ElementAt(gameboardSize * i + j).status));
                    }
                }
                Console.WriteLine();

            }

            protected Position getPositionFromList(int x, int y)
            {
                return positions.ElementAt(x + y * gameboardSize);
            }

            public Status getPositionStatus(int x, int y)
            {
                return positions.ElementAt(x + y * gameboardSize).getStatus();
            }

            protected void setPositionStatus(int x, int y, Status s)
            {
                positions.ElementAt(x + y * gameboardSize).status = s;
            }

            public bool positionIsShip(int x, int y)
            {
                return getPositionStatus(x, y) == Status.battleship || getPositionStatus(x, y) == Status.carrier ||
                    getPositionStatus(x, y) == Status.cruiser || getPositionStatus(x, y) == Status.destroyer ||
                    getPositionStatus(x, y) == Status.submarine;
            }
        }

        public class OwnGameboard : Gameboard
        {
            private Carrier carrier = new Carrier();
            private Battleship battleship = new Battleship();
            private Cruiser cruiser = new Cruiser();
            private Submarine submarine = new Submarine();
            private Destroyer destroyer = new Destroyer();
            public List<Ship> ships = new List<Ship>();

            public OwnGameboard() : base()
            {
                ships.Add(new Carrier());
                ships.Add(new Battleship());
                ships.Add(new Cruiser());
                ships.Add(new Submarine());
                ships.Add(new Destroyer());
                foreach (Ship ship in ships) randomlyPlaceShip(ship);
                print();
            }

            public bool shipsAreSunk()
            {
                foreach (Ship ship in ships)
                {
                    if (ship.getStatus() != Status.sunk) return false;
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
                    randomX = random.Next(0, gameboardSize);
                    randomY = random.Next(0, gameboardSize);
                    if (isVertical && randomY + ship.getSize() > gameboardSize) continue;
                    if (!isVertical && randomX + ship.getSize() > gameboardSize) continue;
                    for (int i = 0; i < ship.getSize(); i++)
                    {
                        if (isVertical)
                        {
                            if(getPositionStatus(randomX, randomY + i) != Status.none)
                            {
                                flag = true;
                                break;
                            }
                        }
                        else
                        {
                            if(getPositionStatus(randomX + i, randomY) != Status.none)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) continue;
                    break;
                } while (true);

                for (int i = 0; i < ship.getSize(); i++)
                {
                    if (isVertical)
                    {
                        setPositionStatus(randomX, randomY + i, ship.getStatus());
                    }
                    else
                    {
                        setPositionStatus(randomX + i, randomY, ship.getStatus());
                    }
                }
            }
        }

        public class Fireboard : Gameboard
        {
            List<Position> positionsToCheck;
            public Fireboard() : base()
            {
                positionsToCheck = new List<Position>();
            }

            public void shoot(OwnGameboard opponentGameboard)
            {
                if (positionsToCheck.Count() > 0) shootNeighbours(opponentGameboard);
                else shootRandom(opponentGameboard);
            }

            public void shootNeighbours(OwnGameboard opponentGameboard)
            {
                int randomFromPostionsToCheck = random.Next(0, positionsToCheck.Count());
                int _x = positionsToCheck.ElementAt(randomFromPostionsToCheck).getX();
                int _y = positionsToCheck.ElementAt(randomFromPostionsToCheck).getY();
                Console.WriteLine("Shot neighbour: x:" + _x + "   y:" + _y);

                if (opponentGameboard.positionIsShip(_x, _y))
                {
                    if (_x + 1 < 10 && getPositionStatus(_x + 1, _y) == Status.none && positionsToCheck.Find(p => p.getX() == _x + 1 && p.getY() == _y) == null)
                        positionsToCheck.Add(new Position(_x + 1, _y));
                    if (_x - 1 > -1 && getPositionStatus(_x - 1, _y) == Status.none && positionsToCheck.Find(p => p.getX() == _x - 1 && p.getY() == _y) == null)
                        positionsToCheck.Add(new Position(_x - 1, _y));
                    if (_y + 1 < 10 && getPositionStatus(_x, _y + 1) == Status.none && positionsToCheck.Find(p => p.getX() == _x && p.getY() == _y + 1) == null)
                        positionsToCheck.Add(new Position(_x, _y + 1));
                    if (_y - 1 > -1 && getPositionStatus(_x, _y - 1) == Status.none && positionsToCheck.Find(p => p.getX() == _x && p.getY() == _y - 1) == null)
                        positionsToCheck.Add(new Position(_x, _y - 1));

                    opponentGameboard.ships.Find(s => s.getStatus() == opponentGameboard.getPositionStatus(_x,_y)).hit();
                    getPositionFromList(_x, _y).hit();
                }
                else getPositionFromList(_x, _y).setStatusMissed();

                positionsToCheck.RemoveAt(randomFromPostionsToCheck);
                foreach (Position p in positionsToCheck)
                {
                    Console.WriteLine(p.getX() + "," + p.getY());
                }
            }


            public void shootRandom(OwnGameboard opponentGameboard)
            {
                int randomX, randomY;
                do
                {
                    randomX = random.Next(0, gameboardSize);
                    randomY = random.Next(0, gameboardSize);
                } while (getPositionStatus(randomX, randomY) != Status.none);
                Console.WriteLine("random shot: x:" + randomX + " y:" + randomY);

                if (opponentGameboard.positionIsShip(randomX, randomY))
                {
                    if (randomX + 1 < 10 && getPositionStatus(randomX + 1, randomY) == Status.none && positionsToCheck.Find(p => p.getX() == randomX + 1 && p.getY() == randomY) == null)
                        positionsToCheck.Add(new Position(randomX + 1, randomY));
                    if (randomX - 1 > -1 && getPositionStatus(randomX - 1, randomY) == Status.none && positionsToCheck.Find(p => p.getX() == randomX - 1 && p.getY() == randomY) == null)
                        positionsToCheck.Add(new Position(randomX - 1, randomY));
                    if (randomY + 1 < 10 && getPositionStatus(randomX, randomY + 1) == Status.none && positionsToCheck.Find(p => p.getX() == randomX && p.getY() == randomY + 1) == null)
                        positionsToCheck.Add(new Position(randomX, randomY + 1));
                    if (randomY - 1 > -1 && getPositionStatus(randomX, randomY - 1) == Status.none && positionsToCheck.Find(p => p.getX() == randomX && p.getY() == randomY - 1) == null)
                        positionsToCheck.Add(new Position(randomX, randomY - 1));

                    opponentGameboard.ships.Find(ship => ship.getStatus() == opponentGameboard.getPositionStatus(randomX, randomY)).hit();
                    getPositionFromList(randomX, randomY).hit();
                }
                else getPositionFromList(randomX, randomY).setStatusMissed();
            }
        }

        public class Player
        {
            public OwnGameboard ownGameboard = new OwnGameboard();
            public Fireboard fireboard = new Fireboard();
        }

        public class Game
        {
            public Game()
            {
                Player p1 = new Player();
                Player p2 = new Player();
                //public void start()
                //{
                while (!p1.ownGameboard.shipsAreSunk() && !p2.ownGameboard.shipsAreSunk())
                {
                    /*Console.Clear();
                    
                    p1.ownGameboard.print();*/
                    Console.WriteLine("P1");
                    p1.fireboard.shoot(p2.ownGameboard);
                    if (p2.ownGameboard.shipsAreSunk()) break;
                    p1.fireboard.print();
                    //Thread.Sleep(1000);

                    /*Console.Clear();
                    
                    p2.ownGameboard.print();*/
                    Console.WriteLine("P2");
                    p2.fireboard.shoot(p1.ownGameboard);
                    p2.fireboard.print();
                    //Thread.Sleep(1000);
                }
                //Console.Clear();
                Console.WriteLine("P1");
                p1.ownGameboard.print();
                p2.fireboard.print();
                Console.WriteLine("P2");
                p2.ownGameboard.print();
                p1.fireboard.print();
                if (p1.ownGameboard.shipsAreSunk()) Console.WriteLine("P2 won");
                if (p2.ownGameboard.shipsAreSunk()) Console.WriteLine("P1 won");
            }
        }

        static int Main(string[] args)
        {
            Game game = new Game();
            //}
            //Player player2 = new Player();
            //OwnGameboard ownGameboard = new OwnGameboard();
            //Fireboard fireboard = new Fireboard();
            //fireboard.randomShoot(ownGameboard);
            //gameboard.randomlyPlaceShip(gameboard.carrier);
            //gameboard.randomlyPlaceShip(gameboard.carrier);
            //ownGameboard.print();
            //Console.WriteLine();
            //fireboard.print();
            //Console.WriteLine();
            return 0;
        }
    }
}
