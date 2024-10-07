﻿using System.Security.AccessControl;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class Token
    {
        public List<TagPosition> TagPositions { get; set; } = new List<TagPosition>();
        public string Content { get; set; } = string.Empty;

        public Token(string content)
        {
            Content = content;
        }
    }
}
