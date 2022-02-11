using System;
using System.IO;

namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            string[] words = getData("words.txt");
            Console.WriteLine("================================");
            Console.WriteLine("Please select difficulty:");
            Console.WriteLine("1 - Easy");
            Console.WriteLine("2 - Hard");
            string difficulty = Console.ReadLine();
            int selected = 0;
            while(selected == 0) {
                switch (difficulty)
                {
                    case "1":
                    case "easy":
                    case "Easy":
                        difficulty = "easy";
                        selected = 1;
                        Console.WriteLine("You selected easy");
                        break;
                    case "2":
                    case "hard":
                    case "Hard":
                        difficulty = "hard";
                        selected = 1;
                        Console.WriteLine("You selected hard");
                        break;
                    default:
                        Console.WriteLine("Please select number or difficulty level");
                        break;
                } 
            }
            // Cannot leave this loop without difficulty selected
            Console.WriteLine("================================");

            int size = words.Length;
            // Pseudo-Randomly selected words
            string[] selectedWords;
            if(difficulty == "easy")
            {
                selectedWords = new string[4];
                for(int i=0; i<4; i++)
                {
                    selectedWords[i] = words[rng.Next(size)];
                }
            }
            else
            {
                selectedWords = new string[8];
                for (int i = 0; i < 8; i++)
                {
                    selectedWords[i] = words[rng.Next(size)];
                }
            }

            startGame(selectedWords, difficulty);
        }

        static void startGame(string[] gameWords, string diff)
        {
            int won = 0;
            Random rng = new Random();
            string[,] hiddenBoard;
            string[,] gameBoard;
            int roundCap;
            int pairs;
            if(diff == "easy")
            {
                pairs = 4;
                roundCap = 10;
                hiddenBoard = new string[2, pairs];
                gameBoard = new string[2, pairs];
            }
            else
            {
                pairs = 8;
                roundCap = 15;
                hiddenBoard = new string[2, pairs];
                gameBoard = new string[2, pairs];
            }
            

            foreach (string word in gameWords)
            {
                int repetition = 0;
                while(repetition < 2)
                {
                    int x = rng.Next(pairs);
                    int y = rng.Next(2);
                    if(hiddenBoard[y, x] == null)
                    {
                        hiddenBoard[y, x] = word;
                        repetition++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            for (int i=0; i<2; i++)
            {
                for(int j=0; j<pairs; j++)
                {
                    gameBoard[i, j] = "X";
                }
            }

            int roundCounter = 0;

            
            
            // one iteration is one round
            while (roundCounter < roundCap)
            {
                Console.WriteLine("-----------------");
                Console.WriteLine("   Level: {0}", diff);
                Console.WriteLine("   Guess chances: {0}", roundCap - roundCounter);
                Console.WriteLine("");
                showGameBoard(gameBoard, diff);
                Console.WriteLine("-----------------");

                Console.WriteLine("Choose position");
                string pos = "";
                do
                {
                    pos = Console.ReadLine();
                    if (!Char.IsLetter(pos[0]) || !Char.IsDigit(pos[1]))
                    {
                        pos = String.Empty;
                        Console.WriteLine("Write position in format LetterNumber - e.g A1");
                    }
                    if (Char.GetNumericValue(pos[1]) > pairs || Char.GetNumericValue(pos[1]) == 0 
                        || !"AB".Contains(pos[0]))
                    {
                        pos = String.Empty;
                        Console.WriteLine("There is only two letters (A,B) and {0} columns", pairs);
                    }
                } while (pos.Length != 2);
                int x1 = (int)Char.GetNumericValue(pos[1])-1;
                int y1;
                if (pos[0] == 'A') y1 = 0;
                else y1 = 1;

                if (gameBoard[y1, x1] != "X")
                {
                    Console.WriteLine("========");
                    Console.WriteLine("IT'S OK!");
                    Console.WriteLine("========");
                    continue;
                }
                gameBoard[y1, x1] = hiddenBoard[y1, x1];
                Console.WriteLine("-----------------");
                Console.WriteLine("   Level: {0}", diff);
                Console.WriteLine("   Guess chances: {0}", roundCap - roundCounter);
                Console.WriteLine("");
                showGameBoard(gameBoard, diff);
                Console.WriteLine("-----------------");


                do
                {
                    pos = Console.ReadLine();
                    if (!Char.IsLetter(pos[0]) || !Char.IsDigit(pos[1]))
                    {
                        pos = String.Empty;
                        Console.WriteLine("Write position in format LetterNumber - e.g A1");
                    }
                    if (Char.GetNumericValue(pos[1]) > pairs || Char.GetNumericValue(pos[1]) == 0
                        || !"AB".Contains(pos[0]))
                    {
                        pos = String.Empty;
                        Console.WriteLine("There is only two letters (A,B) and {0} columns", pairs);
                    }
                } while (pos.Length != 2);
                int x2 = (int)Char.GetNumericValue(pos[1])-1;
                int y2;
                if (pos[0] == 'A') y2 = 0;
                else y2 = 1;

                if (gameBoard[y2, x2] != "X")
                {
                    gameBoard[y1, x1] = "X";
                    roundCounter++;
                    Console.WriteLine("========");
                    Console.WriteLine("FAILURE!");
                    Console.WriteLine("========");
                    continue;
                }
                gameBoard[y2, x2] = hiddenBoard[y2, x2];
                Console.WriteLine("-----------------");
                Console.WriteLine("   Level: {0}", diff);
                Console.WriteLine("   Guess chances: {0}", roundCap - roundCounter);
                Console.WriteLine("");
                showGameBoard(gameBoard, diff);
                Console.WriteLine("-----------------");

                if(gameBoard[y2, x2] != gameBoard[y1, x1])
                {
                    gameBoard[y1, x1] = "X";
                    gameBoard[y2, x2] = "X";
                    Console.WriteLine("========");
                    Console.WriteLine("FAILURE!");
                    Console.WriteLine("========");
                }
                else
                {
                    Console.WriteLine("========");
                    Console.WriteLine("CORRECT!");
                    Console.WriteLine("========");
                }

                if(isGameEnded(gameBoard, pairs) == true)
                {
                    won = 1;
                    break;
                }

                System.Threading.Thread.Sleep(2000);
                Console.Clear();

                roundCounter++;
            }
            if(won == 1)
            {
                Console.WriteLine("================");
                Console.WriteLine("CONGRATULATIONS!");
                Console.WriteLine("================");
            }
            else
            {
                Console.WriteLine("================");
                Console.WriteLine("MAYBE NEXT TIME!");
                Console.WriteLine("================");
            }

        }

        static Boolean isGameEnded(string [,] board, int pairs)
        {
            for(int i=0; i<2; i++)
            {
                for(int j=0; j<pairs; j++)
                {
                    if(board[i,j] == "X")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void showGameBoard(string[,] gameBoard, string diff)
        {
            int cols;
            if (diff == "easy") {
                cols = 4;
                Console.WriteLine("     1 2 3 4");
            }
            else {
                cols = 8;
                Console.WriteLine("     1 2 3 4 5 6 7 8");
            }
            Console.Write("   A ");
            for (int i = 0; i < cols; i++)
                Console.Write("{0} ", gameBoard[0, i]);
            Console.Write("\n");

            Console.Write("   B ");
            for (int i = 0; i < cols; i++)
                Console.Write("{0} ", gameBoard[1, i]);
            Console.Write("\n");
        }

        static string[] getData(string filename)
        {
            int counter = 0;
            string[] words = null;
            foreach (string line in File.ReadAllLines(filename))
            {
                if (line != String.Empty)
                {
                    counter++;
                }
            }
            words = new string[counter];
            int p = 0;
            foreach (string line in File.ReadAllLines(filename))
            {
                words[p] = line;
                p++;
            }

            return words;
        }
    }
}
