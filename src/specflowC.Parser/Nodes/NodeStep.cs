using specflowC.Parser.Enums;
using System.Collections.Generic;

namespace specflowC.Parser.Nodes
{
    public class NodeStep
    {
        public string Name { get; set; }

        public List<Parameter> Parameters { get { return _parameters; } }

        private List<Parameter> _parameters = new List<Parameter>();

        public List<string[]> Rows { get; set; }

        public NodeStep()
        {
            Name = string.Empty;
            Rows = new List<string[]>();
        }

        public NodeStep(TokenGherkinStep tokens)
        {
            Name = tokens.MethodName;
            Rows = new List<string[]>();
            MapParameters(tokens.ParameterTokens);
        }

        public bool Equals(NodeStep otherNodeStep)
        {
            return nodeNamesAreEqual(otherNodeStep) &&
                    parameterCountIsEqual(otherNodeStep) &&
                    bothStepsHaveTablesOrDontHaveTables(otherNodeStep) &&
                    parametersHaveSameType(otherNodeStep);
        }

        private bool parametersHaveSameType(NodeStep otherNodeStep)
        {
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                if (this.Parameters[i].Type != otherNodeStep.Parameters[i].Type)
                {
                    return false;
                }
            }
            return true;
        }

        private bool bothStepsHaveTablesOrDontHaveTables(NodeStep otherNodeStep)
        {
            return ((this.Rows.Count > 0) && (otherNodeStep.Rows.Count > 0))
                   ||
                   ((this.Rows.Count == 0) && (otherNodeStep.Rows.Count == 0));
        }

        private bool parameterCountIsEqual(NodeStep otherNodeStep)
        {
            return (this.Parameters.Count == otherNodeStep.Parameters.Count);
        }

        private bool nodeNamesAreEqual(NodeStep otherNodeStep)
        {
            return (this.Name == otherNodeStep.Name);
        }

        private void MapParameters(List<string> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            _parameters.Clear();
            int count = 0;
            foreach (var parameterValueFromFeatureFile in parameters)
            {
                Parameters.Add(CreateParameter(count, parameterValueFromFeatureFile));
                count++;
            }
        }

        private Parameter CreateParameter(int id, string parameterValueFromFeatureFile)
        {
            string parameterValueToCodeGenerator;
            bool isFromExampleTable = false;
            if (parameterValueFromFeatureFile.Contains(EnumNames.tickOpen))
            {
                parameterValueToCodeGenerator = parameterValueFromFeatureFile.Substring(1, parameterValueFromFeatureFile.Length - 2);
                isFromExampleTable = true;
            }
            else
            {
                parameterValueToCodeGenerator = parameterValueFromFeatureFile;
                isFromExampleTable = false;
            }

            var p = new Parameter()
            {
                Name = string.Format("p{0}", id),
                Type = GetParameterType(isFromExampleTable, parameterValueToCodeGenerator),
                Value = parameterValueToCodeGenerator,
                IsFromExampleTable = isFromExampleTable
            };
            return p;
        }

        private string GetParameterType(bool isFromExampleTable, string parameterValueToCodeGenerator)
        {
            int itemp;
            decimal dtemp;

            if (isFromExampleTable)
            {
                return "string";
            }
            if (int.TryParse(parameterValueToCodeGenerator, out itemp))
            {
                return "int";
            }
            else if (decimal.TryParse(parameterValueToCodeGenerator, out dtemp))
            {
                return "decimal";
            }
            else
            {
                return "string";
            }
        }
    }
}