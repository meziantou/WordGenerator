using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace WordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption("-?|-h|--help");
            app.VersionOption("--version", "1.0.0");

            app.Command("generate", command =>
            {
                command.HelpOption("-?|-h|--help");
                var pathArg = command.Argument("[path]", "Path to the file that contains words");

                command.OnExecute(() =>
                {
                    string path = pathArg.Value;
                    if (string.IsNullOrEmpty(path))
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    var generator = new WordGenerator();
                    using (var reader = new StreamReader(path))
                    {
                        string line;
                        while (!reader.EndOfStream && (line = reader.ReadLine()) != null)
                        {
                            generator.AddWord(line);
                        }
                    }

                    for (int j = 0; j < 100; j++)
                    {
                        Console.WriteLine(generator.Generate(7));
                    }

                    return 0;
                });
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException)
            {
                app.ShowHelp();
            }
        }
    }
}
