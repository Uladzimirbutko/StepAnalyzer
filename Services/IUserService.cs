using System.Collections.Generic;
using StepAnalyzer.Entities;

namespace StepAnalyzer.Services
{
    public interface IUserService
    {
        List<string> GetAllNames(Dictionary<string, List<int>> users);
        User GetUserByName(Dictionary<string, List<int>> users, string name);
        List<int> UserStepsByName(Dictionary<string, List<int>> users, string name);

        string WriteToJson(Dictionary<string, List<int>> users, User user);

    }
}