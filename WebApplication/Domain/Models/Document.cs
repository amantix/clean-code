using Domain.Enums;

namespace Domain.Models;

public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public AccessType AccessType { get; set; }
    public Guid UserId{ get; set; }
}