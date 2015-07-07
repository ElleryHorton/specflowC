using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    public static class TestCodeBehindData
    {
        public static IList<NodeFeature> FeatureWithScenarioAndNoStep()
        {
            IList<NodeFeature> features = new List<NodeFeature>();
            NodeFeature featureWithScenario = new NodeFeature("Feature1");
            NodeScenario scenario = new NodeScenario("Scenario1");
            featureWithScenario.Scenarios.Add(scenario);    // scenario to feature
            features.Add(featureWithScenario);              // feature to feature list

            return features;
        }

        public static IList<NodeFeature> TwoFeaturesWithScenario()
        {
            IList<NodeFeature> features = new List<NodeFeature>();

            NodeFeature featureWithScenario = new NodeFeature("Feature1");
            NodeScenario scenario = new NodeScenario("Scenario1");
            featureWithScenario.Scenarios.Add(scenario);    // scenario to feature
            features.Add(featureWithScenario);              // feature to feature list

            featureWithScenario = new NodeFeature("Feature2");
            scenario = new NodeScenario("Scenario2");
            featureWithScenario.Scenarios.Add(scenario);    // scenario to feature
            features.Add(featureWithScenario);              // feature to feature list

            return features;
        }

        public static IList<NodeFeature> FeatureWithScenarioAndStep()
        {
            var features = FeatureWithScenarioAndNoStep();

            NodeStep step = new NodeStep();
            step.Name = "GivenIHaveAStep";

            features[0].Scenarios[0].Steps.Add(step);   // steps to scenario

            return features;
        }

        public static IList<NodeFeature> FeatureWithScenarioAndStepAndParameter(string type)
        {
            var features = FeatureWithScenarioAndStep();

            features[0].Scenarios[0].Steps[0].Parameters.Add(
                new Parameter()
                {
                    IsFromExampleTable = false,
                    Name = "p0",
                    Type = type,
                    Value = "123"
                }
            );

            return features;
        }

        public static IList<NodeFeature> FeatureWithScenarioOutlineAndStep()
        {
            IList<NodeFeature> features = new List<NodeFeature>();
            NodeFeature featureWithScenario = new NodeFeature("Feature1", new List<NodeHook>());
            NodeScenarioOutline scenario = new NodeScenarioOutline("Scenario1", new List<NodeHook>());
            scenario.Examples.Rows = new List<string[]>() {
                new[] { "a", "b", "c" },
                new[] { "1", "2", "3" },
                new[] { "4", "5", "6" }
            };
            NodeStep step = new NodeStep();
            step.Name = "GivenIHaveAStep";

            scenario.Steps.Add(step);                       // steps to scenario
            featureWithScenario.Scenarios.Add(scenario);    // scenario to feature
            features.Add(featureWithScenario);              // feature to feature list

            return features;
        }

        public static IList<NodeFeature> FeatureWithScenarioOutlineAndStepAndParameter(string type)
        {
            var features = FeatureWithScenarioOutlineAndStep();

            features[0].Scenarios[0].Steps[0].Parameters.Add(
                new Parameter()
                {
                    IsFromExampleTable = false,
                    Name = "p0",
                    Type = type,
                    Value = "123"
                }
            );

            return features;
        }

        public static IList<NodeFeature> FeatureWithScenarioOutlineAndStepAndParameterized(string type)
        {
            var features = FeatureWithScenarioOutlineAndStep();

            features[0].Scenarios[0].Steps[0].Parameters.Add(
                new Parameter()
                {
                    IsFromExampleTable = true,
                    Name = "p0",
                    Type = type,
                    Value = "a"
                }
            );

            features[0].Scenarios[0].Steps[0].Parameters.Add(
                new Parameter()
                {
                    IsFromExampleTable = true,
                    Name = "p0",
                    Type = type,
                    Value = "b"
                }
            );

            return features;
        }
    }
}