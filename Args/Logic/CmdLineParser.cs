using Args.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Args.Logic;
internal class CmdLineParser
{
    /*Todo:
     * Ajouter Obligatoire et facultatif
     * ...
     * Gestion d'erreur
     * Ability to parse different type of input
     *  => Ajouter la possibité de configurer le parseur
     */
    private const int DefaultParam = 8080;

    private IEnumerable<Argument> Arguments = new Argument[]
    {
        new LogArgument(),
        new PortArgument(),
        new IntegerArrayArgument(),
        new StringArrayArgument()
    };

    internal Param Parse(params string[] args)
    {
        var record = new Param(false, DefaultParam);
        for (var index = 0; index < args.Length; index++)
        {
            var current = args[index];
            var argument = Arguments.FirstOrDefault(x => x.Key == current);
            if(argument != null)
            {
                record = argument.Parse(args, index, record);
                index += argument.AdditionalIncrement;
                continue;
            }
        }
        return record;
    }
}
