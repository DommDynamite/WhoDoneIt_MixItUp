using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace WhoDoneIt
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //Console.WriteLine($"{Directory.GetCurrentDirectory()}");

            // Remove Punctuation from the game's title
            string gameName = "";
            var sb = new StringBuilder();
            foreach (char c in args[1])
            {
                if (!char.IsPunctuation(c))
                {
                    sb.Append(c);
                }
            }

            gameName = sb.ToString();

            
            if (DoesJSONExist(gameName) == false)
            {
                List<MurderGuess> newList = new List<MurderGuess>();
                string json = JsonSerializer.Serialize(newList);
                File.WriteAllText(@$"{FindActiveDirectory()}\Data\{gameName}.json", json);
            }

            List<MurderGuess> guesses = new List<MurderGuess>();
            
            string action = args[0];

            switch (action)
            {
                case "Add": // Add $username $allargs

                    using (StreamReader r = new StreamReader(@$"{FindActiveDirectory()}\Data\{gameName}.json"))
                    {
                        string serializedObject = r.ReadToEnd();
                        guesses = JsonSerializer.Deserialize<List<MurderGuess>>(serializedObject);
                    }

                    string viewerGuess = "";
                    var sbGuess = new StringBuilder();
                    foreach (char c in args[3])
                    {
                        if (!char.IsPunctuation(c))
                        {
                            sbGuess.Append(c);
                        }
                    }

                    viewerGuess = sbGuess.ToString();

                    if (guesses.Count == 0)
                    {
                        guesses.Add(new MurderGuess()
                        {
                            username = args[2],
                            guess = viewerGuess,
                            guessesleft = 1
                        });

                        string chatMessage = $"New {args[1]} WhoDoneIt guess added: {guesses[guesses.Count-1].username} for [{guesses[guesses.Count-1].guess}]. {guesses[guesses.Count-1].guessesleft} guesses left!";

                        Console.WriteLine(chatMessage);
                    }
                    else
                    {
                        bool found = false;

                        foreach (MurderGuess guess in guesses)
                        {
                            if (guess.username == args[2] && guess.guessesleft > 0)
                            {
                                found = true;

                                guess.guess = viewerGuess;

                                string message = $"{guess.username}'s {args[1]} WhoDoneIt guess has been updated to: [{guess.guess}].";

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
                            else if (guess.username == args[2] && guess.guessesleft == 0)
                            {
                                found = true;

                                Console.WriteLine($"Sorry, but you have no more guesses for {args[1]}!");
                            }
                        }

                        if (found == false)
                        {
                            guesses.Add(new MurderGuess()
                            {
                                username = args[2],
                                guess = viewerGuess,
                                guessesleft = 1
                            });

                            string chatMessage = $"New {args[1]} WhoDoneIt guess added: {guesses[guesses.Count - 1].username} for [{guesses[guesses.Count - 1].guess}]. {guesses[guesses.Count - 1].guessesleft} guesses left!";

                            Console.WriteLine(chatMessage);
                        }
                    }

                    
                    string json = JsonSerializer.Serialize(guesses);
                    File.WriteAllText(@$"{FindActiveDirectory()}\Data\{gameName}.json", json);

                    break;

                case "Check": // Check $userprimaryrole $username $allargs

                    using (StreamReader r = new StreamReader(@$"{FindActiveDirectory()}\Data\{gameName}.json"))
                    {
                        string serializedObject = r.ReadToEnd();
                        guesses = JsonSerializer.Deserialize<List<MurderGuess>>(serializedObject);
                    }

                    if ((args[2] == "Mod" || args[2] == "Streamer" || args[2] == "VIP") && args[4] == "All")
                    {
                        string guessList = "";

                        foreach (MurderGuess guess in guesses)
                        {
                            guessList = guessList + $"| {guess.username}: {guess.guess} ";
                        }

                        Console.WriteLine($"All WhoDoneIt Guesses for {args[1]} : {guessList}");
                    }
                    else
                    {
                        foreach (MurderGuess guess in guesses)
                        {
                            if (guess.username == args[3])
                            {
                                Console.WriteLine($"In {args[1]}, {guess.username} currently thinks it is [{guess.guess}] who did it.");
                            }
                        }
                    }

                    break;

                case "Reset":

                    // Remove Punctuation from the game's title
                    string perpName = "";
                    var sb2 = new StringBuilder();
                    foreach (char c in args[2])
                    {
                        if (!char.IsPunctuation(c))
                        {
                            sb2.Append(c);
                        }
                    }

                    perpName = sb2.ToString();

                    using (StreamReader r = new StreamReader(@$"{FindActiveDirectory()}\Data\{gameName}.json"))
                    {
                        string serializedObject = r.ReadToEnd();
                        guesses = JsonSerializer.Deserialize<List<MurderGuess>>(serializedObject);
                    }

                    string jsonArchive = JsonSerializer.Serialize(guesses);
                    File.WriteAllText(@$"{FindActiveDirectory()}\Data\Archives\{gameName}_{perpName}.json", jsonArchive);

                    File.Delete(@$"{FindActiveDirectory()}\Data\{gameName}.json");

                    Console.WriteLine($"The perpetrator was deemed: [{args[2]}] and the guesses have been archived and reset. Ready for new guesses.");
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

        public static bool DoesJSONExist (string gameName)
        {
            string[] jsonFiles = Directory.GetFiles($@"{FindActiveDirectory()}\Data");

            bool found = false;

            if (jsonFiles.Length != 0)
            {
                foreach (string filename in jsonFiles)
                {
                    if (filename.Contains(gameName))
                    {
                        found = true;
                    }
                }
            }

            return found;
        }
    }

    public class MurderGuess
    {
        public string username { get; set; }

        public string guess { get; set; }

        public int guessesleft { get; set; }
    }
}
