# HappyBattleshipSimulator
-Ships can touch each other but not overlap

-Setting the positions of ship on the board - due to the fact that it is a simulation of the game between players, random place ships on the board looks like: at the beginning, place of first position on which ship will be located is determined. Then the direction in which the ship will be inserted is selected (vertically (downwards) or horizontally (to the right)), it is checked if these positions are available, and then the ship is placed on the board. If it would be game between two real players, I would develope that player choose the first position of ship, and then freely select the direction (could be up, down, left, right), then I'll have to add additional(left and up) validation if ship fits board 

-Shooting function is switching between two modes - 1)random shoot (while we don't have any neighbours to check) and 2)shoot neighbour, which shoots near just hitted ship

-Random shooting algorithm can be improved by checking every second board position (since every ship size is at least 2 - in Milton Bradley or Hasbro version of Battleship Game, but when we add to app ship in size 1 - this algorithm won't work properly)

-Change the value of one variable (GameboardSize) makes possibility to decrease/increase size of the board
