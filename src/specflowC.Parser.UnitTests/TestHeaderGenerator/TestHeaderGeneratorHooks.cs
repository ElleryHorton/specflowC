using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestHeaderGeneratorHooks
    {
        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndOneFeatureHook()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> hooks = new List<NodeHook>();
            hooks.Add(new NodeHook("integration"));

            features.Add(new NodeFeature("TestFeature1", hooks));

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
				"\t\tTEST_CLASS_HOOK_INTEGRATION()",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndTwoFeatureHooks()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> hooks = new List<NodeHook>();
            hooks.Add(new NodeHook("hook1"));
            hooks.Add(new NodeHook("hook2"));

            features.Add(new NodeFeature("TestFeature1", hooks));

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
				"\t\tTEST_CLASS_HOOK_HOOK1()",
				"\t\tTEST_CLASS_HOOK_HOOK2()",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndOneScenarioHook()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            NodeFeature testFeature = new NodeFeature("TestFeature1");
            testFeature.Scenarios.Add(new NodeScenario("TestScenario1", new List<NodeHook>() { new NodeHook("scenariohook1") }));
            features.Add(testFeature);

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
				"\t\tTEST_METHOD_HOOK_SCENARIOHOOK1(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureAndTwoScenarioHooks()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> scenarioHooks = new List<NodeHook>();
            scenarioHooks.Add(new NodeHook("scenariohook1"));
            scenarioHooks.Add(new NodeHook("scenariohook2"));

            NodeFeature testFeature = new NodeFeature("TestFeature1");
            testFeature.Scenarios.Add(new NodeScenario("TestScenario1", scenarioHooks));
            features.Add(testFeature);

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
				"\t\tTEST_METHOD_HOOK_SCENARIOHOOK1_SCENARIOHOOK2(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneFeatureHookAndNoScenarioHooks()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            NodeFeature testFeature = new NodeFeature("TestFeature1", new List<NodeHook>() { new NodeHook("featurehook1") });
            testFeature.Scenarios.Add(new NodeScenario("TestScenario1", new List<NodeHook>() { new NodeHook("featurehook1") }));

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneFeatureHookAndOneScenarioHook()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            NodeFeature testFeature = new NodeFeature("TestFeature1", new List<NodeHook>() { new NodeHook("featurehook1") });
            testFeature.Scenarios.Add(new NodeScenario("TestScenario1", new List<NodeHook>() { new NodeHook("featurehook1"), new NodeHook("scenariohook1") }));

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1_SCENARIOHOOK1(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureTwoFeatureHooksAndTwoScenarioHooks()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> featureHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2")
			};

            List<NodeHook> scenarioHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2"),
				new NodeHook("scenariohook1"),
				new NodeHook("scenariohook2")
			};

            NodeFeature testFeature = new NodeFeature("TestFeature1", featureHooks);
            testFeature.Scenarios.Add(new NodeScenario("TestScenario1", scenarioHooks));

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK2()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1_FEATUREHOOK2_SCENARIOHOOK1_SCENARIOHOOK2(TestScenario1);",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureOneFeatureHookOneScenarioHookAndOneStep()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Step 1
            NodeStep step1 = new NodeStep(tokenStep1);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1", new List<NodeHook>() { new NodeHook("featurehook1"), new NodeHook("scenariohook1") });
            scenario1.Steps.Add(step1);

            //Creates Feature 1
            NodeFeature testFeature = new NodeFeature("TestFeature1", new List<NodeHook>() { new NodeHook("featurehook1") });
            testFeature.Scenarios.Add(scenario1);

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1_SCENARIOHOOK1(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureTwoFeatureHooksTwoScenarioHooksAndOneStep()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> featureHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2")
			};

            List<NodeHook> scenarioHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2"),
				new NodeHook("scenariohook1"),
				new NodeHook("scenariohook2")
			};

            //Creates Gherkin Step 1
            TokenGherkinStep tokenStep1 = new TokenGherkinStep();
            tokenStep1.MethodName = "GivenMethod1";
            List<string> tokenParameters1 = new List<string>();
            tokenStep1.ParameterTokens = tokenParameters1;

            //Creates Step 1
            NodeStep step1 = new NodeStep(tokenStep1);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1", scenarioHooks);
            scenario1.Steps.Add(step1);

            //Creates Feature 1
            NodeFeature testFeature = new NodeFeature("TestFeature1", featureHooks);
            testFeature.Scenarios.Add(scenario1);

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK2()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1_FEATUREHOOK2_SCENARIOHOOK1_SCENARIOHOOK2(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureTwoFeatureHooksTwoScenarioHooksAndTwoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();

            List<NodeHook> featureHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2")
			};

            List<NodeHook> scenarioHooks = new List<NodeHook>() {
				new NodeHook("featurehook1"),
				new NodeHook("featurehook2"),
				new NodeHook("scenariohook1"),
				new NodeHook("scenariohook2")
			};

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

            //Creates Step 1
            NodeStep step1 = new NodeStep(tokenStep1);

            //Creates Step 2
            NodeStep step2 = new NodeStep(tokenStep2);

            //Creates Scenario 1
            NodeScenario scenario1 = new NodeScenario("TestScenario1", scenarioHooks);
            scenario1.Steps.Add(step1);
            scenario1.Steps.Add(step2);

            //Creates Feature 1
            NodeFeature testFeature = new NodeFeature("TestFeature1", featureHooks);
            testFeature.Scenarios.Add(scenario1);

            features.Add(testFeature);

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
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK1()",
				"\t\tTEST_CLASS_HOOK_FEATUREHOOK2()",
				"\t\tTEST_METHOD_HOOK_FEATUREHOOK1_FEATUREHOOK2_SCENARIOHOOK1_SCENARIOHOOK2(TestScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenMethod1();",
				"\t\tvoid GivenMethod2();",
				"\t};",
				"}"
			};

            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }
    }
}