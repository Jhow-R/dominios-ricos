using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var student = new Student(
                name: new Name("André", "Baltieri"),
                document: new Document("12345678912", EDocumentType.CPF),
                email: new Email("hello@balta.io")
            );
            student.AddSubscription(subscription);
        }
    }
}
