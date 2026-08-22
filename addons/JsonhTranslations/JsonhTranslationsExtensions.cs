#nullable enable

namespace JsonhTranslations;

public static class JsonhTranslationsExtensions {
    /// <summary>
    /// Translate: Finds a translation for the message, and formats the given arguments.
    /// </summary>
    public static string Tr(this string Message, params scoped System.ReadOnlySpan<object?> FormatArgs) {
        return string.Format(Godot.TranslationServer.Translate(new Godot.StringName(Message)), FormatArgs);
    }
    /// <summary>
    /// Translate plural: Finds a translation for the message, first trying with the suffix appended, and formats the given arguments.
    /// </summary>
    public static string TrN(this string Message, string Suffix, params scoped System.ReadOnlySpan<object?> FormatArgs) {
        string MessageWithSuffix = $"{Message}{Suffix}";
        string TranslatedMessageWithSuffix = Godot.TranslationServer.Translate(new Godot.StringName(MessageWithSuffix));
        if (TranslatedMessageWithSuffix != MessageWithSuffix) {
            return string.Format(TranslatedMessageWithSuffix, FormatArgs);
        }
        else {
            return Tr(Message, FormatArgs);
        }
    }
    /// <summary>
    /// Translate array: Finds every translation for the message with the suffix formated with the index, and formats the given arguments.
    /// </summary>
    public static System.Collections.Generic.List<string> TrA(this string Message, string Suffix, params scoped System.ReadOnlySpan<object?> FormatArgs) {
        System.Collections.Generic.List<string> TranslatedMessages = [];
        for (int Index = 0; ; Index++) {
            string MessageWithSuffixAndIndex = $"{Message}{string.Format(Suffix, Index.ToString())}";
            string TranslatedMessageWithSuffixAndIndex = Godot.TranslationServer.Translate(new Godot.StringName(MessageWithSuffixAndIndex));
            if (TranslatedMessageWithSuffixAndIndex == MessageWithSuffixAndIndex) {
                break;
            }
            TranslatedMessages.Add(string.Format(TranslatedMessageWithSuffixAndIndex, FormatArgs));
        }
        return TranslatedMessages;
    }
    /// <summary>
    /// Creates a translation with the given locale and messages parsed from the given JSONH.
    /// </summary>
    public static Godot.Translation CreateTranslation(string Locale, string Jsonh) {
        Godot.Translation Translation = new() {
            Locale = Locale,
        };
        System.Text.Json.Nodes.JsonNode? Node = JsonhCs.JsonhReader.ParseNode(Jsonh, new JsonhCs.JsonhReaderOptions() {
            ParseSingleElement = true,
        }).Value;
        AddTranslationMessages(Translation, Node);
        return Translation;
    }
    /// <summary>
    /// Creates a translation with a locale from the JSONH filename and messages parsed from the JSONH file.
    /// </summary>
    public static Godot.Translation CreateTranslationFromFile(string JsonhFilePath) {
        string Locale = System.IO.Path.GetFileNameWithoutExtension(JsonhFilePath);
        string Jsonh = Godot.FileAccess.GetFileAsString(JsonhFilePath);
        return CreateTranslation(Locale, Jsonh);
    }

    private static void AddTranslationMessages(Godot.Translation Translation, System.Text.Json.Nodes.JsonNode? Node, scoped System.ReadOnlySpan<char> Prefix = "") {
        switch (Node) {
            case null:
                return;
            case System.Text.Json.Nodes.JsonObject Object:
                foreach (System.Collections.Generic.KeyValuePair<string, System.Text.Json.Nodes.JsonNode?> Property in Object) {
                    switch (Property.Value?.GetValueKind()) {
                        case System.Text.Json.JsonValueKind.String:
                            Translation.AddMessage(new Godot.StringName($"{Prefix}{Property.Key}"), new Godot.StringName((string)Property.Value!));
                            break;
                        case System.Text.Json.JsonValueKind.Object:
                        case System.Text.Json.JsonValueKind.Array:
                            AddTranslationMessages(Translation, Property.Value, Prefix: $"{Prefix}{Property.Key}.");
                            break;
                        default:
                            throw new System.NotImplementedException(Property.Value?.ToJsonString() ?? "null");
                    }
                }
                break;
            case System.Text.Json.Nodes.JsonArray Array:
                foreach ((int Index, System.Text.Json.Nodes.JsonNode? Item) in System.Linq.Enumerable.Index(Array)) {
                    switch (Item?.GetValueKind()) {
                        case System.Text.Json.JsonValueKind.String:
                            Translation.AddMessage(new Godot.StringName($"{Prefix}{Index}"), new Godot.StringName((string)Item!));
                            break;
                        case System.Text.Json.JsonValueKind.Object:
                        case System.Text.Json.JsonValueKind.Array:
                            AddTranslationMessages(Translation, Item, Prefix: $"{Prefix}{Index}.");
                            break;
                        default:
                            throw new System.NotImplementedException(Item?.ToJsonString() ?? "null");
                    }
                }
                break;
            default:
                throw new System.NotImplementedException(Node.GetType().Name);
        }
    }
}