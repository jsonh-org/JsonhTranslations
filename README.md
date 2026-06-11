# JSONH Translations for Godot 4x

[![Godot Asset Store](https://img.shields.io/github/v/release/jsonh-org/JsonhTranslations.svg?label=Godot%20Asset%20Store&logo=godotengine)](https://store.godotengine.org/asset/joyless/jsonh-translations)

A Godot plugin for importing translation files in [JSONH V2](https://github.com/jsonh-org/Jsonh) format using [JsonhCs](https://github.com/jsonh-org/JsonhCs).

This plugin requires Godot with C# .NET support.

## Setup

1. Install the [JsonhCs](https://www.nuget.org/packages/JsonhCs) NuGet package.
1. [Install the addon and enable the plugin.](https://docs.godotengine.org/en/stable/tutorials/plugins/editor/installing_plugins.html)
1. Build the .NET project.
1. [Find your desired locale code](https://docs.godotengine.org/en/stable/tutorials/i18n/locales.html).
1. Create a text file called `{locale_code}.jsonh`. For example, English is `en.jsonh`.
1. Import the text file as a "JSONH Translation".
1. Navigate to `Project Settings > Localization > Translations` and add the text file. Change "All recognized" to "All files" and select the text file.
1. Add the messages using the syntax below.

## Syntax

Add messages as properties of a root object:

```jsonh
HELLO: "Hello!"
BYE: "Goodbye!"
```

Add hierarchical messages with objects:

```jsonh
FOOD.CAKE: "Cake"
FOOD.BREAD: "Bread"
```
```jsonh
FOOD: {
    CAKE: "Cake"
    BREAD: "Bread"
}
```

Add indexed messages with arrays:

```jsonh
GREETINGS.0: "Hello"
GREETINGS.1: "Hi"
GREETINGS.2: "Hey"
```
```jsonh
GREETINGS: [
    "Hello"
    "Hi"
    "Hey"
]
```

## Methods

Translate a message:

```jsonh
GREET: "Hello"
```
```cs
using JsonhTranslations;

string Message = "GREET".Tr();
```

Translate and format a message:

```jsonh
COUNT_GOLD: "You have {0} gold."
```
```cs
using JsonhTranslations;

string Message = "COUNT_GOLD".Tr(500);
```

Translate a plural message:

```jsonh
YOU_HAVE_SWORDS: "You have {0} swords."
YOU_HAVE_SWORDS*1: "You have {0} sword."
```
```cs
using JsonhTranslations;

string Message = "YOU_HAVE_SWORDS".TrN("*" + 2);
```

Translate an array of messages:

```jsonh
GREET_OPTIONS: [
    "Hello"
    "Hi"
    "Hey"
]
```
```cs
using JsonhTranslations;

List<string> Messages = "GREET_OPTIONS".TrA(".{0}");
```

Add a translation dynamically:

```cs
using JsonhTranslations;

Translation EnglishTranslation = JsonhTranslationsExtensions.CreateTranslationFromFile("en.jsonh");
TranslationServer.AddTranslation(EnglishTranslation);
```

## Import Options

### Output Source Text

Whether to include the translation file's source code as meta in the translation resource.

This is useful for providing one of your translation files in your game as a template for contributors to provide their own translations.

The source code be accessed with the `source_text` meta property:
```cs
string SourceText = (string)GD.Load<Translation>("res://en.jsonh").GetMeta("source_text", "");
```
