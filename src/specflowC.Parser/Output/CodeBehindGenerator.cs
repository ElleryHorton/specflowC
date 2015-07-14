using specflowC.Parser.Nodes;
using specflowC.Parser.Output;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser
{
    public class CodeBehindGenerator : Generator, IGenerate
    {
        private int _tableNumberInScenario;

        protected override string[] BuildContents(NodeFeature feature)
        {
            BuildIncludeStatements(feature.Name);
            OpenNameSpace(LanguageConfig.NameSpace);
            BuildScenarioMethods(feature.Name, feature.Scenarios);
            CloseNameSpace();

            return Contents.ToArray();
        }

        private void BuildScenarioMethods(string featureName, List<NodeScenario> scenarios)
        {
            foreach (var scenario in scenarios)
            {
                OpenScenarioMethod(featureName, scenario.Name);

                RepeatStepsIfScenarioOutline(scenario);

                CloseScenarioMethod();
            }
        }

        private void RepeatStepsIfScenarioOutline(NodeScenario scenario)
        {
            var examples = GetExamples(scenario);
            int repeat = 1;
            _tableNumberInScenario = -1;
            int countOfRowsExcludingHeaderRow = examples.Rows.Count - 1;
            do
            {
                foreach (var step in scenario.Steps)
                {
                    BuildStepMethodCall(step, examples, repeat);
                }
                repeat++;
            } while (repeat <= countOfRowsExcludingHeaderRow);
        }

        private static NodeExamples GetExamples(NodeScenario scenario)
        {
            if (scenario is NodeScenarioOutline)
            {
                return ((NodeScenarioOutline)scenario).Examples;
            }
            else
            {
                return new NodeExamples();
            }
        }

        private void CloseNameSpace()
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add("}");
            }
        }

        private void CloseScenarioMethod()
        {
            Contents.Add("\t}");
        }

        private void BuildStepMethodCall(NodeStep step, NodeExamples examples, int exampleIndex)
        {
            // if we have rows and first time thru scenario outline
            if (step.Rows.Count > 0 && exampleIndex == 1) // TODO: remove exampleIndex == 1 if parameterizing table arguments
            {
                _tableNumberInScenario++;

                // TODO: could pass in NodeExamples examples to support parameterizing table arguments
                BuildTestDataTable(step.Rows, Contents);
            }

            Contents.Add(string.Format("\t\t{0};", LanguageConfig.StepMethod(step.Name, BuildParameterString(step.Parameters, step.Rows, examples, exampleIndex))));
        }

        private string BuildParameterString(List<Parameter> parameters, List<string[]> rows, NodeExamples examples, int exampleIndex)
        {
            string retstr;
            if (rows.Count > 0)
            {
                if (parameters.Count > 0)
                {
                    retstr = string.Format("table{0},{1},{2},{3}", _tableNumberInScenario, rows.Count, rows[0].Length, BuildParameterStringFromStepParameters(parameters, examples, exampleIndex));
                }
                else
                {
                    retstr = string.Format("table{0},{1},{2}", _tableNumberInScenario, rows.Count, rows[0].Length);
                }
            }
            else
            {
                retstr = BuildParameterStringFromStepParameters(parameters, examples, exampleIndex);
            }
            return retstr;
        }

        private static string BuildParameterStringFromStepParameters(List<Parameter> parameters, NodeExamples examples, int exampleIndex)
        {
            string parameterString = string.Empty;
            foreach (var parameter in parameters)
            {
                string parameterType = parameter.Type;
                string parameterValue = parameter.Value;

                if (parameter.IsFromExampleTable)
                {
                    int index = examples.GetIndexOfParameter(parameter.Value);
                    if (index >= 0)
                    {
                        parameterType = "string";
                        parameterValue = examples.Rows[exampleIndex][index];
                    }
                }

                if (parameterString != string.Empty)
                {
                    parameterString += ", ";
                }
                if (parameterType == "string")
                {
                    parameterString += string.Format("\"{0}\"", parameterValue);
                }
                else
                {
                    parameterString += parameterValue;
                }
            }
            return parameterString;
        }

        private void BuildTestDataTable(List<string[]> rows, List<string> contents)
        {
            if (rows.FirstOrDefault(row => row.Length != rows[0].Length) != null)
            {
                contents.Add(string.Format("\t\t{0}", LanguageConfig.ErrorTableParse));
            }

            contents.Add(string.Format("\t\t{0}", LanguageConfig.TableImplementationOpen(_tableNumberInScenario)));

            // TODO: replace parameters in SpecFlow table? Not yet a SpecFlow feature
            // http://stackoverflow.com/questions/20370854/specflow-use-parameters-in-a-table-with-a-scenario-context
            // http://stackoverflow.com/questions/5118860/specflow-cucumber-gherkin-using-tables-in-a-scenario-outline

            int rowIndex = 0;
            while (rowIndex < rows.Count - 1) // all but the last one
            {
                contents.Add(string.Format("\t\t\t{{ \"{0}\" }},", string.Join("\", \"", rows[rowIndex]))); // comma
                rowIndex++;
            }
            if (rowIndex < rows.Count) // the last one doesn't have a comma
            {
                contents.Add(string.Format("\t\t\t{{ \"{0}\" }}", string.Join("\", \"", rows[rowIndex]))); // no comma
            }
            contents.Add(string.Format("\t\t{0}", LanguageConfig.TableImplementationClose));
        }

        private void OpenScenarioMethod(string featureName, string scenarioName)
        {
            Contents.Add(string.Format("\tvoid {0}::{1}()", featureName, scenarioName));
            Contents.Add("\t{");
        }

        private void OpenNameSpace(string namespaceName)
        {
            if (LanguageConfig.UseNamespace)
            {
                Contents.Add(string.Format("namespace {0}", namespaceName));
                Contents.Add("{");
            }
        }

        private void BuildIncludeStatements(string featureName)
        {
            if (LanguageConfig.UseInclude)
            {
                foreach (var statement in LanguageConfig.includeStatementsInScenarios)
                {
                    Contents.Add(statement);
                }
                Contents.Add(string.Empty);
            }
        }
    }
}