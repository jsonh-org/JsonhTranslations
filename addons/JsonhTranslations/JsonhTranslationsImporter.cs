#if TOOLS
using _Godot = global::Godot;
using _GodotCollections = global::Godot.Collections;

namespace JsonhTranslations;

[_Godot.Tool]
public partial class JsonhTranslationsImporter : _Godot.EditorImportPlugin {
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
    public override _GodotCollections.Array<_GodotCollections.Dictionary> _GetImportOptions(string Path, int PresetIndex) {
        return [];
    }
    public override int _GetImportOrder() {
        return 0;
    }
    public override float _GetPriority() {
        return 1.0f;
    }
    public override _Godot.Error _Import(string SourceFile, string SavePath, _GodotCollections.Dictionary Options, _GodotCollections.Array<string> PlatformVariants, _GodotCollections.Array<string> GenFiles) {
        _Godot.Translation Translation = JsonhTranslationsExtensions.CreateTranslationFromFile(SourceFile);
        return _Godot.ResourceSaver.Save(Translation, $"{SavePath}.{_GetSaveExtension()}");
    }
}
#endif