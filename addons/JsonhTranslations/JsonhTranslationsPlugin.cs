#if TOOLS
#nullable enable

namespace JsonhTranslations;

[Godot.Tool]
public partial class JsonhTranslationsPlugin : Godot.EditorPlugin {
    private JsonhTranslationsImporter? JsonhTranslationsImporter;

    public override void _EnterTree() {
        JsonhTranslationsImporter = new JsonhTranslationsImporter();
        AddImportPlugin(JsonhTranslationsImporter);
    }
    public override void _ExitTree() {
        RemoveImportPlugin(JsonhTranslationsImporter!);
        JsonhTranslationsImporter = null;
    }
}
#endif