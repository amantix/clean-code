using System.Text;

namespace MarkdownDraft;

class Program
{
    static void Main(string[] args)
    {
        string input = "__dadawdaw _wdwadawd_ wdadawdawdawd__";

        var result = Parse(input);
        foreach (var VARIABLE in result)
        {
            Console.WriteLine($"{VARIABLE.Type}: {VARIABLE.StartIndex} - {VARIABLE.EndIndex}");
            foreach (var token in VARIABLE.InsideTokens)
            {
                Console.WriteLine($"\t{token.Type}: {token.StartIndex} - {token.EndIndex}");
            }

            Console.WriteLine("=================================================");
        }
    }

    public static List<Token> Parse(string str)
    {
        var splittedString = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var listOfSpecialSymbols = new List<SpecialSymbol>();
        Stack<SpecialSymbol> openSymbolsStack = new Stack<SpecialSymbol>();
        var tokens = new List<Token>();

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '#')
            {
                /*if (isHeaderOpened)
                list.Add(new Token()
                {

                });
                isHeaderOpened = true;*/
                // func();
                continue;
            }

            if (str[i] == '\n')
            {
                // isHeaderOpened = false;
                // Добавить в стек закрывающую решетку
            }

            if (i < str.Length - 1 && str.Substring(i, 2) == "__")
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Bold, Index = i, Length = 2 });
                ++i;
                continue;
            }

            if (str[i] == '_')
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Italics, Index = i, Length = 1 });
            }
        }
        
        
        for (int i = 0; i < listOfSpecialSymbols.Count; i++)
        {
            var symbol = listOfSpecialSymbols[i];

            // Если это открывающий символ
            if (IsOpeningSymbol(symbol, openSymbolsStack))
            {
                openSymbolsStack.Push(symbol);
            }
            else
            {
                // По сути если мы попали в это ветвление, то открывающий тег точно есть
                // по этому ищем его и удаляем вложенность в закрывающемся теге
                // Закрывающий символ — ищем пару
                //
                // // ВОТ ТУТ НУЖНО ИЗМЕНИТЬ ЛОГИКУ НАХОЖДЕНИЯ ОТКРЫВАЮЩЕГОСЯ 
                
                // var openingSymbol = openSymbolsStack.Pop();
                SpecialSymbol openingSymbol;
                
                // По сути если внутри пары тегов, с которыми мы тут работаем, если
                // и были какие-то теги, то они уже убрались из стека
                // по этому с чистой душой можем делать .Pop()
                while ((openingSymbol = openSymbolsStack.Pop()).Type != symbol.Type);
                
                var newToken = new Token
                {
                    StartIndex = openingSymbol.Index,
                    EndIndex = symbol.Index + symbol.Length - 1,
                    Type = symbol.Type,
                    InsideTokens = ExtractInsideTokens(openingSymbol.Index, symbol.Index, tokens)
                };

                // Остается одна проблема
                // Вложенные в другие теги теги все равно будут находиться в tokens
                // Поэтому лучше их как-то удалить,
                // сделаю это в методе ExtractInsideTokens
                tokens.Add(newToken);
            }
        }


        return tokens;
    }
    
    private static bool IsOpeningSymbol(SpecialSymbol symbol, Stack<SpecialSymbol> stack)
    {
        // Определяем, открывающий ли это символ, в зависимости от контекста
        // Например, можно проверять, что нет открытой пары для символа этого типа в стеке
        bool openedTagBefore = false;

        foreach (var element in stack)
        {
            if (element.Type == symbol.Type)
            {
                openedTagBefore = true;
            }
        }
        
        // Проверяем, что стек может быть нулевым, до этого нигде этот открывающийся стек не встречался
        // Тогда он точно открывающий
        return  !openedTagBefore;
    }

    private static List<Token> ExtractInsideTokens(int startIndex, int endIndex, List<Token> tokens)
    {
        // Извлекаем вложенные токены, которые лежат внутри текущего токена
        List<Token> resultToReturn = new List<Token>();
        // Мы получили в переменную выше токены, которые нужно засунуть
        // в InsideTokens
        // Теперь нужно их удалить из главного массива tokens
        foreach (var token in tokens.ToList())
        {
            if (token.StartIndex > startIndex && token.StartIndex < endIndex)
            {
                resultToReturn.Add(token);
                tokens.Remove(token);
            }
        }

        return resultToReturn;
    }
}