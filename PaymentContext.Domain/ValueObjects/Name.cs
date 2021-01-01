using System;
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

            if (String.IsNullOrEmpty(FirstName))
                AddNotification(nameof(FirstName), "Nome Inválido");
        }
    }
}