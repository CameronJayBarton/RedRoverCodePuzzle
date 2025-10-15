using System.Text;

namespace RedRoverCodePuzzle;

public class Node
{
    public string Value { get; } = string.Empty;

    public Node? Parent { get; } = null;

    public List<Node> Children { get; } = [];

    public Node() { }

    public Node(string value, Node parent)
    {
        Value = value;
        Parent = parent;
    }

    /// <summary>
    /// Recursively converts the current node's descendants into a structured string.
    /// </summary>
    /// <param name="itemPrefix">The string to prepend to each output item. Example: "- "</param>
    /// <param name="indentSpaceCount">The number of spaces that represent one nesting level - used to calculate the current actual indent. Example: 2</param>
    /// <param name="currentNestingLevel">The nesting level of the current node - used to calculate the current actual indent.</param>
    /// <returns>A string representation of the current node's descendants.</returns>
    public string DescendantsToString(string itemPrefix, int indentSpaceCount, int currentNestingLevel = 0)
    {
        var outputBuilder = new StringBuilder(string.Empty);

        foreach (var childNode in Children)
        {
            string indent = new(' ', indentSpaceCount * currentNestingLevel);
            string childDisplayValue = indent + itemPrefix + childNode.Value;

            if (!string.IsNullOrWhiteSpace(childDisplayValue))
            {
                outputBuilder.AppendLine(childDisplayValue);
            }

            currentNestingLevel++;
            var grandchildDescendantsDisplayValue = childNode.DescendantsToString(itemPrefix, indentSpaceCount, currentNestingLevel);
            currentNestingLevel--;

            if (!string.IsNullOrWhiteSpace(grandchildDescendantsDisplayValue))
            {
                outputBuilder.Append(grandchildDescendantsDisplayValue);
            }
        }

        return outputBuilder.ToString();
    }

    /// <summary>
    /// Reorders the descendants of this node according to the <param name="sortingStrategy"/> provided.
    /// </summary>
    public void Sort(ISortingStrategy sortingStrategy)
    {
        ArgumentNullException.ThrowIfNull(sortingStrategy);

        sortingStrategy.Sort(this);
    }
}