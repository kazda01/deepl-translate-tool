# DeepL Translate Tool

DeepL Translate Tool is a C# application that provides seamless translation capabilities using the DeepL API integration and parallel processing. It allows you to translate text from input files to desired output formats with ease.

What exactly is meant by that? This tool was designed to be used in Github pipelines to automatically translate ANY project files to any language. And since each project or framework stores its translation files in its own format and file structure, this tool uses Adapters to parse queries from input files and then save results in predefined format. More about available Adapters and their extensibility can be found [below](#Adapters).

## Features

- Translate text using the powerful DeepL API
- Utilize parallel processing and query caching for efficient translation
- Supports various input and output formats which can be easily extended

## Getting Started

Currently you can use this tool by downloading executable or building it at your own.

### Downloading executable (recommended)

1. Download the latest release for your OS from [Releases](https://github.com/kazda01/deepl-translate-tool/releases/latest)
2. Run the executable like explained in [Usage](#Usage)

### Building from source

1. Clone this repository
2. Install .NET Core SDK (if not already installed).
3. CD into the project directory - `cd DeepLTranslateTool`
4. Run the application using `dotnet run` or build it using `dotnet build` and run the executable like explained in [Usage](#Usage)


## Usage

DeepL API requires an API key to be used for every request. They (currently) also offer a Free plan, which will get you 500'000 characters/month which is enough on its own plus this tool uses query caching for 1 month. You can get your API key from [DeepL API](https://www.deepl.com/pro-api).

```bash
# Display all target languages
./DeepLTranslateTool list-languages --api-key YOUR_DEEPL_API_KEY

# Display all source languages
./DeepLTranslateTool list-source-languages --api-key YOUR_DEEPL_API_KEY

# Create plaintext file with text to translate
touch hello.txt
echo "Hello world, this looks like a great tool!" >> hello.txt
echo "I am excited to see how it works." >> hello.txt

# Translate plaintext file in working directory from English to Czech, French and Slovak
./DeepLTranslateTool translate --api-key YOUR_DEEPL_API_KEY --source-language en --languages cs fr sk --input-file hello.txt

# Check generated files
cat cs.txt fr.txt sk.txt
```

### Advanced usage

This is example of advanced usage - translating a PHP Yii2 project to French and German.
Executable is located in project's root directory, but translations are stored in `messages` directory.
Yii2 also stores translations in PHP associative arrays, so the translation file `/messages/fr/app.php` looks like this:

```php
return [
    'Hello world' => 'Bonjour le monde',
    'This looks like a great tool!' => 'Cela ressemble à un excellent outil!',
    'I am excited to see how it works.' => 'Je suis excité de voir comment cela fonctionne.',
];
```

After translation, the tool will create corresponding folders and files in the `messages` directory.

```bash
./DeepLTranslateTool translate --api-key YOUR_DEEPL_API_KEY --source-language en --languages fr de --input-file cs/app.php --adapter php-yii2 --path messages
```

### Help command

There is a help command that can be used to display all available verbs and parameters and their descriptions and default values.

```bash
# There is a help command available for all verbs and parameters
./DeepLTranslateTool --help
./DeepLTranslateTool translate --help
./DeepLTranslateTool list-languages --help
./DeepLTranslateTool list-source-languages --help
```

## Configuration

Configuration is done using command line arguments. You can display all available arguments and their descriptions using `--help` command.

#### Required Parameter
- `--api-key`
  - DeepL API authentication key. You must provide a valid DeepL API key to use the translation service.

#### Optional Parameters
- `-a, --adapter`
  - **Default**: `plaintext`
  - Adapter to use for input and output files. Specify the adapter to handle different file formats.

- `-s, --source-language`
  - **Default**: `en`
  - Source language for translation. Specify the language code of the source text.

- `-l, --languages`
  - **Default**: `cs fr`
  - Languages to translate to, separated by space. Provide one or more target language codes.

- `-i, --input-file`
  - Input file to translate. Specify the file containing the text to be translated. If not specified, the input file is defined by the chosen adapter.

- `--path`
  - **Default**: Current directory
  - Working directory path. Define the base path for input and output files.

- `--no-cache`
  - **Default**: `false`
  - Whether to disable caching of translation results. Set to `true` to disable caching.

- `-v, --verbose`
  - **Default**: `false`
  - Print verbose output. Enable verbose mode for detailed logging.

- `--help`
  - Display help screen. Show the help information.

- `--version`
  - Display version information. Show the current version of the tool.

## Adapters

This tool is using Adapters to parse queries from input files and then save results in desired format. Feel free to create your own Adapter and Pull Request to contribute to the project!

### Currently supported Adapters
- plaintext
- php-yii2

## Github Actions

As mentioned before, this tool was designed to be used in Github pipelines. Currently, there is custom Github Action being developed, which will be available soon. Until then, you can directly use this tool by copying its executable from [Releases](https://github.com/kazda01/deepl-translate-tool/releases/latest) and running it in your pipeline by CLI commands.