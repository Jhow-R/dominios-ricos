using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;
using System.Diagnostics;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Student _student;
        private readonly Subscription _subscription;
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _address;

        public StudentTests()
        {
            _name = new Name("Bruce", "Wayne");
            _document = new Document("12345678912", EDocumentType.CPF);
            _email = new Email("batman@dc.com");
            _address = new Address("Rua 1", "1234", "Bairro Legal", "Gotham", "SP", "BR", "1340000");

            _student = new Student(
                name: _name,
                document: _document,
                email: _email
            );

            _subscription = new Subscription(null);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            foreach (var notifications in _student.Name.Notifications)
                Debug.WriteLine(notifications.Message);

            var payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Wayne Corp", _document, _address, _email);
            _subscription.AddPayment(payment);

            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenAddSubscription()
        {
            var payment = new PayPalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Wayne Corp", _document, _address, _email);
            _subscription.AddPayment(payment);
           
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Valid);
        }
    }
}
