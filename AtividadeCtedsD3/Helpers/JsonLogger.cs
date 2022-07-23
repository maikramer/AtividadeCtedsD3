using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AtividadeCtedsD3.Helpers;

public static class JsonLogger<T>
{
    public static void WriteToLocalFile(T obj, string fileName)
    {
        var localDirectory = Directory.GetCurrentDirectory();
        if (!Directory.Exists(localDirectory)) Directory.CreateDirectory(localDirectory);

        var path = Path.Join(localDirectory, fileName + ".json");
        File.AppendAllLines(path, new[] { JsonConvert.SerializeObject(obj) });
    }

    public static List<T> ReadFromLocalFile(string fileName)
    {
        var list = new List<T>();
        var localDirectory = Directory.GetCurrentDirectory();
        var path = Path.Join(localDirectory, fileName + ".json");
        if (!File.Exists(path)) return list;
        var json = File.ReadAllText(path);
        var jsonReader = new JsonTextReader(new StringReader(json))
        {
            SupportMultipleContent = true // This is important!
        };

        var jsonSerializer = new JsonSerializer();
        while (jsonReader.Read())
        {
            list.Add(jsonSerializer.Deserialize<T>(jsonReader));
        }

        return list;
    }
}