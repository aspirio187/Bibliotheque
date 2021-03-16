using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public static class LocalFileHelper
    {
        public static async Task<bool> WriteJsonFile(string path, object obj)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;
            if (obj == null) return false;
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(obj));
            return true;
        }

        public static async Task<T> ReadJsonFile<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));
        }
    }
}
