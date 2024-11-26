using MarkdownLibrary;

public class MarkdownProcessorTests
{
    private readonly MarkdownProcessor _processor = new MarkdownProcessor();

    [Fact]
    public void Italics_ShouldBeConvertedToEmTag()
    {
        var input = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";
        var expected = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Bold_ShouldBeConvertedToStrongTag()
    {
        var input = "__Текст выделенный двумя символами__";
        var expected = "<strong>Текст выделенный двумя символами</strong>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_ShouldPreventFormatting()
    {
        var input = @"\_Экранированный текст\_ не должен выделяться курсивом";
        var expected = "_Экранированный текст_ не должен выделяться курсивом";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_ShouldBeVisibleIfNotUsedForFormatting()
    {
        var input = @"Здесь сим\\волы экранирования\\ \\должны остаться.";
        var expected = @"Здесь сим\волы экранирования\ \должны остаться.";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedUnderscore_ShouldConvertToItalics()
    {
        var input = @"\\_Это выделится курсивом_";
        var expected = "\\<em>Это выделится курсивом</em>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void BoldAndItalics_ShouldWorkTogether()
    {
        var input = "__Полужирный и _курсив_ внутри__";
        var expected = "<strong>Полужирный и <em>курсив</em> внутри</strong>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ItalicsInsideBold_ShouldNotWork()
    {
        var input = "_Курсив и __полужирный внутри__ не работают_";
        var expected = "<em>Курсив и __полужирный внутри__ не работают</em>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnderscoreInNumbers_ShouldNotBeFormatted()
    {
        var input = "Текст с цифрами_12_3";
        var expected = "Текст с цифрами_12_3";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ItalicsInTheMiddleOfWord_ShouldWork()
    {
        var input = "Выделение в _нач_але и сер_еди_не";
        var expected = "Выделение в <em>нач</em>але и сер<em>еди</em>не";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnpairedUnderscores_ShouldNotBeFormatted()
    {
        var input = "Это непарные_ символы";
        var expected = "Это непарные_ символы";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Heading_ShouldBeConvertedToH1Tag()
    {
        var input = "# Это заголовок";
        var expected = "<h1>Это заголовок</h1>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void HeadingWithFormatting_ShouldWork()
    {
        var input = "# Заголовок с _разными_ символами";
        var expected = "<h1>Заголовок с <em>разными</em> символами</h1>";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EmptyUnderscores_ShouldRemain()
    {
        var input = "____";
        var expected = "____";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void MixedBoldAndItalics_ShouldNotBeFormatted()
    {
        var input = "пересечение __двойных и одинарных_ подчерков";
        var expected = "пересечение __двойных и одинарных_ подчерков";

        var result = _processor.Process(input);

        Assert.Equal(expected, result);
    }
}