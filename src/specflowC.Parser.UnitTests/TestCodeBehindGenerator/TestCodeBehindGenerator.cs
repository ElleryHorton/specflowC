using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;

namespace specflowC.Parser.UnitTests
{
	[TestClass]
	public class TestCodeBehindGenerator
	{
		[TestMethod]
		public void CodeBehindGeneratorCreatesTwoFeaturesWithScenario()
		{
			var features = TestCodeBehindData.TwoFeaturesWithScenario();
			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			Assert.AreEqual(2, files.Count, "File count mismatch.");
			for (int i = 0; i < files.Count; i++)
			{
				string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[i].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[i].Name, features[i].Scenarios[0].Name),
				"\t{",
				"\t}",
				"}"
			};

				AssertExt.ContentsOfStringArray(stringsExpected, files[i]);
			}
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioWithNoSteps()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndNoStep();
			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioWithOneStep()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStep();
			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string parameterName = string.Empty;

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				string.Format("\t\t{0}({1});", features[0].Scenarios[0].Steps[0].Name, parameterName),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioWithSteps()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndNoStep();

			features[0].Scenarios[0].Steps.Add(new NodeStep("GivenIHaveAStep"));
			features[0].Scenarios[0].Steps.Add(new NodeStep("WhenIHaveAStep"));
			features[0].Scenarios[0].Steps.Add(new NodeStep("ThenIHaveAStep"));

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string parameterName = string.Empty;

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				string.Format("\t\t{0}({1});", features[0].Scenarios[0].Steps[0].Name, parameterName),
				string.Format("\t\t{0}({1});", features[0].Scenarios[0].Steps[1].Name, parameterName),
				string.Format("\t\t{0}({1});", features[0].Scenarios[0].Steps[2].Name, parameterName),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesStepWithOneStringParameter()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStepAndParameter("string");

			string parameterName = string.Empty;

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				string.Format("\t\t{0}(\"{1}\");", features[0].Scenarios[0].Steps[0].Name, features[0].Scenarios[0].Steps[0].Parameters[0].Value),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesStepWithOneNumberParameter()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStepAndParameter("int");

			string parameterName = string.Empty;

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				string.Format("\t\t{0}({1});", features[0].Scenarios[0].Steps[0].Name, features[0].Scenarios[0].Steps[0].Parameters[0].Value),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}
	}
}