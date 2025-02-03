using System.Text;

namespace Markdown;

public class MdToken
{ 
    private StringBuilder _content = new StringBuilder();
    private MdToken _token;
    bool isOpened = false;
    public MdTokenType Type;

    public MdToken()
    {
    }

    public MdToken(MdTokenType type)
    {
        Type = type;
    }

    public MdToken(MdToken token)
    {
        _token = token;
    }

    public void AddContent(char ch)
    {
        _content.Append(ch);
    }

    public void ClearContent()
    {
        _content.Clear();
    }

    public bool IsNull()
    {
        return _content.Length == 0;
    }

    public void InsertContent(int index, char ch)
    {
        _content.Insert(0, ch);
    }

    public StringBuilder GetContent()
    {
        return _content;
    }
    
}