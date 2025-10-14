# LMStudio C# Client

A simple C# console application that connects to a local LMStudio server running the Gemma 3 27B model via the OpenAI-compatible chat completions endpoint.

## Prerequisites

- .NET SDK (8.0 or later)
- LMStudio running locally with Gemma 3 27B model loaded
- LMStudio server listening on `http://192.168.88.16:1234`

## Project Structure

```
lmstudio-client/
├── LMStudioClient.csproj    # Project file with OpenAI SDK dependency
└── Program.cs                # Main application code
```

## Dependencies

- `OpenAI` (v2.5.0) - Official OpenAI .NET SDK

## How It Works

The client:
1. Creates an OpenAI client configured to point to your LMStudio server endpoint
2. Initiates a conversation with a system message and user query
3. Sends three messages to demonstrate multi-turn conversation:
4. Displays the responses and token usage information

## Configuration

You can modify the following parameters in `Program.cs`:

```csharp
var endpoint = "http://192.168.88.16:1234/v1";  // LMStudio server URL
var apiKey = "lm-studio";                        // Dummy API key (not validated by LMStudio)
var chatClient = client.GetChatClient("google/gemma-3-27b");  // Model name
```

## Building and Running

### Build the project:
```bash
cd LMStudioClient
dotnet build
```

### Run the project:
```bash
dotnet run
```