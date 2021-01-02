using System;
using System.Diagnostics.Contracts;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>,
        IHandler<CreateCreditCardSubscriptionCommand>
    {

        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail Fast Validations
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar assinatura");
            }

            // Verificar se documento já está cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification(nameof(command.Document), "Este CPF já está em uso");

            // Verificar se e-maul já está cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification(nameof(command.Email), "Este e-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(
                command.Street,
                command.Number,
                command.Neighborhood,
                command.City,
                command.State,
                command.Country,
                command.ZipCode);

            // Gerar as Entidades
            var student = new Student(
                name: name,
                document: document,
                email: email
            );

            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar as Vaidações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar assinatura");


            // Salvar as Informações
            _repository.CreateSubscription(student);

            // Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo!", "Sua assinatura foi criada com sucesso");

            // Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            throw new NotImplementedException();
        }
    }
}