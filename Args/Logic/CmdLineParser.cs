using Args.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Args.Logic;
internal class CmdLineParser
{
    /*Todo:
     * Refactorization, best practices... (exeception management for example)
     * Parametrisation du facultatif
     * ...
     * Gestion d'erreur
     * Ability to parse different type of input
     *  => Ajouter la possibité de configurer le parseur
     */
    private const int DefaultParam = 8080;

    private IEnumerable<Argument> Parsers = new Argument[]
    {
        new LogArgument(),
        new PortArgument(),
        new IntegerArrayArgument(),
        new StringArrayArgument(),
        new WhoAmI()
    };

    internal Param Parse(params string[] args)
    {
        var parsers = new List<Argument>(Parsers);
        var record = new Param(false, DefaultParam);
        for (var index = 0; index < args.Length; index++)
        {
            var current = args[index];
            var parser = Parsers.FirstOrDefault(x => x.Key == current);
            if (parser != null)
            {
                parsers.Remove(parser);
                record = parser.Parse(args, index, record);
                index += parser.AdditionalIncrement;
                continue;
            }
        }
        var missingRequiredArgument = parsers.FirstOrDefault(x => x.Required);
        if (missingRequiredArgument != null)
            throw new MissingArgumentException(missingRequiredArgument.Key);
        return record;
    }
}
