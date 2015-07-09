using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    public static class AssertExt
    {
        public static void ContentsOfStringArray(string[] stringArrayExpected, string[] stringArrayActual)
        {
            Assert.AreEqual(stringArrayExpected.Length, stringArrayActual.Length, "String arrays are not the same length");

            for (int i = 0; i < stringArrayExpected.Length; i++)
            {
                Assert.AreEqual(stringArrayExpected[i], stringArrayActual[i], "Line number: " + i);
            }
        }
    }
}