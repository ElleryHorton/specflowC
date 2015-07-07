using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    public static class AssertExt
    {
        public static void ContentsOfStringArray(string[] stringExpected, string[] stringActual)
        {
            Assert.AreEqual(stringExpected.Length, stringActual.Length, "String arrays are not the same length");

            int stringLength = stringExpected.Length;

            string expected;
            string actual;

            for (int i = 0; i < stringLength; i++)
            {
                expected = stringExpected[i].ToString();
                actual = stringActual[i].ToString();

                Assert.AreEqual(expected, actual, "Line number: " + i);
            }
        }
    }
}