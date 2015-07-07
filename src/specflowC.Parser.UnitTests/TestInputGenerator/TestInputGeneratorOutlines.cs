using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGeneratorOutlines
    {
        [TestMethod]
        public void InputGeneratorCreatesOneExamplesTable()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario Outline: my new scenario outline",
                "\tGiven I have a step",
                "",
                "Examples:",
                "\t| a | b | c |",
                "\t| 1 | 2 | 3 |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios.Count, "scenario count mismatch");
            Assert.IsNotNull(((NodeScenarioOutline)features[0].Scenarios[0]).Examples);
            Assert.AreEqual(2, ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows.Count, "examples row count mismatch");
            Assert.AreEqual("a", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][0]);
            Assert.AreEqual("b", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][1]);
            Assert.AreEqual("c", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][2]);
            Assert.AreEqual("1", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[1][0]);
            Assert.AreEqual("2", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[1][1]);
            Assert.AreEqual("3", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[1][2]);
        }

        [TestMethod]
        public void InputGeneratorExampleTableIsCaseSensitive()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario Outline: my new scenario outline",
                "\tGiven I have a step",
                "",
                "Examples:",
                "\t| abc | aBc | ABC |",
                "\t|  1  |  2  |  3  |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(2, ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows.Count, "examples row count mismatch");
            Assert.AreEqual("abc", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][0]);
            Assert.AreEqual("aBc", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][1]);
            Assert.AreEqual("ABC", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][2]);
        }

        [TestMethod]
        public void InputGeneratorExampleTableHandlesSpaces()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario Outline: my new scenario outline",
                "\tGiven I have a step",
                "",
                "Examples:",
                "\t| a bc | ab c | a b c |",
                "\t|  1   |  2   |  3    |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(2, ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows.Count, "examples row count mismatch");
            Assert.AreEqual("abc", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][0]);
            Assert.AreEqual("abc", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][1]);
            Assert.AreEqual("abc", ((NodeScenarioOutline)features[0].Scenarios[0]).Examples.Rows[0][2]);
        }

        [TestMethod]
        public void InputGeneratorParameterizesFromExampleTable()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario Outline: my new scenario outline",
                "\tGiven I have a step with example data '<a>'",
                "\tGiven I have a step with example data <b>",
                "",
                "Examples:",
                "\t| a | b | c |",
                "\t| 1 | 2 | 3 |"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.IsTrue(features[0].Scenarios[0].Steps[0].Parameters[0].IsFromExampleTable, "parameter not from example table");
            Assert.AreEqual("p0", features[0].Scenarios[0].Steps[0].Parameters[0].Name);
            Assert.AreEqual("a", features[0].Scenarios[0].Steps[0].Parameters[0].Value);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps[1].Parameters.Count, "parameter count mismatch");
            Assert.IsTrue(features[0].Scenarios[0].Steps[1].Parameters[0].IsFromExampleTable, "parameter not from example table");
            Assert.AreEqual("p0", features[0].Scenarios[0].Steps[1].Parameters[0].Name);
            Assert.AreEqual("b", features[0].Scenarios[0].Steps[1].Parameters[0].Value);
        }
    }
}