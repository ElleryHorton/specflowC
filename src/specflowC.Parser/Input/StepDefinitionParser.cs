using specflowC.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser
{
    public class StepDefinitionParser
    {
        private Dictionary<string, FeatureGroup> featureGroups = new Dictionary<string, FeatureGroup>();

        public List<FeatureGroup> Parse(string[] stepDefinitionFile)
        {
            IterateThroughStepDefinitionFile(stepDefinitionFile);

            return featureGroups.Values.ToList();
        }

        private void IterateThroughStepDefinitionFile(string[] stepDefinitionFile)
        {
            for (int i = 0; i < stepDefinitionFile.Length; i++)
            {
                FindStepDefinitionLineToParse(stepDefinitionFile, i);
            }
        }

        private void FindStepDefinitionLineToParse(string[] stepDefinitionFile, int i)
        {
            if (stepDefinitionFile[i].Contains("void"))
            {
                string newString = stepDefinitionFile[i];
                string stepDefinition = newString.TrimStart();

                ParseStepDefinition(stepDefinition);
            }
        }

        private void ParseStepDefinition(string stepDefinition)
        {
            string[] featureAndStepContents = ParseVoidAndColonStrings(stepDefinition);

            string stepNameAndParameters = featureAndStepContents[1];
            string featureName = featureAndStepContents[0];

            if (featureGroups.ContainsKey(featureName))
            {
                AddStep(stepNameAndParameters, featureGroups[featureName].Steps);
            }
            else
            {
                CreateNewFeatureGroup(stepNameAndParameters, featureName);
            }
        }

        private void CreateNewFeatureGroup(string stepNameAndParameters, string featureName)
        {
            FeatureGroup featureGroup = new FeatureGroup();
            List<NodeStep> steps = new List<NodeStep>();

            AddStep(stepNameAndParameters, steps);

            AddFeatureName(featureGroup, featureName, steps);

            featureGroups.Add(featureName, featureGroup);
        }

        private static string[] ParseVoidAndColonStrings(string stepDefinition)
        {
            string[] voidSplit = { "void " };
            string[] stringWithoutVoid = stepDefinition.Split(voidSplit, 2, StringSplitOptions.None);

            string[] colonSplit = { "::" };
            string[] featureAndStepName = stringWithoutVoid[1].Split(colonSplit, 2, StringSplitOptions.None);

            return featureAndStepName;
        }

        private FeatureGroup AddFeatureName(FeatureGroup featureGroup, string featureName, List<NodeStep> steps)
        {
            featureGroup.FeatureName = featureName;
            featureGroup.Steps = steps;

            return featureGroup;
        }

        private void AddStep(string stepContents, List<NodeStep> steps)
        {
            NodeStep step = new NodeStep();
            string openParenthesisSplit = "(";
            string[] tokens = stepContents.Split(openParenthesisSplit.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (HasParameters(tokens[1].ToString()))
            {
                AddParameters(step, tokens[1]);
            }
            step.Name = tokens[0];
            steps.Add(step);
        }

        private static bool HasParameters(string parameterString)
        {
            return parameterString.Trim() != ")";
        }

        private void AddParameters(NodeStep step, string parameterContents)
        {
            if (parameterContents.Contains(","))
            {
                AddMultipleParameters(step, parameterContents);
            }
            else
            {
                AddOneParameter(step, parameterContents);
            }
        }

        private void AddOneParameter(NodeStep step, string parameter)
        {
            char[] oneParameterDelimiter = { ' ', ')' };
            string[] oneParameter = parameter.Split(oneParameterDelimiter);

            Parameter p = new Parameter();
            p.Type = oneParameter[0];
            p.Name = oneParameter[1];
            step.Parameters.Add(p);
        }

        private void AddMultipleParameters(NodeStep step, string parameters)
        {
            char[] multipleParameterDelimiter = { ' ' };
            string[] arrayOfParameters = parameters.Split(multipleParameterDelimiter);

            for (int i = 0; i < arrayOfParameters.Length; i += 2)
            {
                Parameter p = new Parameter();
                p.Type = arrayOfParameters[i];

                if (arrayOfParameters[i + 1].Contains(","))
                {
                    char[] commaTrim = { ',' };
                    string[] parameter = arrayOfParameters[i + 1].Split(commaTrim);
                    p.Name = parameter[0].TrimEnd();
                }
                else if (arrayOfParameters[i + 1].Contains(")"))
                {
                    char[] parenthesisTrim = { ')' };
                    string[] lastParameter = arrayOfParameters[i + 1].Split(parenthesisTrim);
                    p.Name = lastParameter[0].TrimEnd();
                }
                step.Parameters.Add(p);
            }
        }
    }
}