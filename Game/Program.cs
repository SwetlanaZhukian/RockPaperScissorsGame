using System;
using System.Security.Cryptography;
using System.Text;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 3 || args.Length % 2 == 0)
            {
                Console.WriteLine("Invalid input!");
                return;

            }

            for (int i = 0; i <= args.Length; i++)
            {
                for (int j = i + 1; j < args.Length; j++)
                {
                    if (args[i] == args[j])
                    {
                        Console.WriteLine("Invalid input!");
                        return;

                    }
                }
            }

            bool invalidInput = true;
            int index = 1;
            string userMove = "";
            string key = GenerateRandomCryptographicKey(32);
            string computerMove = GenerateRandomMove(args);
  
            Console.WriteLine("HMAC: " + GeherateHMAC(key, computerMove));
            Console.WriteLine("Available moves:");

            CircularDoublyLinkedList<string> moves = new CircularDoublyLinkedList<string>();
            foreach (string move in args)
            {
                moves.Add(move);
            }
             
            foreach (string s in moves)
            {
                Console.WriteLine(index + " - " + s);
                index++;
            }

            Console.WriteLine(0 + " - exit");

            while (invalidInput)
            {
                Console.WriteLine("Enter your move: ");
                try
                {
                    int selection = Convert.ToInt32(Console.ReadLine());

                    if (selection == 0)
                    {
                        Console.WriteLine("Goodbye!");
                        return;
                    }
                    else if (selection > 0 && selection <= args.Length)
                    {
                        userMove = args[selection - 1];
                        Console.WriteLine("Your move: " + userMove);
                        invalidInput = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Computer move: " + computerMove);

            var nextItem= moves.Find(computerMove).Next;
            var previousItem = moves.Find(computerMove).Previous;

            for (int i = 0; i < (moves.Count - 1) / 2; i++)
            {

                if (moves.Find(computerMove) == moves.Find(userMove))
                {
                    Console.WriteLine("Draw!");
                    break;
                }
                if (nextItem == moves.Find(userMove))
                {
                    Console.WriteLine("You win!");
                    break;
                }
                 if (previousItem == moves.Find(userMove))
                {
                    Console.WriteLine("You lose!");
                    break;
                }
                nextItem = nextItem.Next;
                previousItem = previousItem.Previous;
            }

            Console.WriteLine("HMAC key: " + key);

        }

        static public string GenerateRandomCryptographicKey(int keyLength)
        {
            RandomNumberGenerator randomNumber = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[keyLength];
            randomNumber.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        static public string GenerateRandomMove(string[] moves)
        {
            Random random = new Random();
            int index = random.Next(moves.Length);
            return moves[index];

        }

        static public string GeherateHMAC(string key, string message)
        {
            var decodedKey = Encoding.Default.GetBytes(key);

            using (HMACSHA256 hmac = new HMACSHA256(decodedKey))
            {
                var messageBytes = Encoding.Default.GetBytes(message);
                var hash = hmac.ComputeHash(messageBytes);
                return string.Concat(Array.ConvertAll(hash, b => b.ToString("X2"))).ToLower();
            }

        }
    }
}
