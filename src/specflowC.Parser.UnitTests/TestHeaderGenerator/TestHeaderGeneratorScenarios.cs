using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestHeaderGeneratorScenarios
    {
        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndOneScenario()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            NodeFeature featureScenario = new NodeFeature("TestFeature1");
            featureScenario.Scenarios.Add(new NodeScenario("TestScenario1"));

            features.Add(featureScenario);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndTwoScenarios()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            NodeFeature featureScenario = new NodeFeature("TestFeature1");
            featureScenario.Scenarios.Add(new NodeScenario("TestScenario1"));
            featureScenario.Scenarios.Add(new NodeScenario("TestScenario2"));

            features.Add(featureScenario);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"\t\tTEST_METHOD(TestScenario2);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneScenarioAndOneStep()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Step 1 & Adds to list
            NodeStep step1 = new NodeStep(tokenStep1);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneScenarioAndTwoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Gherkin Step 2
            TokenGherkinStep tokenStep2 = new TokenGherkinStep();
            tokenStep2.MethodName = "GivenMethod2";
            List<string> tokenParameters2 = new List<string>();
            tokenStep2.ParameterTokens = tokenParameters2;

            //Creates Step 1 & Adds to list
            NodeStep step1 = new NodeStep(tokenStep1);

            //Creates Step 2 & Adds to list
            NodeStep step2 = new NodeStep(tokenStep2);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1);
            scenario1.Steps.Add(step2);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1();",
				"\t\tvoid GivenMethod2();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureTwoScenariosAndOneStep()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1 for Scenario 1
            TokenGherkinStep tokenStepScenario1 = new TokenGherkinStep();
            tokenStepScenario1.MethodName = "GivenMethod1Scenario1";
            List<string> tokenParameterScenario1 = new List<string>();
            tokenStepScenario1.ParameterTokens = tokenParameterScenario1;

            //Creates Gherkin Step 1 for Scenario 2
            TokenGherkinStep tokenStepScenario2 = new TokenGherkinStep();
            tokenStepScenario2.MethodName = "GivenMethod1Scenario2";
            List<string> tokenParameterScenario2 = new List<string>();
            tokenStepScenario2.ParameterTokens = tokenParameterScenario2;

            //Creates Step 1 for Scenario 1 & Adds to list
            NodeStep step1Scenario1 = new NodeStep(tokenStepScenario1);

            //Creates Step 1 for Scenario 2 & Adds to list
            NodeStep step1Scenario2 = new NodeStep(tokenStepScenario2);

            //Creates Scenario 1 & Adds to list
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1Scenario1);

            //Creates Scenario 2 & Adds to list
            NodeScenario scenario2 = new NodeScenario("TestScenario2");
            scenario2.Steps.Add(step1Scenario2);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            feature.Scenarios.Add(scenario2);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"\t\tTEST_METHOD(TestScenario2);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1Scenario1();",
				"\t\tvoid GivenMethod1Scenario2();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureTwoScenariosAndTwoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1 for Scenario 1
            TokenGherkinStep tokenStep1Scenario1 = new TokenGherkinStep();
            tokenStep1Scenario1.MethodName = "GivenMethod1Scenario1";
            List<string> tokenParameter1Scenario1 = new List<string>();
            tokenStep1Scenario1.ParameterTokens = tokenParameter1Scenario1;

            //Creates Gherkin Step 2 for Scenario 1
            TokenGherkinStep tokenStep2Scenario1 = new TokenGherkinStep();
            tokenStep2Scenario1.MethodName = "GivenMethod2Scenario1";
            List<string> tokenParameter2Scenario1 = new List<string>();
            tokenStep2Scenario1.ParameterTokens = tokenParameter2Scenario1;

            //Creates Gherkin Step 1 for Scenario 2
            TokenGherkinStep tokenStep1Scenario2 = new TokenGherkinStep();
            tokenStep1Scenario2.MethodName = "GivenMethod1Scenario2";
            List<string> tokenParameter1Scenario2 = new List<string>();
            tokenStep1Scenario2.ParameterTokens = tokenParameter1Scenario2;

            //Creates Gherkin Step 2 for Scenario 2
            TokenGherkinStep tokenStep2Scenario2 = new TokenGherkinStep();
            tokenStep2Scenario2.MethodName = "GivenMethod2Scenario2";
            List<string> tokenParameter2Scenario2 = new List<string>();
            tokenStep2Scenario2.ParameterTokens = tokenParameter2Scenario2;

            //Creates Step 1 for Scenario 1 & Adds to list
            NodeStep step1Scenario1 = new NodeStep(tokenStep1Scenario1);

            //Creates Step 2 for Scenario 1 & Adds to list
            NodeStep step2Scenario1 = new NodeStep(tokenStep2Scenario1);

            //Creates Step 1 for Scenario 2 & Adds to list
            NodeStep step1Scenario2 = new NodeStep(tokenStep1Scenario2);

            //Creates Step 2 for Scenario 2 & Adds to list
            NodeStep step2Scenario2 = new NodeStep(tokenStep2Scenario2);

            //Creates Scenario 1 & Adds to list
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1Scenario1);
            scenario1.Steps.Add(step2Scenario1);

            //Creates Scenario 2 & Adds to list
            NodeScenario scenario2 = new NodeScenario("TestScenario2");
            scenario2.Steps.Add(step1Scenario2);
            scenario2.Steps.Add(step2Scenario2);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            feature.Scenarios.Add(scenario2);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"\t\tTEST_METHOD(TestScenario2);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1Scenario1();",
				"\t\tvoid GivenMethod2Scenario1();",
				"\t\tvoid GivenMethod1Scenario2();",
				"\t\tvoid GivenMethod2Scenario2();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneScenarioOneStepAndOneParameter()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            string parameter1 = "";
            tokenParameters1.Add(parameter1);
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Parameter For Step 1
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";

            //Creates a list of rows for step 1
            List<string[]> rows = new List<string[]>();

            //Creates Step 1 & Adds to list
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1(string Parameter1);",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneScenarioOneStepAndTwoParameters()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            string parameter1 = "";
            tokenParameters1.Add(parameter1);
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Parameter For Step 1
            Parameter p1 = new Parameter();
            p1.Name = "Parameter1";
            p1.Type = "string";
            p1.Value = "ValueOfParameter1";
            Parameter p2 = new Parameter();
            p2.Name = "Parameter2";
            p2.Type = "int";
            p2.Value = "2";

            //Creates Step 1 & Adds to list
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenMethod1";
            step1.Parameters.Add(p1);
            step1.Parameters.Add(p2);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1");
            scenario1.Steps.Add(step1);

            //Creates Feature & Adds to list
            NodeFeature feature = new NodeFeature("TestFeature1");
            feature.Scenarios.Add(scenario1);
            features.Add(feature);

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            string[] stringsExpected = new string[] {
				"#include \"CppUnitTest.h\"",
				"#include \"CppUnitTestHooks.h\"",
				"#include \"trim.hpp\"",
				"#include <vector>",
				"",
				"using namespace Microsoft::VisualStudio::CppUnitTestFramework;",
				"using namespace std;",
				"",
				"namespace CppUnitTest",
				"{",
				"\tTEST_CLASS(TestFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1(string Parameter1, int Parameter2);",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }
    }
}