﻿using System;
using System.IO;
using NLog;
using System.Collections.Generic;

namespace MovieListing
{
    class MainClass
    {
        // create a class level instance of logger (can be used in methods other than Main)
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            // config is loaded using xml (NLog.config saved in  debug folder)
            logger.Info("Program started");

            // path to movie data file
            string file = "../../data/movies.csv";

            // make sure movie file exists
            if (!File.Exists(file))
            {
                logger.Error("File does not exist: {File}", file);
            }
            else
            {
                string choice;
                do
                {
                    // display choices to user
                    Console.WriteLine("1) Add Movie");
                    Console.WriteLine("2) Display All Movies");
                    Console.WriteLine("Enter to quit");

                    // input selection
                    choice = Console.ReadLine();
                    logger.Info("User choice: {Choice}", choice);

                    // create parallel lists of movie details
                    // lists must be used since we do not know number of lines of data
                    List<UInt64> MovieIds = new List<UInt64>();
                    List<string> MovieTitles = new List<string>();
                    List<string> MovieGenres = new List<string>();
                    // to populate the lists with data, read from the data file
                    try
                    {
                        StreamReader sr = new StreamReader(file);
                        // first line contains column headers
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            // first look for quote(") in string
                            // this indicates a comma(,) in movie title
                            int idx = line.IndexOf('"');
                            if (idx == -1)
                            {
                                // no quote = no comma in movie title
                                // movie details are separated with comma(,)
                                string[] movieDetails = line.Split(',');
                                // 1st array element contains movie id
                                MovieIds.Add(UInt64.Parse(movieDetails[0]));
                                // 2nd array element contains movie title
                                MovieTitles.Add(movieDetails[1]);
                                // 3rd array element contains movie genre(s)
                                // replace "|" with ", "
                                MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }


                    if (choice == "1")
                    {
                        // Add Movie
                    }
                    else if (choice == "2")
                    {
                        // Display All Movies
                        // loop thru Movie Lists
                        for (int i = 0; i < MovieIds.Count; i++)
                        {
                            // display movie details
                            Console.WriteLine($"Id: {MovieIds[i]}");
                            Console.WriteLine($"Title: {MovieTitles[i]}");
                            Console.WriteLine($"Genre(s): {MovieGenres[i]}");
                            Console.WriteLine();
                        }
                    }
                } while (choice == "1" || choice == "2");
            }

            logger.Info("Program ended");
        }
    }
}
