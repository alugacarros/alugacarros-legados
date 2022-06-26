namespace AlugaCarros.Legados.Api.Models
{
    public class User
    {
        public User(string name, Guid idIdentity)
        {
            Id = Guid.NewGuid();
            Name = name;
            IdIdentity = idIdentity;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid IdIdentity { get; private set; }
    }
}
