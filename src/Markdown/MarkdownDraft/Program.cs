using System.Diagnostics;
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
        //
        // TODO: Когда дойдем до рендера, убирать специальные символы (например "__") будем
        // ориентуруясь на то, парный ли тег (IsPairedTag) и какова длина специального символа (TagLength)
        string str1 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str2 = "_dadawdaw __wdwadawd__ wdadawdawdawd_";
        string str3 = "#__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str4 = "__text_text___";
        string str5 = "_italic_";
        string str6 = "__bold__";
        string str7 = "This is _italic_ text.";
        string str8 = "__bold _italic_ text__";
        string str9 = "__bold text_ with _italic_ text__";
        string str10 = "This is _italic text";
        string str11 = "";
        string str12 = "# Заголовок первого уровня\n\nЭто пример текста, который будет использоваться для тестирования парсера Markdown. Здесь мы можем использовать __жирный текст__, чтобы выделить важные слова, и _курсив_, чтобы сделать акцент на других аспектах.\n\n## Заголовок второго уровня\n\nВ этом разделе мы будем обсуждать различные аспекты парсинга. Например, мы можем смешивать __жирный__ и _курсивный_ текст, чтобы увидеть, как парсер справляется с разными форматами.\n\n### Заголовок третьего уровня\n\n1. Первый элемент списка\n2. Второй элемент списка с _курсивом_\n3. Третий элемент списка с __жирным текстом__ и _курсивом_\n\nТеперь давайте посмотрим на более сложные примеры:\n\nЭто текст с __жирным__ и _курсивом_, а также # заголовком. Следующий параграф будет содержать даже больше форматов.\n\n__Важно:__ __жирный текст__ должен быть правильно обработан, а _курсив_ не должен конфликтовать с __жирным__.\n\nЕсли вы прочитали этот текст, вы можете заметить, что # заголовок не должен быть затенен другим текстом. Например, __жирный текст__, который находится _между курсивом_ и заголовком.\n\n# Заключение\n\nМы надеемся, что этот текст помог вам протестировать ваш парсер. Убедитесь, что все форматы правильно обрабатываются. Если __жирный текст__ стоит в конце строки, а _курсив_ в начале, это не должно вызывать ошибок. # Заголовок должен оставаться отдельным и не смешиваться с другими форматами.\n\nТаким образом, тестируя производительность вашего метода, вы можете увидеть, как он справляется с обработкой длинного текста. Не забудьте протестировать его на различных вводных данных!";

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = Parse(str12);
        var res = stopwatch.Elapsed;
        stopwatch.Stop();
        Console.WriteLine(res);
        Console.WriteLine(str12.Length);
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

        RemoveInvalidTokens(tokens);
        
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

    // В этом методе удалим токены, которые не могут рендериться в текущем токене
    // Пример: _text__text__text_ => <em>text__text__text</em>
    // По сути самый длинный рекусривный вызов здесь может быть длиной в 3, 
    // тк иначе даже если будет __a __текст, который может еще суб-токену содержать__ a__,
    // то такой суб-массив просто удалиться, и не будет обрабатываться
    // NOTE: По сути здесь никогда не может быть nullRefEx,
    // тк как любому токену присуждается минимум пустой список
    // САМОЕ ГЛАВНОЕ - ВЫЗЫВАТЬ ЭТОТ МЕТОД ПЕРЕД FillTokensListsWithTextTokens
    private static void RemoveInvalidTokens(List<Token> tokens, TokenType currentTokenType = TokenType.Main)
    {
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
        
            if (token.Type >= currentTokenType)
            {
                tokens.RemoveAt(i); // Удаляем токен по индексу
            }
            else
            {
                // Рекурсивный вызов для вложенных токенов
                RemoveInvalidTokens(token.InsideTokens, token.Type);
            }
        }
    }

    // В этом месте заполним пустые символы, не занятые никакими токенами токенами текста
    // Пример: __text_text___ = Bold { Text, Italics },
    // потому что без него будет: Bold { Italics }
    private static void FillTokensListsWithTextTokens(Token token)
    {
        // __w_text_w__
        int textStartIndex = token.StartIndex + token.TagLength;
        
        for (int i = 0; i < token.InsideTokens.Count; i++)
        {
            var insideToken = token.InsideTokens[i];
            
            if (token.StartIndex > textStartIndex)
            {
                token.InsideTokens.Insert(i, new Token()
                {
                    StartIndex = textStartIndex,
                    EndIndex = insideToken.StartIndex - 1,
                    Type = TokenType.Text,
                    IsPairedTag = false,
                    TagLength = 0,
                });
            }

            textStartIndex = insideToken.EndIndex + 1;
        }
        
        // Обработка расстояния между последним токеном внутри и правой границей
        if (token.EndIndex - token.TagLength + 1 > textStartIndex)
        {
            token.InsideTokens.Add(new Token()
            {
                StartIndex = textStartIndex,
                EndIndex = token.EndIndex - token.TagLength,
                Type = TokenType.Text,
                IsPairedTag = false,
                TagLength = 0,
            });
        }
    }
}