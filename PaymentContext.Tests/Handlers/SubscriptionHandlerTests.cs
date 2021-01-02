using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // Red, Green, Refactor
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "99999999999";
            command.Email = "hello@balta.io2";    
            command.BarCode = "123456789";
            command.BoletoNumber =  "12345654987";     
            command.PaymentNumber = "123121";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Wayne Corp";
            command.PayerDocument = "12345678911";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "batman@dc.com";           
            command.Street = String.Empty;
            command.Number = String.Empty;
            command.Neighborhood = String.Empty;
            command.City = String.Empty;
            command.State = String.Empty;
            command.Country = String.Empty;
            command.ZipCode = String.Empty;

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}