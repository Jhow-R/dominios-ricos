using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, nameof(Name.FirstName), "Nome deve conter pelo menos 3 caracteres")
                .HasMinLen(LastName, 3, nameof(Name.LastName), "Sobrenome deve conter pelo menos 3 caracteres")
                .HasMaxLen(FirstName, 40, nameof(Name.FirstName), "Nome deve conter até 40 caracteres")
            );
        }
    }
}