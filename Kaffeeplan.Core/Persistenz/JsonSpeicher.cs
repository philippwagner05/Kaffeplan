using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kaffeeplan.Core.Persistenz
{
    public class JsonSpeicher
    {
        private static readonly JsonSerializerOptions Optionen = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public void Speichern<T>(T daten, string pfad)
        {
            var dir = Path.GetDirectoryName(pfad);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            
            string jsonString = JsonSerializer.Serialize(daten, Optionen);
            File.WriteAllText(pfad, jsonString);
        }

        public void CSVSpeichern<T>(T daten, string pfad)
        {
            var dir = Path.GetDirectoryName(pfad);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }

        public T? Laden<T>(string pfad)
        {
            string daten = File.ReadAllText(pfad);
            var jsonDaten = JsonSerializer.Deserialize<T>(daten);
            Console.WriteLine(jsonDaten);
            //throw new NotImplementedException();
            return jsonDaten;
        }
    }
}
