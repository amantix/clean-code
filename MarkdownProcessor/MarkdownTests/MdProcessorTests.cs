namespace MarkdownLibrary.Tests;

public class MdProcessorTests
{

    private readonly MarkdownProcessor _processor;

    public MdProcessorTests()
    {
        _processor = new MarkdownProcessor();
    }


    [Fact]
    public void SingleCharp_ShouldConvertToHeader()
    {
        var input = "#Header text";
        var expected = "<h1>Header text</h1>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DoubleUnderscore_ShouldConvertToStrong()
    {
        // Arrange
        var input = "__bold text__";
        var expected = "<strong>bold text</strong>";

        string result = _processor.ConvertToHtml(input);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SingleUnderscore_ShouldConvertToEmphasis()
    {
        var input = "_italic text_";
        var expected = "<em>italic text</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedHeader_ShouldNotConvert()
    {
        string input = @"\#��� ��� ���������";
        string expected = @"#��� ��� ���������";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedItalicText_ShouldNotConvert()
    {
        string input = @"\_��� ���\_ �� ������ ���������� ����� <em>.";
        string expected = "_��� ���_ �� ������ ���������� ����� <em>.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedBoldText_ShouldNotConvert()
    {
        string input = @"\_\_��� ���\_\_ �� ������ ���������� ����� <strong>.";
        string expected = "__��� ���__ �� ������ ���������� ����� <strong>.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_Correctly()
    {
        string input = @"������ ������������� �������� �� ����������, ������ ���� ���������� ���-��. ����� ���\���� �������������\ \������ ��������.\";
        string expected = @"������ ������������� �������� �� ����������, ������ ���� ���������� ���-��. ����� ���\���� �������������\ \������ ��������.\";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_ShouldWorkCorrectly()
    {
        string input = @"������ ������������� ���� ����� ������������: \\_��� ��� ����� �������� �����_";
        string expected = @"������ ������������� ���� ����� ������������: \<em>��� ��� ����� �������� �����</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NestedEmphasisAndStrong_ShouldWorkCorrectly()
    {
        string input = "__������� ��������� _���������_ ����__ ��������.";
        string expected = "<strong>������� ��������� <em>���������</em> ����</strong> ��������.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SingleUnderscoreInsideDoubleUnderscore_ShouldNotWork()
    {
        string input = "_��������� __�������__ ��_ ��������.";
        string expected = "<em>��������� __�������__ ��</em> ��������.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnderscoresInsideWordsWithNumbers_ShouldNotConvert()
    {
        string input = "����� c �������_12_3 �� ��������� ����������.";
        string expected = "����� c �������_12_3 �� ��������� ����������.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnderscoresInsideWords_ShouldConvert()
    {
        string input = "� � _���_���, � � ���_���_��, � � ���_��._";
        string expected = "� � <em>���</em>���, � � ���<em>���</em>��, � � ���<em>��.</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnmatchedSymbols_ShouldNotConvert()
    {
        string input = "__��������_ ������� �� ��������� ����������.";
        string expected = "__��������_ ������� �� ��������� ����������.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NoWhitespaceAfterUnderscore_ShouldNotConvert()
    {
        string input = "�� ����������, ����������� ���������, ������ ��������� ������������ ������. ����� ���_ ��������_ �� ��������� ����������";
        string expected = "�� ����������, ����������� ���������, ������ ��������� ������������ ������. ����� ���_ ��������_ �� ��������� ����������";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NoWhitespaceBeforeUnderscore_ShouldNotConvert()
    {
        string input = "��������, ������������� ���������, ������ ��������� �� ������������ ��������. ����� ��� _�������� _�� ��������� ���������� ��������� ";
        string expected = "��������, ������������� ���������, ������ ��������� �� ������������ ��������. ����� ��� _�������� _�� ��������� ���������� ��������� ";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CrossingUnderscores_ShouldNotConvert()
    {
        string input = "__����������� _�������__ � ���������_ ���������__.";
        string expected = "__����������� _�������__ � ���������_ ���������__.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EmptyStringBetweenUnderscores_ShouldRemainAsUnderscores()
    {
        string input = "���� ������ ��������� ������ ������ ____. ";
        string expected = "���� ������ ��������� ������ ������ ____. ";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void HeadingConversion_ShouldWork()
    {
        string input = "#��������� __� _�������_ ���������__";
        string expected = "<h1>��������� <strong>� <em>�������</em> ���������</strong></h1>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }
}

