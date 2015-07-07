using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGenerator
    {
        [TestMethod]
        public void InputGeneratorCreatesOneFeatureWithoutScenarios()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features.Count, "feature count mismatch");
            Assert.AreEqual(0, features[0].Scenarios.Count, "scenario count mismatch");
            Assert.AreEqual("MyNewFeature", features[0].Name);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneScenarioWithoutSteps()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("MyNewScenario", features[0].Scenarios[0].Name);
            Assert.AreEqual(0, features[0].Scenarios[0].Steps.Count, "step count mismatch");
        }

        [TestMethod]
        public void InputGeneratorCreatesOneScenarioOutlineWithoutSteps()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario Outline: my new scenario outline"
            };
            var features = generator.Load(contents);

            Assert.AreEqual("MyNewScenarioOutline", features[0].Scenarios[0].Name);
            Assert.AreEqual(0, features[0].Scenarios[0].Steps.Count, "step count mismatch");
        }
    }
}