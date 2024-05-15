using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
	[TestCase("''", 0, "", 2)]
	[TestCase("'a'", 0, "a", 3)]
    [TestCase("\"abc\"", 0, "abc", 5)]
    [TestCase("b \"a'\"", 2, "a'", 4)]
    [TestCase("'a'b", 0, "a", 3)]
    [TestCase("a'b'", 1, "b", 3)]
    [TestCase(@"'a\' b'", 0, "a' b", 7)]
    [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]
    [TestCase("''", 0, "", 2)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
	{
		var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
		Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
	}

	// Добавьте свои тесты
}

class QuotedFieldTask
{
	public static Token ReadQuotedField(string line, int startIndex)
	{
        var quot = line[startIndex];
        var tokenValue = new StringBuilder();
        var i = startIndex + 1;
        while (i < line.Length)
        {
            if (line[i] == quot)
            {
                i++;
                break;
            }

            if (line[i] == '\\')
            {
                tokenValue.Append(line[i + 1]);
                i += 2;
            }
            else
            {
                tokenValue.Append(line[i]);
                i++;
            }
        }
        return new Token(tokenValue.ToString(), startIndex, i - startIndex);
    }
}