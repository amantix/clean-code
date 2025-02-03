namespace API.Requests;

public class DocumentUpdateRequest : CustomRequest
{
    public required string Text { get; set; }
}