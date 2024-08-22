public class SwiftMT799Message
{
    public required string Sender { get; set; }
    public required string Receiver { get; set; }
    public required string TransactionReference { get; set; }
    public required string RelatedReference { get; set; }
    public required string NarrativeText { get; set; }
}

public class SwiftMT799Parser
{
    public SwiftMT799Message Parse(string messageContent)
    {
        var message = new SwiftMT799Message
        {
            Sender = string.Empty,
            Receiver = string.Empty,
            TransactionReference = string.Empty,
            RelatedReference = string.Empty,
            NarrativeText = string.Empty
        };
        var lines = messageContent.Split('\n');

        foreach (var line in lines)
        {
            if (line.StartsWith(":20:"))
                message.TransactionReference = line.Substring(4);
            else if (line.StartsWith(":21:"))
                message.RelatedReference = line.Substring(4);
            else if (line.StartsWith(":79:"))
                message.NarrativeText = line.Substring(4);
            // Add more field parsing as needed
        }

        return message;
    }
}
