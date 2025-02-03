using MarkDown.Classes;

public class TagStack
{
    private readonly Stack<Tag> _stack = new Stack<Tag>();

    public void Push(Tag tagInfo)
    {
        _stack.Push(tagInfo);
    }

    public Tag Pop(string delimiter)
    {
        if (_stack.Count > 0 && _stack.Peek().Delimiter == delimiter)
        {
            return _stack.Pop();
        }
        return null;
    }

    public IEnumerable<Tag> GetTags()
    {
        while (_stack.Count > 0)
        {
            yield return _stack.Pop();
        }
    }

    public bool IsEmpty => _stack.Count == 0;

    public bool Contains(string delimiter)
    {
        foreach (var tag in _stack)
        {
            if (tag.Delimiter == delimiter)
            {
                return true;
            }
        }
        return false;
    }
}
