using OpenAI.Interfaces;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace Cosmos.Chat.GPT.Services;

/// <summary>
/// Service to access OpenAI via Betalgo.OpenAI.
/// </summary>
public class OpenAiService
{
    private readonly IOpenAIService openAIService;

    /// <summary>
    /// System prompt to send with user prompts to instruct the model for chat session
    /// </summary>
    private readonly string _systemPrompt = @"
        You are an AI assistant that helps people find information.
        Provide concise answers that are polite and professional." + Environment.NewLine;
    
    /// <summary>    
    /// System prompt to send with user prompts to instruct the model for summarization
    /// </summary>
    private readonly string _summarizePrompt = @"
        Summarize this prompt in one or two words to use as a label in a button on a web page" + Environment.NewLine;

    /// <summary>
    /// Creates a new instance of the service.
    /// </summary>
    /// <param name="endpoint">Endpoint URI.</param>
    /// <param name="key">Account key.</param>
    /// <param name="modelName">Name of the deployed Azure OpenAI model.</param>
    /// <exception cref="ArgumentNullException">Thrown when endpoint, key, or modelName is either null or empty.</exception>
    /// <remarks>
    /// This constructor will validate credentials and create a HTTP client instance.
    /// </remarks>
    public OpenAiService(string endpoint, string key, string modelName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);

        this.openAIService = new OpenAIService(new OpenAI.OpenAiOptions()
        {
            ApiKey = key
        });
    }

    /// <summary>
    /// Sends a prompt to the deployed OpenAI LLM model and returns the response.
    /// </summary>
    /// <param name="sessionId">Chat session identifier for the current conversation.</param>
    /// <param name="prompt">Prompt message to send to the deployment.</param>
    /// <returns>Response from the OpenAI model along with tokens for the prompt and response.</returns>
    public async Task<(string response, int promptTokens, int responseTokens)> GetChatCompletionAsync(string sessionId, string userPrompt)
    {
        
        ChatMessage systemMessage = new(StaticValues.ChatMessageRoles.System, _systemPrompt);
        ChatMessage userMessage = new(StaticValues.ChatMessageRoles.User, userPrompt);

        ChatCompletionCreateResponse completionsResponse = await openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
        {
            Messages = new List<ChatMessage>() { systemMessage, userMessage },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
            MaxTokens = 2000,//total limit:4096, conversation tokens(defined at appsettings):2000 -> tokens remain:2000
            Temperature = 0.3f,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0
        }); 

        if (!completionsResponse.Successful)
        {
            if (completionsResponse.Error == null)
            {
                throw new Exception("Unknown Error");
            }
            return (
                response: completionsResponse.Error.Code+":"+completionsResponse.Error.Message,
                promptTokens: 0,
                responseTokens: 0
            );
        }
        return (
            response: completionsResponse.Choices[0].Message.Content,
            promptTokens: completionsResponse.Usage.PromptTokens,
            responseTokens: completionsResponse.Usage.CompletionTokens??0
        );
    }

    /// <summary>
    /// Sends the existing conversation to the OpenAI model and returns a two word summary.
    /// </summary>
    /// <param name="sessionId">Chat session identifier for the current conversation.</param>
    /// <param name="conversation">Prompt conversation to send to the deployment.</param>
    /// <returns>Summarization response from the OpenAI model deployment.</returns>
    public async Task<string> SummarizeAsync(string sessionId, string userPrompt)
    {
        
        ChatMessage systemMessage = new(StaticValues.ChatMessageRoles.System, _summarizePrompt);
        ChatMessage userMessage = new(StaticValues.ChatMessageRoles.User, userPrompt);
        
        ChatCompletionCreateResponse completionsResponse = await openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
        {
            Messages = new List<ChatMessage>() { systemMessage, userMessage },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
            MaxTokens = 200,
            Temperature = 0.0f,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0
        });

        string summary = completionsResponse.Choices[0].Message.Content;

        return summary;
    }
}