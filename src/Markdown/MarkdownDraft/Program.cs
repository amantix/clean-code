using System.Diagnostics;
using System.Text;

namespace MarkdownDraft;

class Program
{
    static void Main(string[] args)
    {
        // TODO: Сделать так, чтоб внутри одинарных не работали двойные для этого в enum'е TokenType я специально иерархически типы токенов выстроил. Сделаем отдельный метод ValidateTokens или чет типо того ( СДЕЛАНО )
        //
        // TODO: Пробелы между словами всегда должны быть одинарными 
        // if (str[i] == ' ' && i > 0 && str[i-1] == ' ', тогда скипаем текущие символ
        // 
        // TODO: Также надо предусмотреть, что я же не просто так указал в структуре SpecialSymbol свойство Length ( СДЕЛАНО )
        //
        // TODO: Когда дойдем до рендера, убирать специальные символы (например "__") будем ориентуруясь на то, парный ли тег (IsPairedTag) и какова длина специального символа (TagLength)
        //
        // TODO: Сделать так, чтобы теги при экранировании через "\" не превращались в теги. В случае "\_text\_" должно получиться после рендера "_text_", поэтому просто проигнорировать символы экранирования не получится, нужно будет их убрать в отдельном методе, который будет отвечать за фильтрацию текста, то есть туда же нужно будет добавить решение проблемы с пробелами.
        
        string str1 = "__dadawdaw _wdwadawd_ wdadawdawdawd__";
        string str2 = "_dadawdaw __wdwadawd__ wdadawdawdawd_";
        string str3 = "#__dadawdaw _wdwadawd_ wdadawdawdawd__     \n wdadawda";
        string str4 = "__text_text___";
        string str5 = "_italic_";
        string str6 = "__bold__";
        string str7 = "This is _italic_ text.";
        string str8 = "__bold _italic_ text__";
        string str9 = "__bold text_ with _italic_ text__";
        string str10 = "This is _italic text";
        string str11 = "#dwadadawd";
        string str12 = "# Заголовок первого уровня\n\nЭто пример текста, который будет использоваться для тестирования парсера Markdown. Здесь мы можем использовать __жирный текст__, чтобы выделить важные слова, и _курсив_, чтобы сделать акцент на других аспектах.\n\n## Заголовок второго уровня\n\nВ этом разделе мы будем обсуждать различные аспекты парсинга. Например, мы можем смешивать __жирный__ и _курсивный_ текст, чтобы увидеть, как парсер справляется с разными форматами.\n\n### Заголовок третьего уровня\n\n1. Первый элемент списка\n2. Второй элемент списка с _курсивом_\n3. Третий элемент списка с __жирным текстом__ и _курсивом_\n\nТеперь давайте посмотрим на более сложные примеры:\n\nЭто текст с __жирным__ и _курсивом_, а также # заголовком. Следующий параграф будет содержать даже больше форматов.\n\n__Важно:__ __жирный текст__ должен быть правильно обработан, а _курсив_ не должен конфликтовать с __жирным__.\n\nЕсли вы прочитали этот текст, вы можете заметить, что # заголовок не должен быть затенен другим текстом. Например, __жирный текст__, который находится _между курсивом_ и заголовком.\n\n# Заключение\n\nМы надеемся, что этот текст помог вам протестировать ваш парсер. Убедитесь, что все форматы правильно обрабатываются. Если __жирный текст__ стоит в конце строки, а _курсив_ в начале, это не должно вызывать ошибок. # Заголовок должен оставаться отдельным и не смешиваться с другими форматами.\n\nТаким образом, тестируя производительность вашего метода, вы можете увидеть, как он справляется с обработкой длинного текста. Не забудьте протестировать его на различных вводных данных!";
        string str13 = " __text__  _wdada_ ";
        string str14 =
            "# Заголовок первого уровня\n\nЭто пример длинного текста для тестирования парсера Markdown. В этом тексте мы используем _курсив_ и __жирный текст__ для проверки, как работает обработка таких тегов.\n\nТестирование парсера важно для того, чтобы убедиться, что все теги правильно интерпретируются. Например, _курсивный текст_ помогает выделять слова в предложениях, а __жирный текст__ используется для акцентирования на важных фразах.\n\n# Вложенные элементы\n\nКроме этого, необходимо проверять, как работает парсер с _вложенными_ тегами. Например, вот так: __жирный _и курсив_ в одном предложении__.\n\nТакже стоит протестировать парсер на больших объемах текста, чтобы убедиться, что __он не замедляется__ при обработке длинных строк. Оптимизация работы парсера очень важна, так как это напрямую влияет на производительность программы.\n\n# Тестирование производительности\n\nВот пример длинного текста с большим количеством тегов для проверки производительности:\n\n__Это жирный текст__, а вот _курсивный текст_, который используется для различных тестов. Продолжаем добавлять больше текста, чтобы создать нагрузку на парсер. Проверяем, как _курсив_ и __жирный__ текст взаимодействуют друг с другом.\n\nТеперь давайте добавим еще больше текста, чтобы убедиться, что парсер справляется с обработкой длинных строк. Мы будем добавлять теги _курсива_ и __жирного текста__, чтобы увидеть, как они работают вместе.\n\n__Жирный текст__ должен обрабатываться правильно, как и _курсивный текст_. Это важно, потому что парсер должен работать с множеством символов и тегов одновременно. Важно убедиться, что программа не начинает __замедляться__ или _падать_ на больших данных.\n\n# Заключение\n\nПарсеры Markdown используются в самых разных проектах, от генерации веб-страниц до редактирования текстов в блогах. Тестирование парсера на различных входных данных позволяет убедиться в его стабильности и производительности. __Жирный текст__ и _курсивный текст_ помогают создавать более выразительный контент, и важно, чтобы парсер корректно обрабатывал эти элементы.";

        string str15 =
            "__awdwa_dwdada__wddwa_";
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        
        var result = Parse(str15);
        

        stopwatch.Stop();
        var res = stopwatch.Elapsed;
        Console.WriteLine(res);
        
        foreach (var VARIABLE in result)
        {
            Console.WriteLine($"{VARIABLE.Type}: {VARIABLE.StartIndex} - {VARIABLE.EndIndex}");
            var arr1 = VARIABLE.InsideTokens;
            if (arr1 != null)
            {
                foreach (var token in VARIABLE.InsideTokens)
                {
                    Console.WriteLine($"\t{token.Type}: {token.StartIndex} - {token.EndIndex}: {token.InsideTokens?.Count}");
                    var arr2 = token.InsideTokens;
                    if (arr2 != null)
                    {
                        foreach (var tkn in token.InsideTokens)
                        {
                            Console.WriteLine($"\t\t{tkn.Type}: {tkn.StartIndex} - {tkn.EndIndex}");
                        }
                    }
                }
            }

            Console.WriteLine("=================================================");
        }
        Thread.Sleep(10000);
    }
    
