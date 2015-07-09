using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestStepDefinitionParser
    {
        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureAndOneStepAndNoParameters()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "Parameter count mismatch");
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureAndTwoSteps()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature1::GivenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[0].Steps[1].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureOneStepAndOneParameter()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(string Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[0].Parameters[0].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureOneStepAndTwoParameters()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(string Parameter1, string Parameter2)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[0].Parameters[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[0].Parameters[1].Type);
            Assert.AreEqual("Parameter2", featureGroup[0].Steps[0].Parameters[1].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureTwoStepsAndTwoParameters()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(string Parameter1, int Parameter2)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature1::WhenASentence(decimal Parameter1, string Parameter2)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[0].Parameters[0].Name);
            Assert.AreEqual("int", featureGroup[0].Steps[0].Parameters[1].Type);
            Assert.AreEqual("Parameter2", featureGroup[0].Steps[0].Parameters[1].Name);
            Assert.AreEqual("WhenASentence", featureGroup[0].Steps[1].Name);
            Assert.AreEqual("decimal", featureGroup[0].Steps[1].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[1].Parameters[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[1].Parameters[1].Type);
            Assert.AreEqual("Parameter2", featureGroup[0].Steps[1].Parameters[1].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureOneStepAndOneTableParameter()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureOneStepAndOneTableParameterAndOneRegularParameter()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(std::vector<std::vector<std::string>> table, int rows, int cols, decimal Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual(1, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");
            Assert.AreEqual("decimal", featureGroup[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[0].Parameters[0].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithOneFeatureTwoStepsAndOneTableParameterEach()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature1::WhenASentence(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence", featureGroup[0].Steps[0].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");

            Assert.AreEqual("WhenASentence", featureGroup[0].Steps[1].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithTwoFeaturesAndOneStep()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::GivenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);

            Assert.AreEqual("Feature2", featureGroup[1].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[1].Steps[0].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithTwoFeaturesOneStepAndOneParameterEach()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1(string Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::GivenASentence2(int Parameter1)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("string", featureGroup[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[0].Steps[0].Parameters[0].Name);

            Assert.AreEqual("Feature2", featureGroup[1].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[1].Steps[0].Name);
            Assert.AreEqual("int", featureGroup[1].Steps[0].Parameters[0].Type);
            Assert.AreEqual("Parameter1", featureGroup[1].Steps[0].Parameters[0].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithTwoFeaturesOneStepAndOneTableParameterEach()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::GivenASentence2(std::vector<std::vector<std::string>> table, int rows, int cols)",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");

            Assert.AreEqual("Feature2", featureGroup[1].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[1].Steps[0].Name);
            Assert.AreEqual(0, featureGroup[0].Steps[0].Parameters.Count, "table counted as a parameter");
            Assert.AreEqual(1, featureGroup[0].Steps[0].Rows.Count, "table doesn't have any rows");
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithTwoFeaturesAndTwoStepsEach()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature1::WhenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::GivenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::WhenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("WhenASentence1", featureGroup[0].Steps[1].Name);

            Assert.AreEqual("Feature2", featureGroup[1].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[1].Steps[0].Name);
            Assert.AreEqual("WhenASentence2", featureGroup[1].Steps[1].Name);
        }

        [TestMethod]
        public void StepDefinitionParsesFileWithTwoFeaturesAndTwoStepsEachWithDifferentOrder()
        {
            string[] stepDefinitionFile1 = new string[]
            {
                "#include \"Feature1.h\"",
                "",
                "namespace CppUnitTest",
                "{",
                "\tvoid Feature1::GivenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::GivenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature1::WhenASentence1()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "",
                "\tvoid Feature2::WhenASentence2()",
                "\t{",
                "\t\tAssert::Fail(L\"Pending implementation...\");",
                "\t{",
                "}"
            };

            StepDefinitionParser parser = new StepDefinitionParser();
            List<FeatureGroup> featureGroup = parser.Parse(stepDefinitionFile1);

            Assert.AreEqual("Feature1", featureGroup[0].FeatureName);
            Assert.AreEqual("GivenASentence1", featureGroup[0].Steps[0].Name);
            Assert.AreEqual("WhenASentence1", featureGroup[0].Steps[1].Name);

            Assert.AreEqual("Feature2", featureGroup[1].FeatureName);
            Assert.AreEqual("GivenASentence2", featureGroup[1].Steps[0].Name);
            Assert.AreEqual("WhenASentence2", featureGroup[1].Steps[1].Name);
        }
    }
}