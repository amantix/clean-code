using Xunit;
using MarkdownProcessorLib;

namespace MarkdownProcessor.Tests;
public class MarkdownProcessorTests
{
    [Fact]
    public void TestItalic()
    {
        
        var processor = new Md(); 

        string result = processor.Render("Это _курсив_ текст.");
       
        Assert.Equal("Это <em>курсив</em> текст.", result);
    }

    [Fact]
    public void TestBold()
    {
        var processor = new Md();

        string result = processor.Render("Это __полужирный__ текст.");

        Assert.Equal("Это <strong>полужирный</strong> текст.", result);
    }

    [Fact]
    public void TestEscapedUnderscore()
    {
        var processor = new Md();

        string result = processor.Render("Это \\_не курсив\\_.");

        Assert.Equal("Это _не курсив_.", result);
    }

    [Fact]
    public void TestNestedItalicInBold()
    {
        var processor = new Md();

        string result = processor.Render("Это __вложенный _курсив_ текст__.");

        Assert.Equal("Это <strong>вложенный <em>курсив</em> текст</strong>.", result);
    }

    
    [Fact]
    public void TestItalicWithDigits()
    {
        var processor = new Md();

        string result = processor.Render("Это _не_ должно быть курсивом: цифры_12_3.");

        Assert.Equal("Это <em>не</em> должно быть курсивом: цифры_12_3.", result);
    }

    [Fact]
    public void TestHeading()
    {
        var processor = new Md();

        string result = processor.Render("# Это заголовок");

        Assert.Equal("<h1>Это заголовок</h1>", result);
    }

    [Fact]
    public void TestHeadingWithBoldAndItalic()
    {
        var processor = new Md();

        string result = processor.Render("# Заголовок __с _разными_ символами__");

        Assert.Equal("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", result);
    }

    [Fact]
    public void TestEscapedCharactersRemain()
    {
        var processor = new Md();

        string result = processor.Render("Здесь сим\\волы экранирования\\ \\должны остаться.\\");

        Assert.Equal("Здесь сим\\волы экранирования\\ \\должны остаться.", result);
    }

    [Fact]
    public void TestEscapedBackslash()
    {
        var processor = new Md();

        string result = processor.Render("\\\\_вот это будет выделено тегом_");

        Assert.Equal("<em>вот это будет выделено тегом</em>", result);
    }

    [Fact]
    public void TestBoldInsideItalicDoesNotWork()
    {
        var processor = new Md();

        string result = processor.Render("_одинарное __двойное__ не_ работает.");

        Assert.Equal("<em>одинарное __двойное__ не</em> работает.", result);
    }

    [Fact]
    public void TestNoItalicBetweenDifferentWords()
    {
        var processor = new Md();

        string result = processor.Render("выделение в ра_зных сл_овах не работает.");

        Assert.Equal("выделение в ра_зных сл_овах не работает.", result);
    }

    [Fact]
    public void TestUnpairedUnderscoreNoEffect()
    {
        var processor = new Md();

        string result = processor.Render("__Непарные_ символы не считаются выделением.");

        Assert.Equal("__Непарные_ символы не считаются выделением.", result);
    }

    [Fact]
    public void TestUnderscoreShouldHaveNonSpaceCharacterBeforeOpening()
    {
        var processor = new Md();

        string result = processor.Render("эти_ подчерки_ не считаются выделением.");

        Assert.Equal("эти_ подчерки_ не считаются выделением.", result);
    }

    [Fact]
    public void TestUnderscoreShouldHaveNonSpaceCharacterAfterClosing()
    {
        var processor = new Md();

        string result = processor.Render("эти _подчерки _не считаются окончанием выделения.");

        Assert.Equal("эти _подчерки _не считаются окончанием выделения.", result);
    }

    [Fact]
    public void TestIntersectionOfBoldAndItalicIsIgnored()
    {
        var processor = new Md();

        string result = processor.Render("__пересечение _двойных__ и одинарных_ подчерков не считается выделением.");

        Assert.Equal("__пересечение _двойных__ и одинарных_ подчерков не считается выделением.", result);
    }

    [Fact]
    public void TestEmptyStringInsideUnderscores()
    {
        var processor = new Md();

        string result = processor.Render("Если внутри подчерков пустая строка ____, то они остаются символами подчерка.");

        Assert.Equal("Если внутри подчерков пустая строка ____, то они остаются символами подчерка.", result);
    }

    [Fact]
    public void TestItalicInBeginningMiddleEnd()
    {
        var processor = new Md();

        string result = processor.Render("и в _нач_але, в сер_еди_не и в кон_це_.");

        Assert.Equal("и в <em>нач</em>але, в сер<em>еди</em>не и в кон<em>це</em>.", result);
    }

    [Fact]
    public void TestEmphasisWithSingleUnderscore()
    {

        var processor = new Md();

        string result = processor.Render("_Яблоко __апельсин_");

        Assert.Equal("<em>Яблоко __апельсин</em>", result);
    }

    [Fact]
    public void TestEmphasisWithDoubleUnderscore()
    {

        var processor = new Md();

        string result = processor.Render("__Яблоко _апельсин__");

        Assert.Equal("<strong>Яблоко _апельсин</strong>", result);
    }





}
