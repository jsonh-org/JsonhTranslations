using _System = global::System;
using _CollectionsGeneric = global::System.Collections.Generic;
using _Godot = global::Godot;

namespace JsonhTranslations;

public static class TranslateExtensions {
    /// <summary>
    /// Translate: Finds a translation for the message, and formats the given arguments.
    /// </summary>
    public static string Tr(this string Message, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        return string.Format(_Godot.TranslationServer.Translate(Message), FormatArgs);
    }
    /// <summary>
    /// Translate plural: Finds a translation for the message, first trying with the suffix appended, and formats the given arguments.
    /// </summary>
    public static string TrN(this string Message, string Suffix, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        string MessageWithSuffix = $"{Message}{Suffix}";
        string TranslatedMessageWithSuffix = _Godot.TranslationServer.Translate(MessageWithSuffix);
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
    public static _CollectionsGeneric.List<string> TrA(this string Message, string Suffix, params scoped _System.ReadOnlySpan<object?> FormatArgs) {
        _CollectionsGeneric.List<string> TranslatedMessages = [];
        for (int Index = 0; ; Index++) {
            string MessageWithSuffixAndIndex = $"{Message}{string.Format(Suffix, Index.ToString())}";
            string TranslatedMessageWithSuffixAndIndex = _Godot.TranslationServer.Translate(MessageWithSuffixAndIndex);
            if (TranslatedMessageWithSuffixAndIndex == MessageWithSuffixAndIndex) {
                break;
            }
            TranslatedMessages.Add(string.Format(TranslatedMessageWithSuffixAndIndex, FormatArgs));
        }
        return TranslatedMessages;
    }
}