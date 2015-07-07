using specflowC.Parser.Nodes;
using specflowC.Parser.Output;
using specflowC.Parser.Output.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser
{
    public class StepDefinitionGenerator : Generator, IGenerate
    {
        protected override string[] BuildContents(NodeFeature feature)
        {
            BuildIncludeStatement(feature.Name);
            OpenNameSpace(LanguageConfig.NameSpace);
            BuildStepClass(feature);
            CloseNameSpace();

            return Contents.ToArray();
        }

        private void BuildIncludeStatement(string featureName)
        {
            if (LanguageConfig.UseInclude)
            {
                foreach (var statement in LanguageConfig.includeStatementsInStepDefinition)
                {
                    Contents.Add(statement);
                }
                Contents.Add(string.Empty);
            }
        }

        private void OpenNameSpace(string namespaceName)
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add(string.Format("namespace {0}", namespaceName));
                Contents.Add("{");
            }
        }

        private void BuildStepClass(NodeFeature feature)
        {
            List<NodeStep> filterUniqueSteps = new List<NodeStep>();

            foreach (var scenario in feature.Scenarios)
            {
                if (scenario.Steps.Count > 0)
                {
                    AddSteps(feature, GeneratorHelper.FindUniqueSteps(filterUniqueSteps, scenario.Steps));
                    if (feature.Scenarios.Last() != scenario)
                    {
                        Contents.Add(string.Empty);
                    }
                }
            }
        }

        private void AddSteps(NodeFeature feature, List<NodeStep> steps)
        {
            foreach (var step in steps)
            {
                List<string> parametersInStep = new List<string>();

                if ((step.Parameters.Count == 0) && (step.Rows.Count == 0))
                {
                    Contents.Add(string.Format("\tvoid {0}::{1}()", feature.Name, step.Name));
                }
                else if ((step.Parameters.Count > 0) && (step.Rows.Count == 0))
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    Contents.Add(string.Format("\tvoid {0}::{1}({2})", feature.Name, step.Name, string.Join(", ", parametersInStep)));
                }
                else if ((step.Parameters.Count == 0) && (step.Rows.Count > 0))
                {
                    Contents.Add(string.Format("\tvoid {0}::{1}(std::vector<std::vector<std::string>> table, int rows, int cols)", feature.Name, step.Name));
                }
                else
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    Contents.Add(string.Format("\tvoid {0}::{1}(std::vector<std::vector<std::string>> table, int rows, int cols, {2})", feature.Name, step.Name, string.Join(", ", parametersInStep)));
                }

                OpenMethod();
                AddPendingStepImplementation();
                CloseMethod(step, steps);
            }
        }

        private void AddPendingStepImplementation()
        {
            Contents.Add(string.Format("\t\t{0}", LanguageConfig.PendingStepDeclaration));
        }

        private void OpenMethod()
        {
            Contents.Add("\t{");
        }

        private void CloseMethod(NodeStep currentStep, List<NodeStep> steps)
        {
            Contents.Add("\t}");
            if (steps.Last() != currentStep)
            {
                Contents.Add(string.Empty);
            }
        }

        private void CloseNameSpace()
        {
            if (LanguageConfig.UseNamespace)
            {
                if (Contents.Last().Equals(string.Empty))
                {
                    Contents.RemoveAt(Contents.Count - 1);
                    Contents.Add("}");
                }
                else
                {
                    Contents.Add("}");
                }
            }
        }
    }
}