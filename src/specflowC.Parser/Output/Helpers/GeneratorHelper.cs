using specflowC.Parser.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace specflowC.Parser.Output.Helpers
{
    public static class GeneratorHelper
    {
        public static List<NodeStep> FindUniqueSteps(List<NodeStep> filterUniqueSteps, List<NodeStep> steps)
        {
            List<NodeStep> stepsToPrint = new List<NodeStep>();
            foreach (var step in steps)
            {
                if (!filterUniqueSteps.Any(uniqueStep => uniqueStep.Equals(step)))
                {
                    filterUniqueSteps.Add(step);
                    stepsToPrint.Add(step);
                }
            }
            return stepsToPrint;
        }
    }
}