using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGeneratorStepsTable
    {
        [TestMethod]
        public void InputGeneratorCreatesOneStepWithTable()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step:",
                "\t\t| a | b | c |",
                "\t\t| 1 | 2 | 3 |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("GivenIHaveAStep", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(2, features[0].Scenarios[0].Steps[0].Rows.Count, "table row count mismatch");
            Assert.AreEqual("a", features[0].Scenarios[0].Steps[0].Rows[0][0]);
            Assert.AreEqual("b", features[0].Scenarios[0].Steps[0].Rows[0][1]);
            Assert.AreEqual("c", features[0].Scenarios[0].Steps[0].Rows[0][2]);
            Assert.AreEqual("1", features[0].Scenarios[0].Steps[0].Rows[1][0]);
            Assert.AreEqual("2", features[0].Scenarios[0].Steps[0].Rows[1][1]);
            Assert.AreEqual("3", features[0].Scenarios[0].Steps[0].Rows[1][2]);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithOneParamaterAndTable()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with 'one' parameter and:",
                "\t\t| a | b | c |",
                "\t\t| 1 | 2 | 3 |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("GivenIHaveAStepWithParameterAnd", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("one", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
            Assert.AreEqual(2, features[0].Scenarios[0].Steps[0].Rows.Count, "table row count mismatch");
        }
    }
}