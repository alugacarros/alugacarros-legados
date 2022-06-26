namespace AlugaCarros.Legados.Api.Models.Clients;
public class Client
{
    public Client(string name, string email, string phone, string document, ClientStatus status)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        Document = document;
        Status = status;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Document { get; private set; }
    public ClientStatus Status { get; private set; }
}
