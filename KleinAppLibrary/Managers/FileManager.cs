using KleinMapLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KleinMapLibrary.Managers
{
    public class FileManager
    {
        // ------ SINGLETON ------ //
        private static FileManager instance;
        public static FileManager Instance => instance = instance ?? new FileManager();

        private FileManager(){}

        public async Task<IEnumerable<Station>> LoadDataAsync(string provinceName, string directory)
        {
            IEnumerable<Station> stations = null;

            await Task.Run(() =>
            {
                using (StreamReader file = File.OpenText($"{directory}{provinceName}.json"))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        stations = serializer.Deserialize<IEnumerable<Station>>(reader);
                    }
                }
            });

            return stations;
        }

        public async Task SaveDataAsync(IEnumerable<Station> data, string provinceName, string directory)
        {
            if (! Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await Task.Run(() =>
            {
                using (StreamWriter file = File.CreateText($"{directory}{provinceName}.json"))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(file))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, data);
                    }
                }
            });
        }
    }
}
