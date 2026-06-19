using _System = global::System;
using _SystemCollectionsGeneric = global::System.Collections.Generic;
using _SystemLinq = global::System.Linq;
using _SystemIO = global::System.IO;
using _Json = global::System.Text.Json;
using _JsonNodes = global::System.Text.Json.Nodes;
using _Godot = global::Godot;
using _JsonhCs = global::JsonhCs;

namespace JsonhTranslations;

public static class JsonhTranslationsExtensions {
    /// <summary>
    /// Translate: Finds a translation for the message, and formats the given arguments.
    /// </summary>
    public static string Tr(this string Message, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        return string.Format(_Godot.TranslationServer.Translate(new _Godot.StringName(Message)), FormatArgs);
    }
    /// <summary>
    /// Translate plural: Finds a translation for the message, first trying with the suffix appended, and formats the given arguments.
    /// </summary>
    public static string TrN(this string Message, string Suffix, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        string MessageWithSuffix = $"{Message}{Suffix}";
        string TranslatedMessageWithSuffix = _Godot.TranslationServer.Translate(new _Godot.StringName(MessageWithSuffix));
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
    public static _SystemCollectionsGeneric.List<string> TrA(this string Message, string Suffix, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        _SystemCollectionsGeneric.List<string> TranslatedMessages = [];
        for (int Index = 0; ; Index++) {
            string MessageWithSuffixAndIndex = $"{Message}{string.Format(Suffix, Index.ToString())}";
            string TranslatedMessageWithSuffixAndIndex = _Godot.TranslationServer.Translate(new _Godot.StringName(MessageWithSuffixAndIndex));
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
    public static _Godot.Translation CreateTranslation(string Locale, string Jsonh) {
        _Godot.Translation Translation = new() {
            Locale = Locale,
        };
        _JsonNodes.JsonNode? Node = _JsonhCs.JsonhReader.ParseNode(Jsonh, new _JsonhCs.JsonhReaderOptions() {
            ParseSingleElement = true,
        }).Value;
        AddTranslationMessages(Translation, Node);
        return Translation;
    }
    /// <summary>
    /// Creates a translation with a locale from the JSONH filename and messages parsed from the JSONH file.
    /// </summary>
    public static _Godot.Translation CreateTranslationFromFile(string JsonhFilePath) {
        string Locale = _SystemIO.Path.GetFileNameWithoutExtension(JsonhFilePath);
        string Jsonh = _Godot.FileAccess.GetFileAsString(JsonhFilePath);
        return CreateTranslation(Locale, Jsonh);
    }

    private static void AddTranslationMessages(_Godot.Translation Translation, _JsonNodes.JsonNode? Node, scoped _System.ReadOnlySpan<char> Prefix = "") {
        switch (Node) {
            case null:
                return;
            case _JsonNodes.JsonObject Object:
                foreach (_SystemCollectionsGeneric.KeyValuePair<string, _JsonNodes.JsonNode?> Property in Object) {
                    switch (Property.Value?.GetValueKind()) {
                        case _Json.JsonValueKind.String:
                            Translation.AddMessage(new _Godot.StringName($"{Prefix}{Property.Key}"), new _Godot.StringName((string)Property.Value!));
                            break;
                        case _Json.JsonValueKind.Object:
                        case _Json.JsonValueKind.Array:
                            AddTranslationMessages(Translation, Property.Value, Prefix: $"{Prefix}{Property.Key}.");
                            break;
                        default:
                            throw new _System.NotImplementedException(Property.Value?.ToJsonString() ?? "null");
                    }
                }
                break;
            case _JsonNodes.JsonArray Array:
                foreach ((int Index, _JsonNodes.JsonNode? Item) in _SystemLinq.Enumerable.Index(Array)) {
                    switch (Item?.GetValueKind()) {
                        case _Json.JsonValueKind.String:
                            Translation.AddMessage(new _Godot.StringName($"{Prefix}{Index}"), new _Godot.StringName((string)Item!));
                            break;
                        case _Json.JsonValueKind.Object:
                        case _Json.JsonValueKind.Array:
                            AddTranslationMessages(Translation, Item, Prefix: $"{Prefix}{Index}.");
                            break;
                        default:
                            throw new _System.NotImplementedException(Item?.ToJsonString() ?? "null");
                    }
                }
                break;
            default:
                throw new _System.NotImplementedException(Node.GetType().Name);
        }
    }
}