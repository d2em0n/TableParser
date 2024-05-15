using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
    public static void Test(string input, string[] expectedResult)
    {
        var actualResult = FieldsParserTask.ParseLine(input);
        Assert.AreEqual(expectedResult.Length, actualResult.Count);
        for (int i = 0; i < expectedResult.Length; ++i)
        {
            Assert.AreEqual(expectedResult[i], actualResult[i].Value);
        }
    }

    // Скопируйте сюда метод с тестами из предыдущей задачи.
    [TestCase("text", new[] { "text" })]
    [TestCase("hello world", new[] { "hello", "world" })]
    [TestCase("\"a \'b\' \'c\' d\"", new[] { "a \'b\' \'c\' d" })]
    [TestCase("\'\"1\" \"2\" \"3\"\'", new[] { "\"1\" \"2\" \"3\"" })]
    [TestCase("\'\'", new[] { "" })]
    [TestCase(@"a""b c d e""", new[] { "a", "b c d e" })]
    [TestCase(@"""def", new[] { "def" })]
    [TestCase(@"'\\'", new[] { @"\" })]
    [TestCase(@" \\", new[] { @"\\" })]
    [TestCase(@"  a  ", new[] { "a" })]
    [TestCase(@"'\'\''", new[] { "\'\'" })]
    [TestCase(@"""\""\""", new[] { "\"\"" })]
    [TestCase("", new string[0])]
    [TestCase(@"'a'b", new[] { "a", "b" })]
    [TestCase(@"""a ", new[] { "a " })]

    public static void RunTests(string input, string[] expectedOutput)
    {
        // Тело метода изменять не нужно
        Test(input, expectedOutput);
    }
}

public class FieldsParserTask
{
    // При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
    // Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
    public static List<Token> ParseLine(string line)
    {
        var tokens = new List<Token>();
        var currentIndex = FindToken(line, 0);
        while (currentIndex < line.Length)
        {
            Token token;
            if (line[currentIndex] == '\'' || line[currentIndex] == '\"') token = ReadQuotedField(line, currentIndex);
            else token = ReadField(line, currentIndex);
            tokens.Add(token);
            currentIndex = token.GetIndexNextToToken();
            currentIndex = FindToken(line, currentIndex);
        }

        return tokens;
    }

    private static int FindToken(string line, int startIndex)
    {
        var i = startIndex;
        while (i < line.Length)
        {
            if (line[i] == ' ') i++;
            else break;
        }
        return i;
    }

    private static Token ReadField(string line, int startIndex)
    {
        var tokenValue = new StringBuilder();
        tokenValue.Append(line[startIndex]);
        var i = startIndex + 1;
        while (i < line.Length)
        {
            if ((line[i] == ' ') || (line[i] == '\"') || (line[i] == '\'')) break;
            else
            {
                tokenValue.Append(line[i]);
                i++;
            }
        }
        return new Token(tokenValue.ToString(), startIndex, i - startIndex);
    }

    public static Token ReadQuotedField(string line, int startIndex)
    {
        return QuotedFieldTask.ReadQuotedField(line, startIndex);
    }
}