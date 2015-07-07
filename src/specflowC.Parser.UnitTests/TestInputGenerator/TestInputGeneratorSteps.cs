using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace specflowC.Parser.UnitTests
{
    [TestClass]
    public class TestInputGeneratorSteps
    {
        [TestMethod]
        public void InputGeneratorCreatesOneStep()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStep", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(0, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithOneParameterAndOneStepWithoutParameters()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with one 'parameter'",
                "\tThen I have two steps"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(2, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWithOne", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("parameter", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
            Assert.AreEqual("ThenIHaveTwoSteps", features[0].Scenarios[0].Steps[1].Name);
            Assert.AreEqual(0, features[0].Scenarios[0].Steps[1].Parameters.Count, "parameter count mismatch");
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithOneParameterThatHasSpaces()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with one 'parameter that has spaces'"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWithOne", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("parameter that has spaces", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithOneParameterInTheMiddle()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with one 'parameter' in the middle"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWithOneInTheMiddle", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("parameter", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithTwoParameters()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with 'two' amazing 'parameters'"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWithAmazing", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(2, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("p0", features[0].Scenarios[0].Steps[0].Parameters[0].Name);
            Assert.AreEqual("p1", features[0].Scenarios[0].Steps[0].Parameters[1].Name);
            Assert.AreEqual("two", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
            Assert.AreEqual("parameters", features[0].Scenarios[0].Steps[0].Parameters[1].Value);
        }

        [TestMethod]
        public void InputGeneratorCreatesOneStepWithTwoParametersBackToBack()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with 'two' 'parameters'"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWith", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(2, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("p0", features[0].Scenarios[0].Steps[0].Parameters[0].Name);
            Assert.AreEqual("p1", features[0].Scenarios[0].Steps[0].Parameters[1].Name);
            Assert.AreEqual("two", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
            Assert.AreEqual("parameters", features[0].Scenarios[0].Steps[0].Parameters[1].Value);
        }

        [TestMethod]
        public void InputGeneratorParametersAreCaseSensitive()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with 'cAsE sEnSiTiVe' parameter"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(1, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual("GivenIHaveAStepWithParameter", features[0].Scenarios[0].Steps[0].Name);
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch");
            Assert.AreEqual("cAsE sEnSiTiVe", features[0].Scenarios[0].Steps[0].Parameters[0].Value);
        }

        [TestMethod]
        public void InputGeneratorParameterTypesAreCorrect()
        {
            InputGenerator generator = new InputGenerator();
            string[] contents = new string[] {
                "Feature: my new feature",
                "",
                "Scenario: my new scenario",
                "\tGiven I have a step with '1' int",
                "\tGiven I have a step with '1.1' decimal",
                "\tGiven I have a step with 'one1' string"
            };
            var features = generator.Load(contents);

            Assert.AreEqual(3, features[0].Scenarios[0].Steps.Count, "step count mismatch");
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[0].Parameters.Count, "parameter count mismatch on step 1");
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[1].Parameters.Count, "parameter count mismatch on step 2");
            Assert.AreEqual(1, features[0].Scenarios[0].Steps[2].Parameters.Count, "parameter count mismatch on step 3");

            Assert.AreEqual("int", features[0].Scenarios[0].Steps[0].Parameters[0].Type);
            Assert.AreEqual("decimal", features[0].Scenarios[0].Steps[1].Parameters[0].Type);
            Assert.AreEqual("string", features[0].Scenarios[0].Steps[2].Parameters[0].Type);
        }
    }
}