using Newtonsoft.Json;

namespace FileSystemNTFS.BL.Controller
{
    public class SerializableSaver : IDataSaver
    {
        public T Load<T>() where T : class
        {

            var typeName = typeof(T).Name;

            if (typeof(T).IsGenericType)
            {
                var genericArguments = typeof(T).GetGenericArguments();
                typeName = genericArguments[0].Name;
            }

            var fileName = typeName + ".json";

            
            
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
            var typeName = typeof(T).Name;

            if (typeof(T).IsGenericType)
            {
                var genericArguments = typeof(T).GetGenericArguments();
                typeName = genericArguments[0].Name;
            }

            var fileName = typeName + ".json";
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            var json = JsonConvert.SerializeObject(item, jsonSettings);
            File.WriteAllText(fileName, json);
        }
    }
}