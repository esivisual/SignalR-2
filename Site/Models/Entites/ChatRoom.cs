namespace Site.Models.Entites
{
    public class ChatRoom
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
        
        public ICollection<ChatMessage> ChatMessage { get; set; }

    }
}
