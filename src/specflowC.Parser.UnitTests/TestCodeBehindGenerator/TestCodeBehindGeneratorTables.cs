using Microsoft.VisualStudio.TestTools.UnitTesting;
using specflowC.Parser.Nodes;
using System.Collections.Generic;

namespace specflowC.Parser.UnitTests
{
	[TestClass]
	public class TestCodeBehindGeneratorTables
	{
		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioWithOneStepAndTable()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStep();

			features[0].Scenarios[0].Steps[0].Rows = new List<string[]>() {
				new[] {"a", "b", "c"},
				new[] {"1", "2", "3"}
			};

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				"\t\tstd::vector<std::vector<std::string>> table = {{",
				"\t\t\t{ \"a\", \"b\", \"c\" },",
				"\t\t\t{ \"1\", \"2\", \"3\" }",
				"\t\t}};",
				string.Format("\t\t{0}(table,{1},{2});",
					features[0].Scenarios[0].Steps[0].Name,
					features[0].Scenarios[0].Steps[0].Rows.Count,
					features[0].Scenarios[0].Steps[0].Rows[0].Length),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesErrorAssertForUnevenTable()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStep();

			features[0].Scenarios[0].Steps[0].Rows = new List<string[]>() {
				new[] {"a", "b", "c"},
				new[] {"1", "2", "3"},
				new[] {"4", "5" },
				new[] {"7", "8", "9"}
			};

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				"\t\tAssert::Fail(L\"PARSE ERROR: Table is uneven\");",
				"\t\tstd::vector<std::vector<std::string>> table = {{",
				"\t\t\t{ \"a\", \"b\", \"c\" },",
				"\t\t\t{ \"1\", \"2\", \"3\" },",
				"\t\t\t{ \"4\", \"5\" },",
				"\t\t\t{ \"7\", \"8\", \"9\" }",
				"\t\t}};",
				string.Format("\t\t{0}(table,{1},{2});",
					features[0].Scenarios[0].Steps[0].Name,
					features[0].Scenarios[0].Steps[0].Rows.Count,
					features[0].Scenarios[0].Steps[0].Rows[0].Length),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioWithStepsAndParametersAndTable()
		{
			var features = TestCodeBehindData.FeatureWithScenarioAndStepAndParameter("string");

			NodeStep step = new NodeStep();
			step.Name = "WhenIHaveAStep";
			features[0].Scenarios[0].Steps.Add(new NodeStep());
			features[0].Scenarios[0].Steps[0].Rows = new List<string[]>() {
				new[] {"a", "b", "c"},
				new[] {"1", "2", "3"}
			};

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			string[] stringsExpected = new string[] {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{",
				"\t\tstd::vector<std::vector<std::string>> table = {{",
				"\t\t\t{ \"a\", \"b\", \"c\" },",
				"\t\t\t{ \"1\", \"2\", \"3\" }",
				"\t\t}};",
				string.Format("\t\t{0}(\"{1}\",table,{2},{3});",
					features[0].Scenarios[0].Steps[0].Name,
					features[0].Scenarios[0].Steps[0].Parameters[0].Value,
					features[0].Scenarios[0].Steps[0].Rows.Count,
					features[0].Scenarios[0].Steps[0].Rows[0].Length),
				string.Format("\t\t{0}();", features[0].Scenarios[0].Steps[1].Name),
				"\t}",
				"}"
			};

			AssertExt.ContentsOfStringArray(stringsExpected, files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioOutlineWithStep()
		{
			var features = TestCodeBehindData.FeatureWithScenarioOutlineAndStep();

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			List<string> stringsExpected = new List<string> {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{"
			};
			NodeScenarioOutline outline = (NodeScenarioOutline)features[0].Scenarios[0];
			for (int i = 0; i < outline.Examples.Rows.Count - 1; i++)
			{
				foreach (var step in outline.Steps)
				{
					stringsExpected.Add(string.Format("\t\t{0}();", step.Name));
				}
			}
			List<string> stringsExpectedEnd = new List<string> {
				"\t}",
				"}"
			};

			stringsExpected.AddRange(stringsExpectedEnd);

			AssertExt.ContentsOfStringArray(stringsExpected.ToArray(), files[0]);
		}

		[TestMethod]
		public void CodeBehindGeneratorCreatesScenarioOutlineWithStepAndParameterized()
		{
			var features = TestCodeBehindData.FeatureWithScenarioOutlineAndStepAndParameterized("string");

			var files = GeneratorFactory.Generate(GeneratorType.CodeBehindGenerator, features);

			List<string> stringsExpected = new List<string> {
				string.Format("#include \"{0}.h\"", features[0].Name),
				string.Empty,
				"namespace CppUnitTest",
				"{",
				string.Format("\tvoid {0}::{1}()", features[0].Name, features[0].Scenarios[0].Name),
				"\t{"
			};
			NodeScenarioOutline outline = (NodeScenarioOutline)features[0].Scenarios[0];
			for (int i = 1; i < outline.Examples.Rows.Count; i++) // skip header row
			{
				foreach (var step in outline.Steps)
				{
					stringsExpected.Add(string.Format("\t\t{0}(\"{1}\", \"{2}\");",
						step.Name,
						outline.Examples.Rows[i][0],
						outline.Examples.Rows[i][1]));
				}
			}
			List<string> stringsExpectedEnd = new List<string> {
				"\t}",
				"}"
			};

			stringsExpected.AddRange(stringsExpectedEnd);

			AssertExt.ContentsOfStringArray(stringsExpected.ToArray(), files[0]);
		}
	}
}