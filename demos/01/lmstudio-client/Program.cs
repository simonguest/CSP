using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

class Program
{
    static async Task Main(string[] args)
    {
        var endpoint = "http://192.168.88.16:1234/v1";
        var apiKey = "lm-studio"; // LMStudio doesn't require a real API key

        var client = new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions
        {
            Endpoint = new Uri(endpoint)
        });

        // Get the chat client
        var chatClient = client.GetChatClient("google/gemma-3-27b");
        Console.WriteLine($"Connected to: {endpoint}");
        Console.WriteLine($"Model: google/gemma-3-27b");

        // Create a conversation with an NPC character
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(@"You are Eldrin, a wise and experienced forest ranger who has lived at the edge of the Enchanted Forest for decades.
You speak in a slightly mystical but friendly manner, using vivid descriptions. You know the forest paths well and warn travelers of its dangers while offering helpful guidance.
Keep your responses concise (2-4 sentences) as if speaking naturally in a video game dialogue.
You mention landmarks like the Whispering Willows, the Crystal Brook, the Ancient Oak, and the Moonlit Glade."),
            new UserChatMessage("Hello there, friend. I need to cross this enchanted forest. Can you help me?")
        };

        try
        {
            Console.WriteLine("Player: Hello there, friend. I need to cross this enchanted forest. Can you help me?");
            Console.WriteLine();
            Console.Write("Eldrin: ");

            // Make the chat completion request
            var response = await chatClient.CompleteChatAsync(messages);

            // Display the response
            Console.WriteLine(response.Value.Content[0].Text);

            // Add the assistant's response to the conversation
            messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));

            // Ask a follow-up question
            Console.WriteLine();
            Console.WriteLine("Player: That sounds dangerous. What should I watch out for along the way?");
            messages.Add(new UserChatMessage("That sounds dangerous. What should I watch out for along the way?"));

            Console.WriteLine();
            Console.Write("Eldrin: ");

            // Get the follow-up response
            response = await chatClient.CompleteChatAsync(messages);
            Console.WriteLine(response.Value.Content[0].Text);

            // Add the assistant's response to the conversation
            messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));

            // Ask a final question
            Console.WriteLine();
            Console.WriteLine("Player: Thank you, Eldrin. Do you have any final advice before I set off?");
            messages.Add(new UserChatMessage("Thank you, Eldrin. Do you have any final advice before I set off?"));

            Console.WriteLine();
            Console.Write("Eldrin: ");

            // Get the final response
            response = await chatClient.CompleteChatAsync(messages);
            Console.WriteLine(response.Value.Content[0].Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
            Console.WriteLine("\nMake sure your LMStudio server is running at {endpoint}");
            Console.WriteLine("and that the model 'google/gemma-3-27b' is loaded.");
        }
    }
}
