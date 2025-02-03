using System;
namespace Markdown;

public class MarkdownParser : IMarkdownParser
{
    public List<MdToken> TokensList = new List<MdToken>();
    public string Input;
    public int Length;
    private int currentPosition;
    private MdToken currentToken;

    public List<MdToken> ParseTokens(string input)
    {
        Input = input;
        Length = input.Length;
        for (currentPosition = 0; currentPosition < Input.Length; currentPosition++)
        {
            GetTokens();
        }
        return TokensList;
    }

    private void GetTokens()
    {
        DefineToken(Input[currentPosition], currentPosition);
    }

    private MdToken DefineToken(char ch, int position)
    {
        var token = new MdToken();
        if (ch == '#' && Input[position + 1] == ' ' && (position == 0 || Input[position - 1] == '\n'))
        {
            token.Type = MdTokenType.Heading;
            FillToken(token);
            return token;
        }

        if (ch == '_')
        {
            if (position < Input.Length - 1 && Input[position + 1] == '_')
            {
                token.Type = MdTokenType.Bold;
                FillToken(token);
                return token;
            }

            token.Type = MdTokenType.Italic;
            FillToken(token);
            return token;
        }
        
        FillToken(token);
        return token;
    }
    
    private void FillToken(MdToken token)
    {
        switch (token.Type)
        {
            case MdTokenType.Text: 
                FillText(token);
                TokensList.Add(token);
                break;
            case MdTokenType.Heading:
                FillHeadingToken(token);
                TokensList.Add(token);
                break;
            case MdTokenType.Bold:
                FillBoldToken(token);
                TokensList.Add(token);
                break;
            case MdTokenType.Italic:
                FillItalicToken(token);
                TokensList.Add(token);
                break;
            case MdTokenType.Escaping:
                break;
        }
    }
    
    private void FillHeadingToken(MdToken token)
    {
        currentPosition += 2;
        while (currentPosition < Input.Length)
        {
            if(Input[currentPosition] == '\n' || currentPosition == Input.Length) 
                break;
            token.AddContent(Input[currentPosition]);
            currentPosition++;
        }
    }

    private void FillText(MdToken token)
    {
        while (currentPosition < Input.Length)
        {
            if (Input[currentPosition] == ' ' || Input[currentPosition] == '\n')
                break;
            else
            {
                token.AddContent(Input[currentPosition]);
            }
            currentPosition++;
        }
    }

    private void FillBoldToken(MdToken token)
    {
        currentPosition += 2;
        while (currentPosition < Input.Length)
        {
            if (Input[currentPosition] == '_')
            {
                if(currentPosition + 1 < Input.Length && Input[currentPosition + 1] == '_')
                {
                    if((currentPosition + 2 < Input.Length && Input[currentPosition + 2] == ' ')
                       || currentPosition + 2 == Input.Length)
                        break;
                    else
                    {
                        token.AddContent(Input[currentPosition]);
                    }
                }
                else if (currentPosition + 1 < Input.Length && Input[currentPosition + 1] == ' ')
                {
                    token.AddContent(Input[currentPosition]);
                    token.Type = MdTokenType.Text;
                    break;
                }
                else
                {
                    token.AddContent(Input[currentPosition]);
                }
            }
            else if (Input[currentPosition] == '\\')
            {
                return;
            }
            else if (Input[currentPosition] == ' ')
            {
                if (token.IsNull())
                {
                    token.AddContent(Input[currentPosition]);
                    token.Type = MdTokenType.Text;
                    break;
                }
                if (currentPosition + 1 < Input.Length && Input[currentPosition + 1] == '_')
                {
                    token.AddContent(Input[currentPosition]);
                    token.Type = MdTokenType.Text;
                    break;
                }
                token.AddContent(Input[currentPosition]);
            }
            else
            {
                token.AddContent(Input[currentPosition]);
            }
            currentPosition++;
        } 
        if(token.Type == MdTokenType.Bold)
            currentPosition += 2;
        if (token.Type == MdTokenType.Text)
        {
            token.InsertContent(0, '_');
            token.InsertContent(0, '_');
        }
    }
    
    private void FillItalicToken(MdToken token)
    {
        currentPosition++;
        while (currentPosition < Input.Length)
        {
            if(Input[currentPosition] == '_')
            {
                if ((currentPosition + 1 < Input.Length && Input[currentPosition + 1] == ' ') ||
                    currentPosition + 1 == Input.Length)
                {
                    break;
                }   
                else
                {
                    token.AddContent(Input[currentPosition]);
                }
            }
            else if(Input[currentPosition] == ' ')
            {
                if(token.IsNull())
                {
                    token.AddContent(Input[currentPosition]);
                    token.Type = MdTokenType.Text;
                    break;
                }
                if (currentPosition + 1 < Input.Length && Input[currentPosition + 1] == '_')
                {
                    token.AddContent(Input[currentPosition]);
                    token.Type = MdTokenType.Text;
                    break;
                }
                token.AddContent(Input[currentPosition]);
            }
            else if (Input[currentPosition] == '\\')
            {
                return;
            }
            else
            {
                token.AddContent(Input[currentPosition]);
            }
            currentPosition++;
        }
        if(token.Type == MdTokenType.Italic)
            currentPosition += 1;
        if (token.Type == MdTokenType.Text)
        {
            token.InsertContent(0, '_');
        }
    }

    private void FillEscapingToken(MdToken token)
    {
        currentPosition += 1;
        while (Input[currentPosition] != '\\')
        {
            token.AddContent(Input[currentPosition]);       
        }
    }
}