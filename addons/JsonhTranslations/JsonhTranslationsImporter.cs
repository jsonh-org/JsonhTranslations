#if TOOLS
#nullable enable

namespace JsonhTranslations;

[Godot.Tool]
public partial class JsonhTranslationsImporter : Godot.EditorImportPlugin {
    public override string _GetImporterName() {
        return "JsonhTranslationsImporter";
    }
    public override string _GetVisibleName() {
        return "JSONH Translation";
    }
    public override string[] _GetRecognizedExtensions() {
        return ["jsonh"];
    }
    public override string _GetSaveExtension() {
        return "translation";
    }
    public override string _GetResourceType() {
        return "Translation";
    }
    public override int _GetPresetCount() {
        return 1;
    }
    public override string _GetPresetName(int PresetIndex) {
        return "Default";
    }
    public override Godot.Collections.Array<Godot.Collections.Dictionary> _GetImportOptions(string Path, int PresetIndex) {
        return [
            new Godot.Collections.Dictionary() {
                ["name"] = "output_source_text",
                ["default_value"] = false,
            },
        ];
    }
    public override int _GetImportOrder() {
        return 0;
    }
    public override float _GetPriority() {
        return 1.0f;
    }
    public override Godot.Error _Import(string SourceFile, string SavePath, Godot.Collections.Dictionary Options, Godot.Collections.Array<string> PlatformVariants, Godot.Collections.Array<string> GenFiles) {
        Godot.Translation Translation = JsonhTranslationsExtensions.CreateTranslationFromFile(SourceFile);

        if ((bool)Options["output_source_text"]) {
            string Source = Godot.FileAccess.GetFileAsString(SourceFile);
            Translation.SetMeta("source_text", Source);
        }

        return Godot.ResourceSaver.Save(Translation, $"{SavePath}.{_GetSaveExtension()}");
    }
}
#endif