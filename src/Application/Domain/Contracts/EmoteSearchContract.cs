// ReSharper disable ClassNeverInstantiated.Global
namespace Application.Domain.Contracts;

public record EmoteSearchContract(EmoteSearchContractData Data)
{
    public bool HasResults => Data.Emotes is not null; 
}

public record EmoteSearchContractData(EmoteSearchContractEmotes? Emotes);

public record EmoteSearchContractEmotes(List<EmoteSearchContractItem> Items);

public record EmoteSearchContractItem(string Id, string Name);