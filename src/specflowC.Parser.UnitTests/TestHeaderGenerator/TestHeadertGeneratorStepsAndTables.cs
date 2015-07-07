using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestHeaderGeneratorStepsAndTables
    {
        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureWithDuplicateSteps()
        {
            IList<NodeFeature> features = new List<NodeFeature>();

            //Create duplicate steps & add
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenAMethod1";

            NodeStep step2 = new NodeStep();
            step2.Name = "WhenUsingAMethod1";

            NodeStep step3 = new NodeStep();
            step3.Name = "ThenUsingAMethod1";

            NodeStep step4 = new NodeStep();
            step4.Name = "GivenAMethod1";

            NodeStep step5 = new NodeStep();
            step5.Name = "WhenUsingAMethod1";

            NodeStep step6 = new NodeStep();
            step6.Name = "ThenUsingAMethod1";

            //Create scenario & add
            NodeScenario scenario1 = new NodeScenario("MyScenario1");
            scenario1.Steps.Add(step1);
            scenario1.Steps.Add(step2);
            scenario1.Steps.Add(step3);
            scenario1.Steps.Add(step4);
            scenario1.Steps.Add(step5);
            scenario1.Steps.Add(step6);

            //Create feature & add
            NodeFeature feature1 = new NodeFeature("MyFeature1");
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            //Call output generator
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
				"\tTEST_CLASS(MyFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_METHOD(MyScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenAMethod1();",
				"\t\tvoid WhenUsingAMethod1();",
				"\t\tvoid ThenUsingAMethod1();",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureWithOneTable()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();
            List<NodeHook> featureHooks = new List<NodeHook>();

            List<string[]> rows = new List<string[]>();

            //Create row array & add
            string[] row1 = new string[] { "a", "b", "c" };
            string[] row2 = new string[] { "1", "2", "3" };
            rows.Add(row1);
            rows.Add(row2);

            //Create step & add
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenStep1WithTable";
            step1.Rows = rows;

            //Create scenario & add
            NodeScenario scenario1 = new NodeScenario("MyScenario1", new List<NodeHook>() { new NodeHook("MyFeatureHook1"), new NodeHook("MyScenarioHook1") });
            scenario1.Steps.Add(step1);

            //Create feature & add
            NodeFeature feature1 = new NodeFeature("MyFeature1", new List<NodeHook>() { new NodeHook("MyFeatureHook1") });
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            //Call output generator
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
				"\tTEST_CLASS(MyFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_CLASS_HOOK_MYFEATUREHOOK1()",
				"\t\tTEST_METHOD_HOOK_MYFEATUREHOOK1_MYSCENARIOHOOK1(MyScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenStep1WithTable(std::vector<std::vector<std::string>> table, int rows, int cols);",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureWithOneTableAndOneParameter()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();
            List<NodeHook> featureHooks = new List<NodeHook>();

            //Create parameter & add
            Parameter p1 = new Parameter();
            p1.Name = "MyParameter1";
            p1.Type = "string";
            p1.Value = "ValueOfMyParameter1";

            //Create step & add
            NodeStep step1 = new NodeStep();
            step1.Name = "GivenStep1WithTable";
            step1.Parameters.Add(p1);
            step1.Rows = new List<string[]> {
				new [] { "a", "b", "c" },
				new [] { "1", "2", "3" }
			};

            //Create scenario & add
            NodeScenario scenario1 = new NodeScenario("MyScenario1", new List<NodeHook>() { new NodeHook("MyFeatureHook1"), new NodeHook("MyScenarioHook1") });
            scenario1.Steps.Add(step1);

            //Create feature & add
            NodeFeature feature1 = new NodeFeature("MyFeature1", new List<NodeHook>() { new NodeHook("MyFeatureHook1") });
            feature1.Scenarios.Add(scenario1);
            features.Add(feature1);

            //Call output generator
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
				"\tTEST_CLASS(MyFeature1)",
				"\t{",
				"\tpublic:",
				"\t\tTEST_CLASS_HOOK_MYFEATUREHOOK1()",
				"\t\tTEST_METHOD_HOOK_MYFEATUREHOOK1_MYSCENARIOHOOK1(MyScenario1);",
				"",
				"\tprivate:",
				"\t\tvoid GivenStep1WithTable(std::vector<std::vector<std::string>> table, int rows, int cols, string MyParameter1);",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }
    }
}