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
                var orderOptions = command.Option("--order <ORDER>", "order of Markov model", CommandOptionType.SingleValue);
                var lengthOptions = command.Option("--length <LENGTH>", "length of the words to generate", CommandOptionType.SingleValue);
                var countOptions = command.Option("--count <COUNT>", "number of words to generate", CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    string path = pathArg.Value;
                    if (string.IsNullOrEmpty(path))
                    {
                        app.ShowHelp();
                        return 1;
                    }

                    var generator = new WordGenerator();
                    if (orderOptions.HasValue())
                    {
                        int order;
                        if (int.TryParse(orderOptions.Value(), out order))
                        {
                            generator.Order = order;
                        }
                    }

                    int length;
                    int count;
                    if (!int.TryParse(lengthOptions.Value(), out length))
                    {
                        length = 7;
                    }

                    if (!int.TryParse(countOptions.Value(), out count))
                    {
                        count = 10;
                    }

                    using (var reader = new StreamReader(path))
                    {
                        string line;
                        while (!reader.EndOfStream && (line = reader.ReadLine()) != null)
                        {
                            generator.AddWord(line);
                        }
                    }

                    for (int j = 0; j < count; j++)
                    {
                        Console.WriteLine(generator.Generate(length));
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
