using System.Collections.Generic;

namespace StepAnalyzer.Json
{
    public class UserModelToJson
    {
        public string Name { get; set; }
        public int AverageSteps { get; set; }
        public int MaxSteps { get; set; }
        public int MinSteps { get; set; }

        public List<UserModelWithoutName> UserModelWithoutName { get; set; }
    }
}