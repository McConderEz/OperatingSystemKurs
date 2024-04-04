using Newtonsoft.Json;

namespace FileSystemNTFS.BL.Controller
{
    public class SerializableSaver : IDataSaver
    {
        public T Load<T>() where T : class
        {
            var fileName = typeof(T).Name + ".json";

            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                if (!string.IsNullOrEmpty(json))
                {
                    var item = JsonConvert.DeserializeObject<T>(json);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            return default(T);
        }

        public void Save<T>(T item) where T : class
        {
            var fileName = typeof(T).Name + ".json";
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
    }
}