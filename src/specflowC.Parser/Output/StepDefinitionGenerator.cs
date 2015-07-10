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
            BuildIncludeStatement();
            BuildHeaderStatement();
            OpenNameSpace();
            BuildStepClass(feature.Name, feature.Scenarios);
            CloseNameSpace();

            return Contents.ToArray();
        }

        private void BuildIncludeStatement()
        {
            if (LanguageConfig.UseInclude)
            {
                Contents.AddRange(LanguageConfig.includeStatementsInStepDefinition);
                Contents.Add(string.Empty);
            }
        }

        private void BuildHeaderStatement()
        {
            if (LanguageConfig.UseHeader)
            {
                Contents.AddRange(LanguageConfig.headerStatementsInStepDefinition);
                Contents.Add(string.Empty);
            }
        }

        private void OpenNameSpace()
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add(string.Format("namespace {0}", LanguageConfig.NameSpace));
                Contents.Add("{");
            }
        }

        private void BuildStepClass(string featureName, IList<NodeScenario> scenarios)
        {
            List<NodeStep> filterUniqueSteps = new List<NodeStep>();

            foreach (var scenario in scenarios)
            {
                if (scenario.Steps.Count > 0)
                {
                    AddSteps(featureName, GeneratorHelper.FindUniqueSteps(filterUniqueSteps, scenario.Steps));
                    if (scenarios.Last() != scenario)
                    {
                        Contents.Add(string.Empty);
                    }
                }
            }
        }

        private void AddSteps(string featureName, IList<NodeStep> steps)
        {
            foreach (var step in steps)
            {
                List<string> parametersInStep = new List<string>();

                if ((step.Parameters.Count == 0) && (step.Rows.Count == 0))
                {
                    Contents.Add(string.Format("\tvoid {0}::{1}()", featureName, step.Name));
                }
                else if ((step.Parameters.Count > 0) && (step.Rows.Count == 0))
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    Contents.Add(string.Format("\tvoid {0}::{1}({2})", featureName, step.Name, string.Join(", ", parametersInStep)));
                }
                else if ((step.Parameters.Count == 0) && (step.Rows.Count > 0))
                {
                    Contents.Add(string.Format("\tvoid {0}::{1}({2})", featureName, step.Name, LanguageConfig.TableDeclaration));
                }
                else
                {
                    foreach (var Parameter in step.Parameters)
                    {
                        parametersInStep.Add(Parameter.Type + " " + Parameter.Name);
                    }
                    Contents.Add(string.Format("\tvoid {0}::{1}({2}, {3})", featureName, step.Name, LanguageConfig.TableDeclaration, string.Join(", ", parametersInStep)));
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

        private void CloseMethod(NodeStep currentStep, IList<NodeStep> steps)
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