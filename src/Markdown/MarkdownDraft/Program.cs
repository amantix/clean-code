using System.Text;

namespace MarkdownDraft;

class Program
{
    static void Main(string[] args)
    {
        // TODO: Сделать так, чтоб внутри одинарных не работали двойные
        // для этого в enum'е TokenType я специально иерархически типы токенов выстроил
        // Сделаем отдельный метод ValidateTokens или чет типо того
        //
        // TODO: Пробелы между словами всегда должны быть одинарными 
        // 
        // TODO: Также надо предусмотреть, что я же не просто так указал в структуре
        // SpecialSymbol свойство Length, а 
        string str1 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str2 = "_dadawdaw __wdwadawd__ wdadawdawdawd_";
        string str3 = "#__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str4 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str5 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str6 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";

        var result = Parse(str3);
        Console.WriteLine(str3.Length);
        foreach (var VARIABLE in result)
        {
            Console.WriteLine($"{VARIABLE.Type}: {VARIABLE.StartIndex} - {VARIABLE.EndIndex}");
            foreach (var token in VARIABLE.InsideTokens)
            {
                Console.WriteLine($"\t{token.Type}: {token.StartIndex} - {token.EndIndex}");
                foreach (var tkn in token.InsideTokens)
                {
                    Console.WriteLine($"\t\t{tkn.Type}: {tkn.StartIndex} - {tkn.EndIndex}");
                
                }
            }

            Console.WriteLine("=================================================");
        }
    }
    
    public static List<Token> Parse(string str)
    {
        var listOfSpecialSymbols = new List<SpecialSymbol>();
        Stack<SpecialSymbol> openSymbolsStack = new Stack<SpecialSymbol>();
        var tokens = new List<Token>();
        bool isOpenedHeader = false;

        for (int i = 0; i < str.Length; i++)
        {
            if (i < str.Length &&  str[i] == '#')
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Header, Index = i, TagLength = 1, IsPairedTag = false});
                isOpenedHeader = true;
                ++i;
            }
            
            // i > 0 потому что будем считать, что перенос на новую строку
            // будет считаться концом header'а
            // а header будет кончаться перед переносом на новую строку
            // хотя не, во избежании ситуации "#_text_\n" лучше оставлю последний символ
            // header'а на самом переносе, а то потом проблемы с поиском вложенных могут возникнуть:
            // if (token.StartIndex > startIndex && token.StartIndex < endIndex)
            // Внимание на строгое сравнение
            // А нет, походу придется сделать все-таки до новой строки,
            // ведь может возникнуть ситуация "# text", где header должен работать, хотя 
            // переноса на новую строку (aka закрывающего тега) не было
            // Поэтому теперь закр. тегом будет считать символ до переноса на новую строку
            // и конец строки, если header был открыт
            if (i < str.Length && (str[i] == '\n' || i == str.Length - 1) && isOpenedHeader)
            {
                if (str[i] == '\n')
                {
                    listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Header, Index = i, TagLength = 0, IsPairedTag = false });
                    isOpenedHeader = false;
                    continue;
                }
                
                if (i == str.Length - 1)
                {
                    listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Header, Index = i, TagLength = 1, IsPairedTag = false });
                    isOpenedHeader = false;
                }
            }

            if (i < str.Length - 1 && str.Substring(i, 2) == "__")
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Bold, Index = i, TagLength = 2, IsPairedTag = true });
                i += 2;
            }

            if (i < str.Length && str[i] == '_')
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Italics, Index = i, TagLength = 1, IsPairedTag = true });
                ++i;
            }

            // Надо будет по красивее сделать, а то повторение кода
            if (i >= str.Length - 1 && isOpenedHeader)
            {
                listOfSpecialSymbols.Add(new SpecialSymbol { Type = TokenType.Header, Index = str.Length - 1, TagLength = 1, IsPairedTag = false });
                isOpenedHeader = false;
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
                    EndIndex = symbol.Index + symbol.TagLength - 1,
                    Type = symbol.Type,
                    // Вот эти два свойства ниже понадобятся нам когда будем очищать
                    // Контент токенов от самих тегов
                    TagLength = openingSymbol.TagLength,
                    IsPairedTag = openingSymbol.IsPairedTag,
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
            // Сделал <=, чтоб корректно обрабатывать вложенность header'ов, которые ведут до конца строки
            // Пример "# header _ada_"
            if (token.StartIndex > startIndex && token.StartIndex <= endIndex)
            {
                resultToReturn.Add(token);
                tokens.Remove(token);
            }
        }

        return resultToReturn;
    }
}