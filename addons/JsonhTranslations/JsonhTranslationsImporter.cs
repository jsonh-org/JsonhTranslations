using _System = global::System;
using _Json = global::System.Text.Json;
using _JsonNodes = global::System.Text.Json.Nodes;
using _Godot = global::Godot;
using _GodotCollections = global::Godot.Collections;
using _JsonhCs = global::JsonhCs;

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
    public override Error _Import(string SourceFile, string SavePath, _GodotCollections.Dictionary Options, _GodotCollections.Array<string> PlatformVariants, _GodotCollections.Array<string> GenFiles) {
        _Godot.Translation Translation = new();

        string Locale = SourceFile.GetBaseName();
        Translation.Locale = Locale;

        string Jsonh = _Godot.FileAccess.GetFileAsString(SourceFile);
        _JsonNodes.JsonNode? Node = _JsonhCs.JsonhReader.ParseNode(Jsonh).Value;
        AddEntries(Translation, Node);

        return _Godot.ResourceSaver.Save(Translation, $"{SavePath}.{_GetSaveExtension()}");
    }

    private static void AddEntries(_Godot.Translation Translation, _JsonNodes.JsonNode? Node, string Prefix = "") {
        switch (Node) {
            case null:
                return;
            case _JsonNodes.JsonObject Object:
                foreach (KeyValuePair<string, _JsonNodes.JsonNode?> Property in Object) {
                    switch (Property.Value?.GetValueKind()) {
                        case _Json.JsonValueKind.String:
                            Translation.AddMessage($"{Prefix}{Property.Key}", (string)Property.Value!);
                            break;
                        case _Json.JsonValueKind.Object:
                        case _Json.JsonValueKind.Array:
                            AddEntries(Translation, Property.Value, Prefix: $"{Prefix}{Property.Key}.");
                            break;
                        default:
                            throw new _System.NotImplementedException(Property.Value?.ToJsonString() ?? "null");
                    }
                }
                break;
            case _JsonNodes.JsonArray Array:
                foreach ((int Index, _JsonNodes.JsonNode? Item) in Array.Index()) {
                    switch (Item?.GetValueKind()) {
                        case _Json.JsonValueKind.String:
                            Translation.AddMessage($"{Prefix}{Index}", (string)Item!);
                            break;
                        case _Json.JsonValueKind.Object:
                        case _Json.JsonValueKind.Array:
                            AddEntries(Translation, Item, Prefix: $"{Prefix}{Index}.");
                            break;
                        default:
                            throw new _System.NotImplementedException(Item?.ToJsonString() ?? "null");
                    }
                }
                break;
            default:
                throw new _System.NotSupportedException(Node.GetType().Name);
        }
    }
}