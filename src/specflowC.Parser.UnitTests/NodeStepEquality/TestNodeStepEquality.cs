using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;

namespace specflowC.Parser.UnitTests.NodeStepEquality
{
    [TestClass]
    public class TestNodeStepEquality
    {
        [TestMethod]
        public void NodesWithTableArgumentsAreEqual()
        {
            NodeStep step1 = new NodeStep();
            step1.Name = "my step";
            step1.Rows.Add(new string[] {
                "| a | b | c |"
            });

            NodeStep step2 = new NodeStep();
            step2.Name = "my step";
            step2.Rows.Add(new string[] {
                "| a | b | c |"
            });

            Assert.IsTrue(step1.Equals(step2));
        }
    }
}
