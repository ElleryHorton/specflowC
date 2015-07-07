using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGeneratorInvalidCharacters
    {
        [TestMethod]
        public void InputGeneratorHandlesInvalidCharactersInFeatureName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: :Abc123_-~!@#$%^&*()[].,"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("Abc123", features[0].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesInvalidCharactersInScenarioName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: :Abc123_-~!@#$%^&*()[].,"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("Abc123", features[0].Scenarios[0].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesInvalidCharactersInStepName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven :Abc123_-~!@#$%^&*()[].,",
            };
            var features = generator.Load(contents);

            Assert.AreEqual("GivenAbc123", features[0].Scenarios[0].Steps[0].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesLeadingNumbersInFeatureName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: 123:Abc123_-~!@#$%^&*()[].,"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("_23Abc123", features[0].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesLeadingNumbersInScenarioName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: 123:Abc123_-~!@#$%^&*()[].,"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("_23Abc123", features[0].Scenarios[0].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesLeadingNumbersInStepName()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven 123:Abc123_-~!@#$%^&*()[].,",
            };
            var features = generator.Load(contents);

            Assert.AreEqual("Given123Abc123", features[0].Scenarios[0].Steps[0].Name);
        }
    }
}