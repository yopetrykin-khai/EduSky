using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace EduSky
{
    public class JsonDataService<T> : IDataService<T>
    {
        private readonly string _filePath;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> Load()
        {
            throw new NotImplementedException();
        }

        public void Save(List<T> items)
        {
            throw new NotImplementedException();
        }
    }
}
