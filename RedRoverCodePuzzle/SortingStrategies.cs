namespace RedRoverCodePuzzle;

public interface ISortingStrategy
{
    public void Sort(Node node);
}

/// <summary>
/// Respect the existing order of each descendant node.
/// </summary>
public class NoSort : ISortingStrategy
{
    public void Sort(Node node)
    {
        return;
    }
}

/// <summary>
/// Sort descendant nodes in alphabetical ascending order for every level of every branch of the node tree (recursively).
/// </summary>
public class AlphabeticalAscendingFullTraversalSort : ISortingStrategy
{
    public void Sort(Node node)
    {
        var comparer = new SortByValueAlphabeticalAscending();

        node.Children.Sort(comparer);

        foreach (var childNode in node.Children)
        {
            childNode.Sort(new AlphabeticalAscendingFullTraversalSort());
        }

        return;
    }
}

internal class SortByValueAlphabeticalAscending : IComparer<Node>
{
    public int Compare(Node? x, Node? y)
    {
        x ??= new Node();
        y ??= new Node();

        return string.Compare(x.Value, y.Value, StringComparison.OrdinalIgnoreCase);
    }
}