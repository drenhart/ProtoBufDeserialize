// See https://aka.ms/new-console-template for more information
using Aapt.Pb;
using Android.Bundle;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml.Serialization;

namespace ProtoBufDeserialize;

class Program {
    static void Main(string[] args) {
        //ChangeAppName("new appname");
        //var resourcePbFile = Path.Combine(Directory.GetCurrentDirectory(), "pb", "resources.pb");
        //var outputPath = @"C:\Users\soumi\Desktop\tempfolder\resources.json";
        //pbToJson(resourcePbFile, outputPath);

        var dependenciesPbFile = Path.Combine(Directory.GetCurrentDirectory(), "pb", "dependencies.pb");
        var outputPath = @"C:\Users\soumi\Desktop\tempfolder\dependencies.json";
        pbToJsonDependencies(dependenciesPbFile, outputPath);
    }

    static void pbToJsonDependencies(string pbPath, string outputPath) {
        AppDependencies dependencies;
        using (var file = File.OpenRead(pbPath)) {
            dependencies = AppDependencies.Parser.ParseFrom(file);
        }
        string str = JsonConvert.SerializeObject(dependencies, Formatting.Indented);
        File.WriteAllText(outputPath, str);
    }

    static void pbToJson(string pbPath, string outputPath) {
        ResourceTable resource;
        using (var file = File.OpenRead(pbPath)) {
            resource = ResourceTable.Parser.ParseFrom(file);
        }
        string str = JsonConvert.SerializeObject(resource, Formatting.Indented);
        File.WriteAllText(outputPath, str);
    }

    static void ChangeAppName(string newName) {
        var resourcePbFile = Path.Combine(Directory.GetCurrentDirectory(), "pb", "resources.pb");
        ResourceTable resource;
        using (var file = File.OpenRead(resourcePbFile)) {
            resource = ResourceTable.Parser.ParseFrom(file);
        }

        foreach (var p in resource.Package) {
            var appNameEntry = p.Type.First(t => t.Name == "string")
                                .Entry.First(e => e.Name == "app_name");
            foreach (var e in appNameEntry.ConfigValue) {
                if (e.Value.Item.Str?.Value != null) {
                    e.Value.Item.Str.Value = newName;
                }
            }
        }

        File.Delete(resourcePbFile);

        using (var outfile = File.Create(resourcePbFile)) {
            resource.WriteTo(outfile);
        }
    }

}





