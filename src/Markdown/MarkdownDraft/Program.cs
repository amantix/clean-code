using System.Text;

namespace MarkdownDraft;

class Program
{
    static void Main(string[] args)
    {
        string str = "dawdawdadwad \n dwadawdadadwa";
        string result = String.Empty;
        
        
    }

    public List<Token> Parse(string str)
    {
        List<Token> tokens = new List<Token>();
        
        var endIndex = str.Length - 1;
        
        var curTokenStartIndex = 0;
        var curTokenEndIndex = 0;
        var curString = new StringBuilder();
        var endOfToken = true;
        Token curToken = new Token();
        
        for (int i = 0; i < endIndex; ++i)
        {   
            curString.Append(str[i]);
            int localIndex = i;
            
            if (str[i] == '_')
            {
                ++localIndex; // Чтобы начинать добавлять в curString уже после '_'
                
                curTokenStartIndex = 0;
                endOfToken = false; // Чтоб проверить по итогу токен закрылся или нет
                
                while (str[localIndex + 1] <= endIndex && str[localIndex + 1] != ' ')
                {
                    if (str[localIndex] == '_')
                    {
                        // Случай когда "... __ ..."
                        if (i - localIndex == 1)
                        {
                            curString.Append("__");
                            curToken.Type = TokenType.Text;
                            curToken.Content = curString.ToString();
                            endOfToken = true;
                            break;
                        }
                        
                        // Случай когда "... _слово_..."
                        curToken.Type = TokenType.Italics;
                        curToken.Content = curString.ToString();
                        endOfToken = true;
                        break;
                    }   
                    
                    curString.Append(str[localIndex]);
                    ++localIndex;
                }

                if (!endOfToken)
                {
                    curToken.Type = TokenType.Text;
                    curToken.Content = curString.ToString();
                }

                tokens.Add(curToken);
                curToken = new Token();
                curTokenStartIndex = 0;
                curTokenEndIndex = 0;
                curString = new StringBuilder();
                endOfToken = true;
                i = localIndex;
            }
        }

        return tokens;
    }
}