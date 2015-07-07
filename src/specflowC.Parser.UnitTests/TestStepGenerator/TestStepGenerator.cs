using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests.TestStepGenerator
{
    [TestClass]
    public class TestStepGenerator
    {
        [TestMethod]
        public void StepGeneratorCreatesTwoFeaturesWithScenario()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();
            NodeStep step = new NodeStep();
            step.Name = "GivenMethod";

            NodeFeature feature = new NodeFeature("Feature1");
            NodeScenario scenario = new NodeScenario("Scenario1");
            scenario.Steps.Add(step);
            feature.Scenarios.Add(scenario);
            features.Add(feature);
            feature = new NodeFeature("Feature2");
            scenario = new NodeScenario("Scenario2");
            scenario.Steps.Add(step);
            feature.Scenarios.Add(scenario);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            Assert.AreEqual(2, files.Count, "File count mismatch.");
            for (int i = 0; i < files.Count; i++)
            {
                string[] stringsExpected = new string[] {
                    string.Format("#include \"Feature{0}.h\"", i+1),
                    "",
                    "namespace CppUnitTest",
                    "{",
                    string.Format("\tvoid Feature{0}::GivenMethod()", i+1),
                    "\t{",
                    "\t\tAssert::Fail(L\"Pending implementation...\");",
                    "\t}",
                    "}"
                };
                AssertExt.ContentsOfStringArray(stringsExpected, files[i]);
            }
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStep()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioTwoDuplicateSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add step 1
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";

            //Add step 2
            NodeStep step2 = new NodeStep();
            step2.Name = "GivenMethod1";

            //Add scenario 1
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);
            scenario1.Steps.Add(step2);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioTwoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add step 1
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";

            //Add step 2
            NodeStep step2 = new NodeStep();
            step2.Name = "GivenMethod2";

            //Add scenario 1
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);
            scenario1.Steps.Add(step2);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "",
                "\tvoid Feature1::GivenMethod2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepOneStringParameter()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add parameter
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(string Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepOneDecimalParameter()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add parameter
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "decimal";
            p1.Value = "2.1";

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(decimal Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepTwoParameters()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add parameter 1
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";

            //Add parameter 2
            Parameter p2 = new Parameter();
            p2.Name = "Parameter2";
            p2.Type = "int";
            p2.Value = "2";

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);
            step1.Parameters.Add(p2);

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(string Parameter1, int Parameter2)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepOneRow()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Rows = new List<string[]>() {
                 new [] { "a", "b", "c" }
            };

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepOneParameterAndOneRow()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add parameter
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);
            step1.Rows = new List<string[]>() {
                new [] { "a", "b", "c" }
            };

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(std::vector<std::vector<std::string>> table, int rows, int cols, string Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void StepGeneratorCreatesOneScenarioOneStepTwoParametersAndOneRow()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Add parameter 1
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";

            //Add parameter 2
            Parameter p2 = new Parameter();
            p2.Name = "Parameter2";
            p2.Type = "int";
            p2.Value = "2";

            //Add step
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);
            step1.Parameters.Add(p2);
            step1.Rows = new List<string[]> {
                new string[] { "a", "b", "c" }
            };

            //Add scenario
            NodeScenario scenario1 = new NodeScenario("Scenario1");
            scenario1.Steps.Add(step1);

            //Add feature
            NodeFeature feature1 = new NodeFeature("Feature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            var files = GeneratorFactory.Generate(GeneratorType.StepDefinitionGenerator, features);

            string[] stringsExpected = new string[] {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenMethod1(std::vector<std::vector<std::string>> table, int rows, int cols, string Parameter1, int Parameter2)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t}",
                "}"
            };
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }
    }
}