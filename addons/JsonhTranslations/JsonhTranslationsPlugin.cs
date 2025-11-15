#if TOOLS
using _Godot = global::Godot;

namespace JsonhTranslations;

[_Godot.Tool]
public partial class JsonhTranslationsPlugin : _Godot.EditorPlugin {
    private JsonhTranslations.JsonhTranslationsImporter? JsonhTranslationsImporter;

    public override void _EnterTree() {
        JsonhTranslationsImporter = new JsonhTranslations.JsonhTranslationsImporter();
        AddImportPlugin(JsonhTranslationsImporter);
    }
    public override void _ExitTree() {
        RemoveImportPlugin(JsonhTranslationsImporter!);
        JsonhTranslationsImporter = null;
    }
}
#endif