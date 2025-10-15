namespace RedRoverCodePuzzle;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2 || string.IsNullOrWhiteSpace(args[0]) || string.IsNullOrWhiteSpace(args[1]))
        {
            throw new ArgumentException("You must provide two arguments: \"Input String\" and \"Sorting Strategy\".");
        }

        var stringToTransform = args[0];
        var sortingStrategyInput = args[1];

        ISortingStrategy sortingStrategy = sortingStrategyInput switch
        {
            "none" => new NoSort(),
            "alpha" => new AlphabeticalAscendingFullTraversalSort(),
            _ => throw new ArgumentException($"\"{sortingStrategyInput}\" is not a supported sorting strategy."),
        };

        var stringTransformer = new StringTransformer(stringToTransform, sortingStrategy);
        var output = stringTransformer.Transform();

        Console.Write(output);
    }
}