# DeepL Translate Tool

DeepL Translate Tool is a C# application that provides seamless translation capabilities using the DeepL API integration and parallel processing. It allows you to translate text from input files to desired output formats with ease.

## Features

- Translate text using the powerful DeepL API
- Utilize parallel processing and query caching for efficient translation
- Supports various input and output formats which can be easily extended

## Adapters

This tool is using Adapters to parse queries from input files and then save results in desired format. Feel free to create your own Adapter and Pull Request to contribute to the project!

### Currently supported Adapters
- plaintext
- Yii2

## Getting Started

1. Clone the repository.
2. Install .NET Core SDK (if not already installed).
3. Run the application using `dotnet run`.

## Usage

```bash
# Display help message
dotnet run -- --help
```
