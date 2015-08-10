﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Controllers;
using Game.Core;
using Mono.Options;
using Newtonsoft.Json;

namespace Game.Runner.Commandline
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var files = new List<string>();
            var extra = new List<string>();
            var phrases = new List<string>();
            int timeout;
            int memoryLimit;
            int cores;

            string output = null;

            var verbose = false;
            var show_help = false;

            var p = new OptionSet
            {
                "Usage: icfp2015 [OPTIONS]+",
                "Greet a list of individuals with an optional message.",
                "If no message is specified, a generic greeting is used.",
                "",
                "Options:",
                {
                    "f|file=", "File containing JSON encoded input",
                    v =>
                    {
                        if (!v.Contains("*"))
                            files.Add(v);
                        else
                        {
                            files.AddRange(Directory.GetFiles(Path.GetDirectoryName(v), Path.GetFileName(v)));
                        }
                    }
                },
                {
                    "t|timeout=",
                    "Time limit, in seconds, to produce output",
                    (int v) => timeout = v
                },
                {
                    "m|memory=",
                    "Memory limit, in megabytes, to produce output",
                    (int v) => memoryLimit = v
                },
                {
                    "c|cores=",
                    "Number of processor cores available",
                    (int v) => cores = v
                },
                {
                    "p|phrase=", "Phrase of power",
                    v => phrases.Add(v)
                },
                {
                    "v", "increase debug message verbosity",
                    v => { if (v != null) verbose = true; }
                },
                {
                    "h|help", "show this message and exit",
                    v => show_help = v != null
                }
            };

            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("icfp2015: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `icfp2015 --help' for more information.");
                return;
            }

            if (show_help)
            {
                p.WriteOptionDescriptions(Console.Out);
                return;
            }

            var outputs = new List<Output>();
            var totalScore = 0;

            foreach (var file in files)
            {
                var problem = Problem.FromFile(file);
                var controller = new NotSoGreatController(problem);
                var scores = new List<int>();
                controller.OnGameOver += state =>
                {             
                    scores.Add(state.Score);
                };

                ProgressBar progress = null;
                if (verbose)
                {
                    progress = new ProgressBar();
                    controller.OnLock +=
                        state => { progress.Report((double) state.UnitsLeft/state.Problem.SourceLength); };
                }

                outputs.AddRange(controller.Solve());
                progress?.Dispose();

                if (verbose)
                    Console.Error.WriteLine("[{1}] Score: {0}", scores.Average().ToString("#####"), problem.Id);

                totalScore += (int) scores.Average();
            }

            if (verbose)
                Console.Error.WriteLine("Total Score: {0}", totalScore);

            Console.Write(JsonConvert.SerializeObject(outputs));
            
        }       
    }
}