    public static List<Token> Parse(string str)
    {
        var listOfSpecialSymbols = new List<SpecialSymbol>();
        Stack<SpecialSymbol> openSymbolsStack = new Stack<SpecialSymbol>();
        var mainToken = new Token()
        {
            Type = TokenType.Main,
            StartIndex = 0,
            EndIndex = str.Length - 1,
            IsPairedTag = true,
            TagLength = 0,
            InsideTokens = new List<Token>()
        }; 
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
                var ch = str[i];
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
                    InsideTokens = ExtractInsideTokens(openingSymbol.Index, symbol.Index, mainToken.InsideTokens)
                };

                // Остается одна проблема
                // Вложенные в другие теги теги все равно будут находиться в tokens
                // Поэтому лучше их как-то удалить,
                // сделаю это в методе ExtractInsideTokens
                mainToken.InsideTokens.Add(newToken);
            }
        }

        RemoveInvalidTokens(mainToken);
        FillTokensListsWithTextTokens(mainToken);
        
        return mainToken.InsideTokens;
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
    /*private static void RemoveInvalidTokens(List<Token> tokens, TokenType currentTokenType = TokenType.Main)
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
    }*/
    
    private static void RemoveInvalidTokens(Token token)
    {
        for (int i = token.InsideTokens.Count - 1; i >= 0; i--)
        {
            var insideToken = token.InsideTokens[i];
            
            if (insideToken.Type >= token.Type)
            {
                token.InsideTokens.RemoveAt(i); // Удаляем токен по индексу
            }
            else
            {
                // Рекурсивный вызов для вложенных токенов
                RemoveInvalidTokens(insideToken);
            }
        }
    }

    // В этом месте заполним пустые символы, не занятые никакими токенами токенами текста
    // Пример: __text_text___ = Bold { Text, Italics },
    // потому что без него будет: Bold { Italics }
    private static void FillTokensListsWithTextTokens(Token token)
    {
        foreach (var tokenInside in token.InsideTokens)
        {
            FillTokensListsWithTextTokens(tokenInside);
        }

        int textStartIndex = token.StartIndex + token.TagLength;
        
        for (int i = 0; i < token.InsideTokens.Count; i++)
        {
            var insideToken = token.InsideTokens[i];
        
            if (insideToken.StartIndex > textStartIndex)
            {
                token.InsideTokens.Insert(i, new Token()
                {
                    StartIndex = textStartIndex,
                    EndIndex = insideToken.StartIndex - 1,
                    Type = TokenType.Text,
                    IsPairedTag = false,
                    TagLength = 0,
                });
                ++i; // Перепрыгиваем только что созданный токен
            }

            textStartIndex = insideToken.EndIndex + 1;
        }
    
        // Обработка расстояния между последним токеном внутри и правой границей
        if (token.EndIndex - ((token.IsPairedTag) ? token.TagLength : 0) >= textStartIndex)
        {
            token.InsideTokens.Add(new Token()
            {
                StartIndex = textStartIndex,
                EndIndex = token.EndIndex - ((token.IsPairedTag) ? token.TagLength : 0),
                Type = TokenType.Text,
                IsPairedTag = false,
                TagLength = 0,
            });
        }
    }
}