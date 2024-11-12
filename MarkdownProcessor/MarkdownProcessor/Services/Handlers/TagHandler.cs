using MarkdownProcessorLib.Models;

namespace MarkdownProcessorLib.Services.Handlers;

public class TagHandler
{
    public static void Handle(List<string> html,
        Stack<string> tagStack,
        Stack<int> tagIndexStack,
        Tag tag,
        ref string insideCharFlag,
        List<Block> blocks,
        ref int i)
    {
        if (tagStack.Count > 0 && insideCharFlag == tag.Type)
        {
            html.Add(tag.Value);
            insideCharFlag = "";
            CloseTags(html, tagStack, tagIndexStack);
        }


        else if (tagStack.Count >= 2 && tagStack.Reverse().First() == tag.Type)
        {
            HandleIntersectingTag(html, tagStack, tagIndexStack, tag, ref insideCharFlag);
        }

        else if (tagStack.Count >= 2 && tagStack.Reverse().First() != tag.Type && tag.Type == "strong")
        {
            HandleBoldInItalic(html, tagStack, tagIndexStack);
        }

        else if (tag.Type == "strong" && html.Count + 1 < blocks.Count && blocks[html.Count + 1].Value == "__")
        {
            html.Add("____");
            i++;
        }

        else if (tagStack.Count > 0 && tagStack.Peek() == "!")
        {
            tagStack.Pop();
            html.Add(tag.Value);
        }
        else if (!tagStack.Contains(tag.Type) && blocks[html.Count + 1].Value.StartsWith(" "))
        {

            html.Add(tag.Value);
            tagStack.Push("!");
        }


        else if (tagStack.Contains(tag.Type) && blocks[html.Count - 1].Value.EndsWith(" "))
        {

            html.Add(tag.Value);
            tagStack.Push("!");
        }

        else if (tagStack.Count > 0 && tagStack.Peek() == tag.Type)
        {
            CloseTag(html, tagStack, tagIndexStack, tag);
        }

        else
        {
            OpenTag(html, tagStack, tagIndexStack, tag);
        }

    }

    private static void OpenTag(
        List<string> html,
        Stack<string> tagStack,
        Stack<int> tagIndexStack,
        Tag tag)
    {
        html.Add($"<{tag.Type}>");
        tagStack.Push(tag.Type);
        tagIndexStack.Push(html.Count - 1);
    }

    private static void CloseTag(
        List<string> html,
        Stack<string> tagStack,
        Stack<int> tagIndexStack,
        Tag tag)
    {
        html.Add($"</{tag.Type}>");
        tagStack.Pop();
        tagIndexStack.Pop();
    }

    public static void CloseTags(
        List<string> html,
        Stack<string> tagStack,
        Stack<int> tagIndexStack)
    {
        while (tagIndexStack.Count > 0)
        {
            if (tagStack.Peek() == "!")
            {
                tagStack.Pop();
                continue;
            }

            var index = tagIndexStack.Pop();
            var type = tagStack.Pop();

            if (type == "!") continue;
            html[index] = type == "strong" ? "__" : "_";

        }
    }

    private static void HandleIntersectingTag(
        List<string> html,
        Stack<string> tagStack,
        Stack<int> tagIndexStack,
        Tag tag,
        ref string insideCharFlag)
    {
        insideCharFlag = tagStack.Pop();
        tagStack.Push(tag.Type);
        html[tagIndexStack.Pop()] = insideCharFlag == "strong" ? "__" : "_";
        html.Add($"</{tag.Type}>");
        tagIndexStack.Push(html.Count - 1);
    }

    private static void HandleBoldInItalic(
       List<string> html,
       Stack<string> tagStack,
       Stack<int> tagIndexStack)
    {
        html[tagIndexStack.Pop()] = "__";
        tagStack.Pop();
        html.Add("__");
    }
}
