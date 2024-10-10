namespace MarkdownTests;
using Markdown;
public class Tests
{
    private MdProcessor MdProcessor;
    private string? inputText;
    private string? expextedOutput;

    [SetUp]
    public void Setup()
    {
        MdProcessor = new MdProcessor();
        inputText = "";
        expextedOutput = "";
    }

    [Test]
    public void InputText_ShouldNotBeNull()
    {
        Assert.NotNull(inputText);
    }

}