using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EduSky
{
    public class JsonDataService<T> : IDataService<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerSettings _settings;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        // Сохранение
        public void Save(List<T> items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented, _settings);
            File.WriteAllText(_filePath, json);
        }

        // Загрузка
        public List<T> Load()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(json, _settings) ?? new List<T>();
        }
    }
}
