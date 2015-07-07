using SpecFlow.Feature.CPPParser.Nodes;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Feature.CPPParser
{
    public class OutputGenerator
    {
        private List<string> contents;
        private const string TestClassMacro = "TEST_CLASS";
        private const string TestClassHookPrefix = "TEST_CLASS_HOOK_";
        private const string TestMethodHookPrefix = "TEST_METHOD_HOOK";
        private const string TestMethodPrefix = "TEST_METHOD";
            
        public List<string[]> Generate(IList<NodeFeature> features)
        {
            var listOfFileContents = new List<string[]>();
            foreach (var feature in features)
            {
                contents = new List<string>();
                listOfFileContents.Add(BuildTestClassHeaderFile(feature, "CPPPlatformUnitTest"));

            }
            return listOfFileContents;
        }

        private string[] BuildTestClassHeaderFile(NodeFeature feature, string nameSpaceName)
        {
            BuildIncludeStatements();
            BuildUsingStatements();
            OpenNameSpace(nameSpaceName);
            OpenTestClass(feature.Name);
            
            BuildTestClass(feature);
            
            CloseTestClass();
            CloseNameSpace();

            return contents.ToArray();
        }

        private void BuildTestClass(NodeFeature feature)
        {
            List<string> privateContents = new List<string>();
            List<string> publicContents = new List<string>();
            List<NodeStep> uniqueSteps = new List<NodeStep>();
                        
            foreach (var scenario in feature.Scenarios)
            { 
                AddScenario(scenario, feature.Hooks, publicContents);
                List<NodeStep> stepsToPrint = new List<NodeStep>();
                foreach (var step in scenario.Steps)
                {
                    if (!uniqueSteps.Any(uniqueStep => uniqueStep.Equals(step)))
                    {
                        uniqueSteps.Add(step);
                        stepsToPrint.Add(step);
                    }
                }

                AddStepsForScenario(stepsToPrint, scenario, feature.Scenarios, privateContents);
            }
            
            OpenTestClassPublicSection(feature.Hooks);

            contents.AddRange(publicContents);

            if (privateContents.Count > 0)
            {
                contents.AddRange(privateContents);
            }
        }

        private void CloseNameSpace()
        {
            contents.Add("}");
        }

        private void CloseTestClass()
        {
            contents.Add("\t};");
        }

        private void OpenTestClassPublicSection(List<NodeHook> featureHooks)
        {
            contents.Add("\tpublic:");


            foreach (var featureHook in featureHooks)
            {
                contents.Add(string.Format("\t\t{0}{1}()", TestClassHookPrefix, featureHook.Name.ToUpper()));
            }
        }


        private void OpenTestClass(string featureName)
        {
            contents.Add(string.Format("\t{0}({1})", TestClassMacro, featureName));
            contents.Add("\t{");
        }

        private void OpenNameSpace(string name)
        {
            contents.Add(string.Format("namespace {0}", name));
            contents.Add("{");
        }

        private void BuildUsingStatements()
        {
            contents.Add("using namespace Microsoft::VisualStudio::CppUnitTestFramework;");
            contents.Add(string.Empty);
        }

        private void BuildIncludeStatements()
        {
            contents.Add("#include \"CppUnitTest.h\"");
            contents.Add("#include \"CppUnitTestHooks.h\"");
            contents.Add("#include \"trim.hpp\"");
            contents.Add("#include <vector>");
            contents.Add(string.Empty);
        }

        public void AddScenario(NodeScenario scenario, List<NodeHook> featureHooks, List<string> publicContents)
        {
            string temp = string.Empty;
            
            if ((scenario.Hooks.Count == 0) && (featureHooks.Count == 0))
            {
                publicContents.Add(string.Format("\t\t{0}({1})", TestMethodPrefix, scenario.Name));
            }
            else if ((scenario.Hooks.Count == 0) && (featureHooks.Count > 0))
            {
                foreach (var featureHook in featureHooks)
                {
                    temp += "_" + featureHook.Name;
                }

                publicContents.Add(string.Format("\t\t{0}{1}({2})", TestMethodHookPrefix, temp.ToUpper(), scenario.Name));
            }
            else if ((scenario.Hooks.Count > 0) && (featureHooks.Count == 0))
            {
                foreach (var scenarioHook in scenario.Hooks)
                {
                    temp += "_" + scenarioHook.Name;
                }

                publicContents.Add(string.Format("\t\t{0}{1}({2})", TestMethodHookPrefix, temp.ToUpper(), scenario.Name));
            }
            else
            {
                foreach (var featureHook in featureHooks)
                {
                    temp += "_" + featureHook.Name;
                }

                foreach (var scenarioHook in scenario.Hooks)
                {
                    temp += "_" + scenarioHook.Name;
                }

                publicContents.Add(string.Format("\t\t{0}{1}({2})", TestMethodHookPrefix, temp.ToUpper(), scenario.Name));
            }
        }

        private void AddStepsForScenario(List<NodeStep> steps, NodeScenario currentScenario, List<NodeScenario> scenarios, List<string> privateContents)
        {
            if ((steps.Count > 0) && (scenarios.First() == currentScenario))
            {
                OpenTestClassPrivateSection(privateContents);
            }

            foreach (var step in steps)
            {             
                List<string> parametersInStep = new List<string>();

                if ((step.Parameters.Count == 0) && (step.Rows.Count == 0))
                {
                    privateContents.Add(string.Format("\t\tvoid {0}();", step.Name));
                }
                else if ((step.Parameters.Count > 0) && (step.Rows.Count == 0))
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    privateContents.Add(string.Format("\t\tvoid {0}({1}", step.Name, string.Join(", ", parametersInStep) + ");"));
                }
                else if ((step.Parameters.Count == 0) && (step.Rows.Count > 0))
                {
                    privateContents.Add(string.Format("\t\tvoid {0}(std::vector<std::vector<std::string>> table, int rows, int cols);", step.Name));
                }
                else
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    privateContents.Add(string.Format("\t\tvoid {0}(std::vector<std::vector<std::string>> table, int rows, int cols, {1}", step.Name, string.Join(", ", parametersInStep) + ");"));
                }
            }
        }

        private void OpenTestClassPrivateSection(List<string> privateContents)
        {
            privateContents.Add(string.Empty);
            privateContents.Add("\tprivate:");
        }
    }
}
