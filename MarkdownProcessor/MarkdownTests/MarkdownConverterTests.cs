using MarkdownRenderer;

namespace MarkdownTests;

public class MarkdownConverterTests
{
    private readonly TokensParser _tokensParser;
    private readonly MarkdownConverter _markdownConverter;

    public MarkdownConverterTests()
    {
        _tokensParser = new TokensParser();
        _markdownConverter = new MarkdownConverter(_tokensParser);
    }

    [Theory]
    [InlineData("��� __������__ �����.", "<div>��� <strong>������</strong> �����.</div>")]
    [InlineData("����� __������__ � __������__", "<div>����� <strong>������</strong> � <strong>������</strong></div>")]
    [InlineData("__������__ � ������.", "<div><strong>������</strong> � ������.</div>")]
    [InlineData("� ����� __������__", "<div>� ����� <strong>������</strong></div>")]
    [InlineData("��� __�������� ������", "<div>��� __�������� ������</div>")]
    [InlineData("������ � __���__���", "<div>������ � <strong>���</strong>���</div>")]
    [InlineData("������ � ���__���__��", "<div>������ � ���<strong>���</strong>��</div>")]
    [InlineData("������ � ���__��__", "<div>������ � ���<strong>��</strong></div>")]
    [InlineData("��� __�������� __���������__ ���������� ������ �� ������ �����", "<div>��� __�������� <strong>���������</strong> ���������� ������ �� ������ �����</div>")]
    [InlineData("������ c __���__��1�� ������", "<div>������ c __���__��1�� ������</div>")]
    [InlineData("������ c __�����1��__ �������", "<div>������ c <strong>�����1��</strong> �������</div>")]
    [InlineData("������ c __����__�1��__ � ������ � �������", "<div>������ c <strong>����__�1��</strong> � ������ � �������</div>")]
    public void ConvertToHtml_ShouldConvertBoldTag(string markdownText, string expectedHtml)
    {
        string result = _markdownConverter.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, result);
    }

    [Theory]
    [InlineData("��� _���������_ �����.", "<div>��� <em>���������</em> �����.</div>")]
    [InlineData("����� _���������_ � _���������_", "<div>����� <em>���������</em> � <em>���������</em></div>")]
    [InlineData("_���������_ � ������.", "<div><em>���������</em> � ������.</div>")]
    [InlineData("� ����� _���������_", "<div>� ����� <em>���������</em></div>")]
    [InlineData("��� _�������� ������", "<div>��� _�������� ������</div>")]
    [InlineData("������ � _���_���", "<div>������ � <em>���</em>���</div>")]
    [InlineData("������ � ���_���_��", "<div>������ � ���<em>���</em>��</div>")]
    [InlineData("������ � ���_��_", "<div>������ � ���<em>��</em></div>")]
    [InlineData("��� _�������� _���������_ ���������� ������ �� ������ �����", "<div>��� _�������� <em>���������</em> ���������� ������ �� ������ �����</div>")]
    [InlineData("������ c _���_��1�� ������", "<div>������ c _���_��1�� ������</div>")]
    [InlineData("������ c _�����1��_ �������", "<div>������ c <em>�����1��</em> �������</div>")]
    [InlineData("������ c _����_�1��_ � ������ � �������", "<div>������ c <em>����_�1��</em> � ������ � �������</div>")]
    public void ConvertToHtml_ShouldConvertItalicTag(string markdownText, string expectedHtml)
    {
        string result = _markdownConverter.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, result);
    }

    [Theory]
    [InlineData("__������ �������� ��������� _���������_ ����__", "<div><strong>������ �������� ��������� <em>���������</em> ����</strong></div>")]
    [InlineData("_������ ���������� __�������__ �� ��������_", "<div><em>������ ���������� __�������__ �� ��������</em></div>")]
    [InlineData("���_ ��������_ �� ��������� ����������", "<div>���_ ��������_ �� ��������� ����������</div>")]
    [InlineData("� ������ __����������� _�������__ � ���������_ ��������� �� ���� �� ��� �� ��������� ����������", "<div>� ������ __����������� _�������__ � ���������_ ��������� �� ���� �� ��� �� ��������� ����������</div>")]
    [InlineData("� �� �� ����� ��������� � ��_���� ��_���� �� ��������", "<div>� �� �� ����� ��������� � ��_���� ��_���� �� ��������</div>")]
    [InlineData("�������� ������ ������ ____ (��� ���������)", "<div>�������� ������ ������ ____ (��� ���������)</div>")]
    [InlineData("�������� ������ ������ __ (��� ���������)", "<div>�������� ������ ������ __ (��� ���������)</div>")]
    public void ConvertToHtml_ShouldHandleSpecialCases(string markdownText, string expectedHtml)
    {
        string result = _markdownConverter.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, result);
    }

    [Theory]
    [InlineData("# ���������", "<div><h1>���������</h1></div>")]
    [InlineData("# ��������� � __������__ �������", "<div><h1>��������� � <strong>������</strong> �������</h1></div>")]
    [InlineData("# ��������� � _��������_", "<div><h1>��������� � <em>��������</em></h1></div>")]
    [InlineData("# ��������� � __������__ � _��������_", "<div><h1>��������� � <strong>������</strong> � <em>��������</em></h1></div>")]
    public void ConvertToHtml_ShouldConvertHeaderTag(string markdownText, string expectedHtml)
    {
        string result = _markdownConverter.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, result);
    }

    [Theory]
    [InlineData(@"\_�� ������ � ���������_", @"<div>_�� ������ � ���������_</div>")]
    [InlineData(@"\_��������_ � �����", @"<div>_��������_ � �����</div>")]
    [InlineData(@"\__�� ������ � ���������__", @"<div>__�� ������ � ���������__</div>")]
    [InlineData(@"\__��������__ � �����", @"<div>__��������__ � �����</div>")]
    [InlineData(@"\\_������ � ���������_", @"<div><em>������ � ���������</em></div>")]
    [InlineData(@"\\__������ � ���������__", @"<div><strong>������ � ���������</strong></div>")]
    [InlineData(@"\\_������_ � �����", @"<div><em>������</em> � �����</div>")]
    [InlineData(@"\\__������__ � �����", @"<div><strong>������</strong> � �����</div>")]
    [InlineData(@"��\\_����_ ������ �����", @"<div>��<em>����</em> ������ �����</div>")]
    [InlineData(@"��\\__����__ ������ �����", @"<div>��<strong>����</strong> ������ �����</div>")]
    public void ConvertToHtml_ShouldEscapeBackslash(string markdownText, string expectedHtml)
    {
        string result = _markdownConverter.ConvertToHtml(markdownText);

        Assert.Equal(expectedHtml, result);
    }
}