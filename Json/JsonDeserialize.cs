using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StepAnalyzer.JsonReaderModel;

namespace StepAnalyzer.Json
{
    public class JsonDeserialize
    {
        public static List<UsersModelFromJson> UserModelForJson { get; set; }
        public List<UsersModelFromJson> JsonReader()
        {
            try
            {
                var result = UserModelForJson = new List<UsersModelFromJson>();
                var pathDirectory = Environment.CurrentDirectory + @"\TestData\";
                var files = Directory.GetFiles(pathDirectory, "*.json");
                byte count = 1;
                foreach (var file in files)
                {
                    var readJson = File.ReadAllText(file);
                    var users = JsonConvert.DeserializeObject<List<UsersModelFromJson>>(readJson);
                

                    foreach (var usersModelForJson in users)
                    {
                        usersModelForJson.Day = $"Day {count}";
                        result.Add(usersModelForJson);
                    }

                    count++;
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Dictionary<string, List<int>> UsersDictionary(List<UsersModelFromJson> users)
        {
            var usersDictionary = new Dictionary<string, List<int>>();
            var listSteps = new List<int>();
            
            foreach (var user in users)
            {
                if (usersDictionary.ContainsKey(user.User))
                {
                    continue;
                }
                var names = user.User;
                listSteps = users.Where(model => model.User.Equals(names)).Select(model => model.Steps).ToList();
                usersDictionary.Add(names, listSteps);
            }
            return usersDictionary;
        }
    }
}