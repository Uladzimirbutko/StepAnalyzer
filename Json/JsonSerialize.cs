using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using StepAnalyzer.Entities;

namespace StepAnalyzer.Json
{
    public class JsonSerialize
    {
        public string Serialize(Dictionary<string, List<int>> users, User user)
        {
            try
            {
                var userModel = JsonDeserialize.UserModelForJson;
                var userList = userModel.Where(userByName => userByName.User.Equals(user.Name)).ToList();
                var userModelWithoutName = new List<UserModelWithoutName>();
                foreach (var usersModelForJson in userList)
                {
                    var userModelWithout = new UserModelWithoutName()
                    {
                        Rank = usersModelForJson.Rank,
                        Day = usersModelForJson.Day,
                        Status = usersModelForJson.Status,
                        Steps = usersModelForJson.Steps
                    };
                    userModelWithoutName.Add(userModelWithout);
                }

                var userModelToJson = new UserModelToJson()
                {
                    Name = user.Name,
                    AverageSteps = user.AverageSteps,
                    MaxSteps = user.MaxSteps,
                    MinSteps = user.MinSteps,
                    UserModelWithoutName = userModelWithoutName
                };
                var options = new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };
                var serialize = JsonSerializer.Serialize<UserModelToJson>(userModelToJson, options);
                string path = Environment.CurrentDirectory + $"\\{user.Name}.json";
                File.WriteAllText(path, serialize);
                return $"Сохранение успешно. Путь к файлу {path}";
            }
            catch (Exception e)
            {
                return $"Во время сохранения произошла ошибка: {e.Message}";
            }
        }
    }
}