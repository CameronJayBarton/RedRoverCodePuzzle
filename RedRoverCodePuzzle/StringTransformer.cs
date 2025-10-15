using System.Globalization;
using System.Text;

namespace RedRoverCodePuzzle;

public class StringTransformer
{
    private const char NodeSeparator = ',';
    private const char NodeChildrenStart = '(';
    private const char NodeChildrenEnd = ')';
    private const int TotalCharacterMax = 10_000;
    private const int NodeSeparatorMax = 5_000;
    private const int NodeChildrenMax = 20;

    private const string OutputItemPrefix = "- ";
    private const int OutputIndentSpaceCount = 2;

    private string _input;
    private readonly ISortingStrategy _sortingStrategy;
    private readonly Node _rootNode = new();
    private readonly List<string> _errors = [];

    public StringTransformer(string input, ISortingStrategy sortingStrategy)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        ArgumentNullException.ThrowIfNull(sortingStrategy);

        _input = input;
        _sortingStrategy = sortingStrategy;
    }

    /// <summary>
    /// Operates on the input string stored at <see cref="_input"/> and returns an output string according to the Red Rover Code Puzzle requirements.
    /// https://github.com/RedRoverK12/Red-Rover-Code-Puzzle
    /// </summary>
    public string Transform()
    {
        ValidateInput();
        if (_errors.Count != 0)
        {
            var errorOutput = new StringBuilder($"{_errors.Count} error(s) enountered:{Environment.NewLine}");
            foreach (var error in _errors)
            {
                errorOutput.AppendLine(error);
            }
            return errorOutput.ToString();
        }

        SanitizeInput();

        ParseInputIntoNodeTree();

        _rootNode.Sort(_sortingStrategy);

        var output = _rootNode
            .DescendantsToString(OutputItemPrefix, OutputIndentSpaceCount)
            .TrimEnd();

        return output;
    }

    /// <summary>
    /// Validates the input stored at <see cref="_input"/> to identify two categories of issues:
    /// 1. Invalid structure, such as an opening NodeChildrenStart character without a closing NodeChildrenEnd character.
    /// 2. Unexpectedly demanding input, such as an unusually deep level of nesting.
    /// 
    /// If we need to support more demanding input, such as deeper levels of nesting, we should conduct additional testing and either
    /// (a) relax the input requirements (if the current program can handle the more demanding input) OR
    /// (b) change the program (if the current program cannot handle the more demanding input)
    /// </summary>
    private void ValidateInput()
    {
        var totalCharacterCount = _input.Length;
        var nodeSeparatorCount = _input.Count(x => x.Equals(NodeSeparator));
        var nodeChildrenStartCount = _input.Count(x => x.Equals(NodeChildrenStart));
        var nodeChildrenEndCount = _input.Count(x => x.Equals(NodeChildrenEnd));

        if (totalCharacterCount > TotalCharacterMax)
        {
            var totalCharacterMaxDisplay = TotalCharacterMax.ToString("N0");
            _errors.Add($"The input string must have {totalCharacterMaxDisplay} or fewer characters.");
        }

        if (nodeSeparatorCount > NodeSeparatorMax)
        {
            var nodeSeparatorCountDisplay = nodeSeparatorCount.ToString("N0");
            var nodeSeparatorMaxDisplay = NodeSeparatorMax.ToString("N0");
            _errors.Add($"You have included {nodeSeparatorCountDisplay} \"{NodeSeparator}\" characters, but the maximum supported is ${nodeSeparatorMaxDisplay}.");
        }

        if (nodeChildrenStartCount != nodeChildrenEndCount)
        {
            var nodeChildStartCountDisplay = nodeChildrenStartCount.ToString("N0");
            var nodeChildEndCountDisplay = nodeChildrenEndCount.ToString("N0");
            _errors.Add(
                $"There must be an equal number of \"{NodeChildrenStart}\" and \"{NodeChildrenEnd}\" characters, " +
                $"but you have included {nodeChildStartCountDisplay} \"{NodeChildrenStart}\" characters and {nodeChildEndCountDisplay} \"{NodeChildrenEnd}\" characters.");
        }
        else if (nodeChildrenStartCount > NodeChildrenMax)
        {
            var nodeChildMaxDisplay = NodeChildrenMax.ToString("N0");
            _errors.Add($"You have included {nodeChildMaxDisplay} \"{NodeChildrenStart}\" / \"{NodeChildrenEnd}\" characters, but the maximum supported is ${nodeChildMaxDisplay}.");
        }
    }

    /// <summary>
    /// Sanitizes the input string stored at <see cref="_input"/>."
    /// </summary>
    private void SanitizeInput()
    {
        _input = _input.Trim();

        // We always assume and create exactly one _rootNode when instantiating a StringTransformer object, so additional root level wrapping is unnecessary.
        while (_input.Length > 0 && _input[0].Equals(NodeChildrenStart) && _input[^1].Equals(NodeChildrenEnd))
        {
            _input = _input[1..];
            _input = _input[..^1];
            _input = _input.Trim();
        }
    }

    /// <summary>
    /// Parses the input string stored at <see cref="_input"/> into a node tree at <see cref="_rootNode"/> while maintaining the ordering present in the input string.
    /// </summary>
    private void ParseInputIntoNodeTree()
    {
        Node currentParentNode = _rootNode;
        Node? currentNode;
        StringBuilder nodeValueBuilder = new(string.Empty);
        string sanitizedNodeValue;
        bool skipCurrentNode;

        foreach (var inputChar in _input)
        {
            if (inputChar.Equals(NodeSeparator) || inputChar.Equals(NodeChildrenStart) || inputChar.Equals(NodeChildrenEnd))
            {
                // In our nodeValueBuilder we now have the complete string for the "Value" property of the current node.
                sanitizedNodeValue = nodeValueBuilder.ToString().Trim();
                skipCurrentNode = string.IsNullOrWhiteSpace(sanitizedNodeValue);

                if (!skipCurrentNode)
                {
                    currentNode = new(sanitizedNodeValue, currentParentNode);
                    currentParentNode.Children.Add(currentNode);

                    if (inputChar.Equals(NodeChildrenStart))
                    {
                        // We are starting a new branch, so the current node will be the parent of the following nodes.
                        currentParentNode = currentNode;
                    }
                }

                if (inputChar.Equals(NodeChildrenEnd))
                {
                    // We are ending the current branch, so traverse to the parent and continue.
                    currentParentNode = currentParentNode.Parent!;
                }

                nodeValueBuilder = new StringBuilder(string.Empty);
            }
            else
            {
                nodeValueBuilder.Append(inputChar);
            }
        }

        sanitizedNodeValue = nodeValueBuilder.ToString().Trim();
        skipCurrentNode = string.IsNullOrWhiteSpace(sanitizedNodeValue);
        if (!skipCurrentNode)
        {
            currentNode = new(sanitizedNodeValue, currentParentNode);
            currentParentNode.Children.Add(currentNode);
        }
    }
}