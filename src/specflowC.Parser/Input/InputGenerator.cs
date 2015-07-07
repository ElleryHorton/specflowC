using specflowC.Parser.Enums;
using specflowC.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser
{
    public class InputGenerator
    {
        private List<NodeFeature> features = new List<NodeFeature>();
        private NodeFeature lastFeature = null;
        private NodeScenario lastScenario = null;
        private NodeStep lastStep = null;
        private List<NodeHook> featureHooks = new List<NodeHook>();
        private List<NodeHook> scenarioHooks = new List<NodeHook>();
        private List<NodeHook> hooksToAdd = null;
        private bool MakePreviousScenarioHookAFeatureHookHack = false;

        public List<NodeFeature> Load(string[] contents)
        {
            bool isBuildingExampleTable = false;
            bool isHeaderRow = false;

            // since we read top to bottom, sometimes we don't know if a hook is for the next scenario or a new feature
            foreach (string line in contents)
            {
                string formattedLine = line.Trim();
                string label = GherkinParser.DetermineLabel(formattedLine);

                if (label != EnumNames.slim)
                {
                    isBuildingExampleTable = false; // reset at start of new label other than slims
                }

                switch (label)
                {
                    case EnumNames.hook:
                        AddHook(formattedLine);
                        break;

                    case EnumNames.Feature:
                        AddFeature(formattedLine, label);
                        break;

                    case EnumNames.Scenario:
                        lastScenario = new NodeScenario(GherkinParser.ParseNameWithLabel(formattedLine, label), scenarioHooks);
                        AddScenario(lastScenario);
                        break;

                    case EnumNames.ScenarioOutline:
                        lastScenario = new NodeScenarioOutline(GherkinParser.ParseNameWithLabel(formattedLine, label), scenarioHooks);
                        AddScenario(lastScenario);
                        break;

                    case EnumNames.Examples:
                        isBuildingExampleTable = true;
                        isHeaderRow = true;
                        break;

                    case EnumNames.Given:
                    case EnumNames.When:
                    case EnumNames.Then:
                    case EnumNames.And:
                        AddStep(formattedLine);
                        break;

                    case EnumNames.slim:
                        if (isHeaderRow)
                        {
                            formattedLine = GherkinParser.RemoveWhiteSpace(formattedLine);
                        }
                        if (isBuildingExampleTable)
                        {
                            AddExampleRow(formattedLine);
                        }
                        else
                        {
                            AddTableRow(formattedLine);
                        }
                        isHeaderRow = false;
                        break;

                    default: // do nothing
                        break;
                }
            }

            return features;
        }

        private void AddHook(string formattedLine)
        {
            hooksToAdd = GherkinParser.GetHooks(formattedLine);
            if (features.Count == 0)
            {
                AddDistinctHooksByName(featureHooks, hooksToAdd);
                AddDistinctHooksByName(scenarioHooks, hooksToAdd);
            }
            else
            {
                AddDistinctHooksByName(scenarioHooks, hooksToAdd);
                MakePreviousScenarioHookAFeatureHookHack = true;
            }
        }

        private void AddDistinctHooksByName(List<NodeHook> existingHooks, List<NodeHook> toAdd)
        {
            foreach (var hookToAdd in toAdd)
            {
                if (ListOfHooksDoesNotContainHook(existingHooks, hookToAdd))
                {
                    existingHooks.Add(hookToAdd);
                }
            }
        }

        private static bool ListOfHooksDoesNotContainHook(List<NodeHook> existingHooks, NodeHook hookToAdd)
        {
            return existingHooks.FirstOrDefault(existingHook => existingHook.Name == hookToAdd.Name) == null;
        }

        private void AddFeature(string formattedLine, string label)
        {
            if (MakePreviousScenarioHookAFeatureHookHack)
            {
                MakePreviousScenarioHookAFeatureHook(); // undo previous scenario hook in scenarioHooks
                MakePreviousScenarioHookAFeatureHookHack = false;
            } // else no need to undo previous scenario hook
            lastFeature = new NodeFeature(GherkinParser.ParseNameWithLabel(formattedLine, label), featureHooks);
            features.Add(lastFeature);
        }

        private void MakePreviousScenarioHookAFeatureHook()
        {
            if (hooksToAdd == null)
            {
                return;
            }
            foreach (var hook in hooksToAdd)
            {
                scenarioHooks.Remove(hook);
            }
            lastScenario = null;
            lastStep = null;

            featureHooks = new List<NodeHook>();
            scenarioHooks = new List<NodeHook>();
            AddDistinctHooksByName(featureHooks, hooksToAdd);
            AddDistinctHooksByName(scenarioHooks, hooksToAdd);
        }

        private void AddScenario(NodeScenario scenario)
        {
            MakePreviousScenarioHookAFeatureHookHack = false; // any preceding hooks belong to this scenario
            lastFeature.Scenarios.Add(scenario);
        }

        private void AddExampleRow(string formattedLine)
        {
            if (lastScenario != null)
            {
                if (lastScenario is NodeScenarioOutline)
                {
                    ((NodeScenarioOutline)lastScenario).Examples.Rows.Add(ParseSlimRow(formattedLine));
                } // else do nothing
            }
        }

        private void AddStep(string formattedLine)
        {
            var tokens = GherkinParser.ParseOutStepAndParameters(formattedLine);
            if (tokens == null) // invalid gherkin step
            {
                lastStep = null; // so that any tables don't get added to the last valid step
            }
            else
            {
                lastStep = new NodeStep(tokens);
                lastScenario.Steps.Add(lastStep);
            }
        }

        private void AddTableRow(string formattedLine)
        {
            if (lastStep != null)
            {
                lastStep.Rows.Add(ParseSlimRow(formattedLine));
            } // else do nothing
        }

        private static string[] ParseSlimRow(string formattedLine)
        {
            var columns = formattedLine.Split(EnumNames.slim.ToCharArray());
            string[] subset_columns = new string[columns.Length - 2];
            Array.Copy(columns, 1, subset_columns, 0, columns.Length - 2);
            return subset_columns.Select(col => col.Trim()).ToArray();
        }
    }
}