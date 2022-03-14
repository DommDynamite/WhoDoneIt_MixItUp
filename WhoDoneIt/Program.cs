using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace WhoDoneIt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(@$"{FindActiveDirectory()}");

            List<MurderGuess> guesses = new List<MurderGuess>();

            using (StreamReader r = new StreamReader(@$"{FindActiveDirectory()}\Data\Guesses.json"))
            {
                string serializedObject = r.ReadToEnd();
                guesses = JsonSerializer.Deserialize<List<MurderGuess>>(serializedObject);
            }

            string action = args[0];

            switch (action)
            {
                case "Add": // Add $username $allargs

                    if (guesses.Count == 0)
                    {
                        guesses.Add(new MurderGuess()
                        {
                            username = args[1],
                            guess = args[2],
                            guessesleft = 1
                        });

                        string chatMessage = $"New WhoDoneIt guess added: {guesses[guesses.Count-1].username} for [{guesses[guesses.Count-1].guess}]. {guesses[guesses.Count-1].guessesleft} guesses left!";

                        Console.WriteLine(chatMessage);
                    }
                    else
                    {
                        bool found = false;

                        foreach (MurderGuess guess in guesses)
                        {
                            if (guess.username == args[1] && guess.guessesleft > 0)
                            {
                                found = true;

                                guess.guess = args[2];

                                string message = $"{guess.username}'s guess has been updated to: [{guess.guess}].";

                                guess.guessesleft = guess.guessesleft - 1;

                                if (guess.guessesleft > 0)
                                {
                                    message = message + $" You have {guess.guessesleft} guess left!";
                                }
                                else
                                {
                                    message = message + " You have no guesses left!";
                                }

                                Console.WriteLine(message);
                            }
                            else if (guess.username == args[1] && guess.guessesleft == 0)
                            {
                                found = true;

                                Console.WriteLine("Sorry, but you have no more guesses!");
                            }
                        }

                        if (found == false)
                        {
                            guesses.Add(new MurderGuess()
                            {
                                username = args[1],
                                guess = args[2],
                                guessesleft = 1
                            });

                            string chatMessage = $"New WhoDoneIt guess added: {guesses[guesses.Count - 1].username} for [{guesses[guesses.Count - 1].guess}]. {guesses[guesses.Count - 1].guessesleft} guesses left!";

                            Console.WriteLine(chatMessage);
                        }
                    }

                    
                    string json = JsonSerializer.Serialize(guesses);
                    File.WriteAllText(@$"{FindActiveDirectory()}\Data\Guesses.json", json);

                    break;

                case "Check": // Check $userprimaryrole $username $allargs
                    
                    if (args[1] == "Mod" && args[3] == "All")
                    {
                        string guessList = "";

                        foreach (MurderGuess guess in guesses)
                        {
                            guessList = guessList + $"| {guess.username}: {guess.guess} ";
                        }

                        Console.WriteLine($"All Guesses {guessList}");
                    }
                    else
                    {
                        foreach (MurderGuess guess in guesses)
                        {
                            if (guess.username == args[2])
                            {
                                Console.WriteLine($"{guess.username} currently thinks it is [{guess.guess}] who did it.");
                            }
                        }
                    }

                    break;

                default:
                    break;
            }

            


        }

        public static string FindActiveDirectory()
        {
            if (Directory.GetCurrentDirectory().Contains("ExternalApp"))
            {
                return Directory.GetCurrentDirectory();
            }
            else
            {
                return Directory.GetCurrentDirectory() + @"\ExternalApp\WhoDoneIt";
            }
        }
    }

    public class MurderGuess
    {
        public string username { get; set; }

        public string guess { get; set; }

        public int guessesleft { get; set; }
    }
}
