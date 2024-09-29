using System.Text;

namespace MarkdownDraft;

class Program
{
    static void Main(string[] args)
    {
        // ВАЖНО
        // Когда будем делать функцию валидации парсинга, нужно объединить несколько подряд идующих
        // токенов текст в один токен
        string str1 = "_dawdawdadwad_";
        string str2 = "_dawdawdadwad_  fadawdwa";
        string str3 = "_dawdawdadwad_ ";
        string str4 = "_dawdawdadwad";
        string str5 = "_";

        foreach (var VARIABLE in Parse(str4))
        {
            Console.Write($"({VARIABLE.Type}: {VARIABLE.Content})  |  ");
        }
        
        
    }

    public static List<Token> Parse(string str)
    {

        // Как будто бы кстати очищать 
        // Token curToken = new Token();
        // необязательно
        List<Token> tokens = new List<Token>();
        
        var endIndex = str.Length - 1;
        
        var curString = new StringBuilder();
        var endOfToken = true;
        Token curToken = new Token();
        
        for (int i = 0; i <= endIndex; ++i)
        {
            if (str[i] == '#')
            {
                HandleHeader(ref i);
            }

            if (i != endIndex && str[i] == '_' && str[i + 1] == '_')
            {
                HandleBold(ref i);
            }
            
            if (str[i] == '_')
            {
                HandleTtalics(ref i);
            }
            
            // Записываем простые символы, которые не являются никак выделенными
            if (i < endIndex)
                curString.Append(str[i]);
            else if (i == endIndex)
            {
                // Если остались символы в конце строки после курсива
                curString.Append(str[i]);
                curToken.Type = TokenType.Text;
                curToken.Content = curString.ToString();
                tokens.Add(curToken);
            }
        }

        void HandleHeader(ref int i)
        {
            // Если перед встречей с заголовком был простой текст
            if (curString.Length > 0)
            {
                curToken.Type = TokenType.Text;
                curToken.Content = curString.ToString();
                tokens.Add(curToken);
                // Очищаем
                curToken = new Token();
                curString = curString.Clear();
            }

            endOfToken = false;
            int localIndex = i + 1;

            while (localIndex <= endIndex)
            {
                if (str[localIndex] == '\n')
                {
                    curToken.Type = TokenType.Header;
                    curToken.Content = curString.ToString();
                    endOfToken = true;
                    break;
                }
                    
                curString.Append(str[localIndex]);
                ++localIndex;
            }
                
            if (!endOfToken)
            {
                curToken.Type = TokenType.Header;
                curToken.Content = curString.ToString();
                endOfToken = true;
            }
                
            tokens.Add(curToken);
            curToken = new Token();
            curString = curString.Clear();
            endOfToken = true;
        }

        void HandleBold(ref int i)
        {
            
        }
        
        void HandleTtalics(ref int i) // Обработка потенциального курсива ("_")
        {
            
                // Если перед встречей с курсивом был простой текст
                if (curString.Length > 0)
                {
                    curToken.Type = TokenType.Text;
                    curToken.Content = curString.ToString();
                    tokens.Add(curToken);
                    // Очищаем
                    curToken = new Token();
                    curString = curString.Clear();
                }
                
                // Определяет, насколько нужно перепрыгнуть вперед после окончания парсинга курсива
                int step = 0;
                int localIndex = i + 1; // Чтобы начинать добавлять в curString уже после '_'
                endOfToken = false; // Чтоб проверить по итогу токен закрылся или нет
                
                while (localIndex <= endIndex && str[localIndex] != ' ')
                {
                    if (str[localIndex] == '_')
                    {
                        // Случай когда "... __ ..."
                        // ВАЖНО
                        // В будущем нужно будет пересмотреть этот участок
                        // Либо проводить проверку на if (str[i:i+1] == "__" до
                        // if (str[i] == '_')
                        if (localIndex - i == 1)
                        {
                            step = 1; // Так как след буква - пробел, ее нужно будет положить в список токенов как текст
                            curString.Append("__");
                            curToken.Type = TokenType.Text;
                            curToken.Content = curString.ToString();
                            endOfToken = true;
                            break;
                        }
                        
                        // Случай когда "... _слово_..." или "... _слово_ ..."
                        curToken.Type = TokenType.Italics;
                        curToken.Content = curString.ToString();
                        endOfToken = true;
                        step = 1;
                        break;
                    }   
                    
                    curString.Append(str[localIndex]);
                    ++localIndex;
                }

                // Случай когда "... _слово ..." или "... _слово" 
                // Незакрытый тег короче
                if (!endOfToken)
                {
                    curToken.Type = TokenType.Text;
                    curToken.Content = $"_{curString}";
                    // Если дело в пробеле, то step = 0
                    // Если строка и вовсе закончилась, то в принципе специально
                    // перепрыгнем за границы, чтоб потом на проверке
                    // никакой символ не добавился
                    step = (localIndex == endIndex) ? 1 : 0;
                    endOfToken = true;
                }

                tokens.Add(curToken);
                curToken = new Token();
                curString = curString.Clear();
                endOfToken = true;
                // Потому что текущее слово закончилось
                i = localIndex + step;
        }
        
        return tokens;
    }

    public static void ValidateParsedText(List<Token> tokens)
    {
        var currentText = new StringBuilder();
        for (int i = tokens.Count - 1; i >= 0; --i)
        {
            while (i >= 0 && tokens[i].Type == TokenType.Text)
            {
                currentText.Insert(0, tokens[i].Content);
                tokens.RemoveAt(i);
                --i;
            }
            tokens.Insert(i + 1, new Token{ Type = TokenType.Text, Content = currentText.ToString() });
        }
    }
}