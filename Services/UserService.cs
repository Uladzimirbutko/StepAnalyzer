using System;
using System.Collections.Generic;
using System.Linq;
using StepAnalyzer.Entities;
using StepAnalyzer.Json;

namespace StepAnalyzer.Services
{
    public class UserService : IUserService
    {

        public List<string> GetAllNames(Dictionary<string,List<int>> users)
        {
            var userNames = users.Keys.ToList();
            return userNames;
        }

        public User GetUserByName (Dictionary<string, List<int>> users, string name)
        {
            var listSteps = users.FirstOrDefault(pair => pair.Key.Equals(name)).Value;
            var user = new User()
            {
                Name = name,
                AverageSteps = Convert.ToInt32(listSteps.Average()),
                MinSteps = MinValuesFromUser(listSteps),
                MaxSteps = MaxValuesFromUser(listSteps)
            };
            return user;
        }

        public List<int> UserStepsByName(Dictionary<string, List<int>> users, string name)
        {
            var listSteps = users.FirstOrDefault(pair => pair.Key.Equals(name)).Value;
            return listSteps;
        }

        public string WriteToJson(Dictionary<string, List<int>> users, User user)
        {
            var serialize = new JsonSerialize();
            var result = serialize.Serialize(users, user);
            return result;
        }

        private int MinValuesFromUser(List<int> users)
        {
            return users.Min();
        }

        private int MaxValuesFromUser(List<int> users)
        {
            return users.Max();
        }
    }
}