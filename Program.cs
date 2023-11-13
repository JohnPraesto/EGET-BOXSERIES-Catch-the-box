using System.Diagnostics;

namespace Catch_the_box
{
    internal class Program
    {
        // Initializing several variables here because they are used by several methods
        private static ManualResetEvent pauseEvent = new ManualResetEvent(true); // The box keeps moving between levels without this one
        private static Stopwatch timer = new Stopwatch(); // Keeps track of time
        private static int duration = 9; // The duration for a level is set to nine seconds
        private static int level = 1;
        private static int interval = 1000; // The interval at which the box moves. At level 1, the box moves once per second.
        private static int oX = 0; // Position setter for the player
        private static int oY = 0;
        private static int x = 20; // Position setter for the box
        private static int y = 10;
        static void Main(string[] args)
        {
            Console.WriteLine("Catch the box before the timer runs out (9 sec)!");
            Console.WriteLine("Use the arrow keys.");
            Console.Write("\nPress Enter to start");
            Console.ReadLine();
            Console.Clear();
            Console.Write("O");
            Thread xMove = new Thread(moveX); // Creates a separate thread that will run simultaneously as Main
            xMove.Start(); // It needs to be started. It's the moveX method that the thread is running.
            timer.Start();
            ConsoleKeyInfo oMove; // This will read the input from the usear (the arrow keys)
            while (timer.Elapsed.TotalSeconds < duration) // This loop stops running when the time elapsed during a level reaches 9 seconds
            {
                oMove = Console.ReadKey(); // Reading user input
                Console.SetCursorPosition(oX, oY); // Sets the cursor position to where oX and oY is
                Console.Write(" "); // Overwrites the previous position of the player
                if (oMove.Key == ConsoleKey.UpArrow) // Setting new coordinates for the player based on user input
                    oY--;
                else if (oMove.Key == ConsoleKey.RightArrow)
                    oX++;
                else if (oMove.Key == ConsoleKey.DownArrow)
                    oY++;
                else if (oMove.Key == ConsoleKey.LeftArrow)
                    oX--;
                checkBoundry(ref oX, ref oY);
                Console.SetCursorPosition(oX, oY); // Setting new position for the player
                Console.Write("O"); // Writes the new position of the player
                if (oX == x && oY == y) // If the player holds the same position as the Box:
                {
                    pauseEvent.Reset();
                    Console.Clear();
                    level++; // Getting to th next level
                    interval = (int)(interval * 0.75); // Reducing the Box movement interval (it will be faster for each level)
                    Console.WriteLine($"You captured the box! Press Enter to start level {level}!");
                    Reset();
                    pauseEvent.Set();
                }
            }
        }
        
        public static void moveX()
        {
            Random rnd = new Random();
           
            Console.SetCursorPosition(x, y);
            while (timer.Elapsed.TotalSeconds < duration)
            {
                pauseEvent.WaitOne();
                Console.Write("■"); // Writes the box
                Console.SetCursorPosition(Console.WindowWidth - 60, 0); // Adjust the position of the timer display
                Console.Write($"{duration - (int)timer.Elapsed.TotalSeconds}"); // Making the timer visable
                Thread.Sleep(interval); // Stops the loop for the duration of interval
                Console.SetCursorPosition(x, y);
                Console.Write(" "); // Overwrites the previous position of the box
                x += rnd.Next(-1, 2); // Creates a new adjacent random position for the box
                y += rnd.Next(-1, 2);
                checkBoundry(ref x, ref y);
                Console.SetCursorPosition(x, y);
                if (timer.Elapsed.TotalSeconds >= duration)
                {
                    Console.Clear();
                    Console.WriteLine("Time's up! Game Over.");
                    Console.WriteLine("You reached level " + level);
                    Console.Write("\nPress Enter to play again.");
                    interval = 1000;
                    level = 1;
                    Reset();
                }
            }
        }
        public static void checkBoundry(ref int x, ref int y)
        { // This method makes sure that the player and the box stays within the playing field
            if (x > 40)
                x = 40;
            if (x < 0)
                x = 0;
            if (y > 20)
                y = 20;
            if (y < 0)
                y = 0;
        }
        public static void Reset()
        {
            Console.ReadLine();
            oX = 0;
            oY = 0;
            x = 20;
            y = 10;
            timer.Reset();
            timer.Start();
            Console.Clear();
        }
    }
}
// Thoughts for improvement
// Create classes for player and box
// Gameboard is 40x20. Make board 20x20 but remove half of the vertical rows. Makes more even board.
// Try make the box moving away from the player