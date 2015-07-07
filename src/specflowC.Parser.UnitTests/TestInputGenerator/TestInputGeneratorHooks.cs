using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGeneratorHooks
    {
        [TestMethod]
        public void InputGeneratorCreatesOneFeatureHook()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "@feature",
                "Feature: my new feature",
                "",
                "Scenario: my new scenario"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("feature", features[0].Hooks[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Hooks.Count, "scenario hook count mismatch");
            Assert.AreEqual("feature", features[0].Scenarios[0].Hooks[0].Name);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneFeatureHookOneScenarioHook()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "@feature",
                "Feature: my new feature",
                "",
                "@scenario",
                "Scenario: my new scenario"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("feature", features[0].Hooks[0].Name);
            Assert.AreEqual(2, features[0].Scenarios[0].Hooks.Count, "scenario hook count mismatch");
            Assert.AreEqual("feature", features[0].Scenarios[0].Hooks[0].Name);
            Assert.AreEqual("scenario", features[0].Scenarios[0].Hooks[1].Name);
        }

        [TestMethod]
        public void InputGeneratorCreatesMultipleHooks()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "@f1@f2 @f3",
                "@f4",
                "Feature: my new feature",
                "",
                "@s1@s2 @s3",
                "@s4",
                "Scenario: my new scenario"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(4, features[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("f1", features[0].Hooks[0].Name);
            Assert.AreEqual("f2", features[0].Hooks[1].Name);
            Assert.AreEqual("f3", features[0].Hooks[2].Name);
            Assert.AreEqual("f4", features[0].Hooks[3].Name);
            Assert.AreEqual(8, features[0].Scenarios[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("f1", features[0].Scenarios[0].Hooks[0].Name);
            Assert.AreEqual("f2", features[0].Scenarios[0].Hooks[1].Name);
            Assert.AreEqual("f3", features[0].Scenarios[0].Hooks[2].Name);
            Assert.AreEqual("f4", features[0].Scenarios[0].Hooks[3].Name);
            Assert.AreEqual("s1", features[0].Scenarios[0].Hooks[4].Name);
            Assert.AreEqual("s2", features[0].Scenarios[0].Hooks[5].Name);
            Assert.AreEqual("s3", features[0].Scenarios[0].Hooks[6].Name);
            Assert.AreEqual("s4", features[0].Scenarios[0].Hooks[7].Name);
        }

        [TestMethod]
        public void InputGeneratorHandlesDuplicateHooks()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "@f1",
                "@f1",
                "Feature: my new feature",
                "",
                "@f1",
                "@f1",
                "@s1",
                "@s1",
                "Scenario: my new scenario"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("f1", features[0].Hooks[0].Name);
            Assert.AreEqual(2, features[0].Scenarios[0].Hooks.Count, "feature hook count mismatch");
            Assert.AreEqual("f1", features[0].Scenarios[0].Hooks[0].Name);
            Assert.AreEqual("s1", features[0].Scenarios[0].Hooks[1].Name);
        }

        [TestMethod]
        public void InputGeneratorCreatesSecondFeatureWithHook()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "@f1",
                "Feature: feature1",
                "",
                "@s1",
                "Scenario: scenario1",
                "",
                "@f2",
                "Feature: feature2",
                "",
                "@s2",
                "Scenario: scenario2",
            };
            var features = generator.Load(contents);

            Assert.AreEqual(2, features.Count, "feature count mismatch");

            VerifyFeature(features[0], "1");

            VerifyFeature(features[1], "2");
        }

        private static void VerifyFeature(NodeFeature feature, string changingText)
        {
            Assert.AreEqual(string.Format("Feature{0}", changingText), feature.Name);
            Assert.AreEqual(1, feature.Hooks.Count, string.Format("feature{0} hook count mismatch", changingText));
            Assert.AreEqual(string.Format("f{0}", changingText), feature.Hooks[0].Name);
            Assert.AreEqual(1, feature.Scenarios.Count, string.Format("scenario{0} count mismatch", changingText));
            Assert.AreEqual(string.Format("Scenario{0}", changingText), feature.Scenarios[0].Name);
            Assert.AreEqual(2, feature.Scenarios[0].Hooks.Count, string.Format("scenario{0} hook count mismatch", changingText));
            Assert.AreEqual(string.Format("f{0}", changingText), feature.Scenarios[0].Hooks[0].Name);
            Assert.AreEqual(string.Format("s{0}", changingText), feature.Scenarios[0].Hooks[1].Name);
        }
    }
}