using specflowC.Parser.Nodes;
using specflowC.Parser.Output;
using specflowC.Parser.Output.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser
{
    public class HeaderGenerator : Generator, IGenerate
    {
        protected override string[] BuildContents(NodeFeature feature)
        {
            BuildIncludeStatements();
            BuildUsingStatements();
            OpenNameSpace(LanguageConfig.NameSpace);
            OpenTestClass(feature.Name);
            BuildTestClass(feature);
            CloseTestClass();
            CloseNameSpace();

            return Contents.ToArray();
        }

        private void BuildTestClass(NodeFeature feature)
        {
            List<string> privateContents = new List<string>();
            List<string> publicContents = new List<string>();
            List<NodeStep> filterUniqueSteps = new List<NodeStep>();

            foreach (var scenario in feature.Scenarios)
            {
                AddScenario(scenario, feature.Hooks, publicContents);
                if ((scenario.Steps.Count > 0) && (feature.Scenarios.First() == scenario))
                {
                    OpenTestClassPrivateSection(privateContents);
                }
                AddSteps(GeneratorHelper.FindUniqueSteps(filterUniqueSteps, scenario.Steps), scenario, privateContents);
            }

            OpenTestClassPublicSection(feature.Hooks);

            Contents.AddRange(publicContents);

            if (privateContents.Count > 0)
            {
                Contents.AddRange(privateContents);
            }
        }

        private void CloseNameSpace()
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add("}");
            }
        }

        private void CloseTestClass()
        {
            Contents.Add("\t};");
        }

        private void OpenTestClassPublicSection(List<NodeHook> featureHooks)
        {
            Contents.Add(string.Format("\t{0}", LanguageConfig.PublicDeclaration));

            foreach (var featureHook in featureHooks)
            {
                Contents.Add(string.Format("\t\t{0}", LanguageConfig.FeatureClassInnerAttribute(featureHook.Name)));
            }
        }

        private void OpenTestClass(string featureName)
        {
            Contents.Add(string.Format("\t{0}", LanguageConfig.FeatureClassDeclaration));
            Contents.Add("\t{");
        }

        private void OpenNameSpace(string name)
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add(string.Format("namespace {0}", name));
                Contents.Add("{");
            }
        }

        private void BuildUsingStatements()
        {
            foreach (var statement in LanguageConfig.usingStatementsInStatementsHeader)
            {
                Contents.Add(statement);
            }
            Contents.Add(string.Empty);
        }

        private void BuildIncludeStatements()
        {
            if (LanguageConfig.UseInclude)
            {
                foreach (var statement in LanguageConfig.includeStatementsInHeader)
                {
                    Contents.Add(statement);
                }
                Contents.Add(string.Empty);
            }
        }

        private void AddScenario(NodeScenario scenario, List<NodeHook> featureHooks, List<string> publicContents)
        {
            string temp = string.Empty;
            foreach (var scenarioHook in scenario.Hooks)
            {
                temp += "_" + scenarioHook.Name;
            }
            publicContents.Add(string.Format("\t\t{0};", LanguageConfig.ScenarioMethodDeclaration(temp, scenario.Name)));
        }

        private void AddSteps(List<NodeStep> steps, NodeScenario currentScenario, List<string> privateContents)
        {
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
            privateContents.Add(string.Format("\t{0}", LanguageConfig.PrivateDeclaration));
        }
    }
}