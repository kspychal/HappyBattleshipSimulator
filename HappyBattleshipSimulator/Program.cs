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

            public int getX()
            {
                return x;
            }

            public int getY()
            {
                return y;
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
            public Carrier()
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
        }

        public class OwnGameboard : Gameboard
        {
            public OwnGameboard() : base()
            {
                ships.Add(carrier);
                ships.Add(battleship);
                ships.Add(cruiser);
                ships.Add(submarine);
                ships.Add(destroyer);
                foreach (Ship ship in ships) randomlyPlaceShip(ship);
                print();
            }

            private Carrier carrier = new Carrier();
            private Battleship battleship = new Battleship();
            private Cruiser cruiser = new Cruiser();
            private Submarine submarine = new Submarine();
            private Destroyer destroyer = new Destroyer();
            public List<Ship> ships = new List<Ship>();

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
                            if (positions.ElementAt((gameboardSize * (randomY + i) + randomX)).status != Status.none)
                            {
                                flag = true;
                                break;
                            }
                        }
                        else
                        {
                            if (positions.ElementAt((gameboardSize * randomY + randomX + i)).status != Status.none)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) continue;
                    break;
                } while (true);

                //ship.placeShip(randomX, randomY, isVertical, ship.getSize());
                for (int i = 0; i < ship.getSize(); i++)
                {
                    if (isVertical == true)
                    {
                        positions.ElementAt((gameboardSize * (randomY + i) + randomX)).status = ship.getStatus();
                    }
                    else
                    {
                        positions.ElementAt((gameboardSize * randomY + randomX + i)).status = ship.getStatus();
                    }
                }
            }
        }

        public class Fireboard : Gameboard
        {
            int x, y;
            List<Position> positionsToCheck = new List<Position>();


            public void shoot(OwnGameboard opponentGameboard)
            {
                if (positionsToCheck.Count() > 0) shootNear(opponentGameboard);
                else randomShoot(opponentGameboard);
            }

            public void shootNear(OwnGameboard opponentGameboard)
            {
                int randomFromPostionsToCheck = random.Next(0, positionsToCheck.Count());
                int _x = positionsToCheck.ElementAt(randomFromPostionsToCheck).getX();
                int _y = positionsToCheck.ElementAt(randomFromPostionsToCheck).getY();
                Console.WriteLine("Shot near: x:" + x + "   y:" + y);

                if (opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == Status.cruiser || opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == Status.battleship 
                    || opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == Status.carrier || opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == Status.destroyer
                    || opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == Status.submarine)
                {
                    if (_x + 1 < 10 && positions.ElementAt(_x + 1 + gameboardSize * _y).status == Status.none && !positionsToCheck.Contains(new Position(_x + 1, _y))) positionsToCheck.Add(new Position(_x + 1, _y));
                    if (_x - 1 > -1 && positions.ElementAt(_x - 1 + gameboardSize * _y).status == Status.none && !positionsToCheck.Contains(new Position(_x - 1, _y))) positionsToCheck.Add(new Position(_x - 1, _y));
                    if (_y + 1 < 10 && positions.ElementAt(_x + gameboardSize * (_y + 1)).status == Status.none && !positionsToCheck.Contains(new Position(_x, _y + 1))) positionsToCheck.Add(new Position(_x, _y + 1));
                    if (_y - 1 > -1 && positions.ElementAt(_x + gameboardSize * (_y - 1)).status == Status.none && !positionsToCheck.Contains(new Position(_x, _y - 1))) positionsToCheck.Add(new Position(_x, _y - 1));

                    positions.ElementAt(_x + gameboardSize * _y).hit();
                    Ship tmpShip = opponentGameboard.ships.Find(ship => ship.getStatus() == opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status);
                    if (tmpShip != null) tmpShip.hit();
                    //opponentGameboard.ships.
                    //opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).hit();
                    /*foreach (Ship ship in opponentGameboard.ships)
                    {
                        if (opponentGameboard.positions.ElementAt(_x + gameboardSize * _y).status == ship.getStatus())
                        {
                            ship.hit();
                            break;
                        }
                    }*/
                }
                else positions.ElementAt(_x + gameboardSize * _y).status = Status.missed;

                positionsToCheck.RemoveAt(randomFromPostionsToCheck);
            }


            public void randomShoot(OwnGameboard opponentGameboard)
            {
                int randomX, randomY, randomPosition;
                do
                {
                    randomX = random.Next(0, gameboardSize);
                    randomY = random.Next(0, gameboardSize);
                    randomPosition = gameboardSize * randomY + randomX;
                } while (positions.ElementAt(randomPosition).status != Status.none);
                Console.WriteLine("random shot: x:" + randomX + " y:" + randomY);

                if (opponentGameboard.positions.ElementAt(randomPosition).status == Status.cruiser || opponentGameboard.positions.ElementAt(randomPosition).status == Status.battleship 
                    || opponentGameboard.positions.ElementAt(randomPosition).status == Status.carrier || opponentGameboard.positions.ElementAt(randomPosition).status == Status.destroyer
                    || opponentGameboard.positions.ElementAt(randomPosition).status == Status.submarine)
                {
                    x = randomX;
                    y = randomY;

                    if (x + 1 < 10 && positions.ElementAt(x + 1 + gameboardSize * y).status == Status.none && !positionsToCheck.Contains(new Position(x + 1, y))) positionsToCheck.Add(new Position(x + 1, y));
                    if (x - 1 > -1 && positions.ElementAt(x - 1 + gameboardSize * y).status == Status.none && !positionsToCheck.Contains(new Position(x - 1, y))) positionsToCheck.Add(new Position(x - 1, y));
                    if (y + 1 < 10 && positions.ElementAt(x + gameboardSize * (y + 1)).status == Status.none && !positionsToCheck.Contains(new Position(x, y + 1))) positionsToCheck.Add(new Position(x, y + 1));
                    if (y - 1 > -1 && positions.ElementAt(x + gameboardSize * (y - 1)).status == Status.none && !positionsToCheck.Contains(new Position(x, y - 1))) positionsToCheck.Add(new Position(x, y - 1));

                    positions.ElementAt(randomPosition).hit();
                    opponentGameboard.ships.Find(ship => ship.getStatus() == opponentGameboard.positions.ElementAt(randomPosition).status).hit();
                    //tmpShip.hit();
                    /*foreach (Ship ship in opponentGameboard.ships)
                    {
                        if (opponentGameboard.positions.ElementAt(x + gameboardSize * y).status == ship.getStatus())
                        {
                            positions.ElementAt(randomPosition).hit();
                            ship.hit();
                            break;
                        }
                    }*/
                }
                else positions.ElementAt(randomPosition).status = Status.missed;
            }
        }

        public class Player
        {
            public OwnGameboard ownGameboard = new OwnGameboard();
            public Fireboard fireboard = new Fireboard();
            public Player()
            {
                /*while (!ownGameboard.shipsAreSunk())
                {
                    Console.Clear();
                    ownGameboard.print();
                    fireboard.shoot(ownGameboard);
                    fireboard.print();
                    //Thread.Sleep(1);
                    
                }*/
            }
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
                    Console.WriteLine("P1");
                    p1.ownGameboard.print();*/
                    p1.fireboard.shoot(p2.ownGameboard);
                    if (p2.ownGameboard.shipsAreSunk()) break;
                    /*p1.fireboard.print();
                    Thread.Sleep(1000);*/

                    /*Console.Clear();
                    Console.WriteLine("P2");
                    p2.ownGameboard.print();*/
                    p2.fireboard.shoot(p1.ownGameboard);
                    /*p2.fireboard.print();
                    Thread.Sleep(1000);*/
                }
                Console.Clear();
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
