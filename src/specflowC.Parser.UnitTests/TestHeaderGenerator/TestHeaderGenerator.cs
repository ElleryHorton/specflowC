using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestHeaderGenerator
    {
        [TestMethod]
        public void HeaderGeneratorCreatesOneFeatureNoHooksNoScenariosNoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();
            features.Add(new NodeFeature("MyTestFeature"));

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
				"\tTEST_CLASS(MyTestFeature)",
				"\t{",
				"\tpublic:",
				"\t};",
				"}"
			};
            AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
        }

        [TestMethod]
        public void HeaderGeneratorCreatesTwoFeaturesNoHooksNoScenariosNoSteps()
        {
            IList<NodeFeature> features;
            features = new List<NodeFeature>();
            features.Add(new NodeFeature("MyTestFeature1"));
            features.Add(new NodeFeature("MyTestFeature2"));

            var files = GeneratorFactory.Generate(GeneratorType.HeaderGenerator, features);

            Assert.AreEqual(2, files.Count, "File count mismatch.");
            for (int i = 0; i < files.Count; i++)
            {
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
					string.Format("\tTEST_CLASS(MyTestFeature{0})", i+1),
					"\t{",
					"\tpublic:",
					"\t};",
					"}"
				};
                AssertExt.ContentsOfStringArray(stringsExpected, files[i]);
            }
        }
    }
}