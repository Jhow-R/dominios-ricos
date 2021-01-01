using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void AdicionarAssinatura()
        {
            var subscription = new Subscription(null);
            var name = new Name("André", "Baltieri");

            foreach (var notifications in name.Notifications)
            {
                Console.WriteLine(notifications.Message);
            }

            var student = new Student(
                name: name,
                document: new Document("12345678912", EDocumentType.CPF),
                email: new Email("hello@balta.io")
            );
            student.AddSubscription(subscription);
        }
    }
}
