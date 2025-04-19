namespace Domain.Entities;
public class ChatMessage
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
