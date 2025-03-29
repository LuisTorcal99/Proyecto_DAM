using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Proyecto_DAM.Interfaces;


namespace Proyecto_DAM.Service
{
    public class FileService<T> : IFileProvider<T> where T : class
    {
        public IEnumerable<T> Load(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();
            var content = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(content) ?? new List<T>();
        }

        public void Save(string filePath, IEnumerable<T> data)
        {
            var content = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, content);
        }
    }
}
