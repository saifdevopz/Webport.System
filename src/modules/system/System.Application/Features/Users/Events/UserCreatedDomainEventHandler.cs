﻿using Common.Application.Mail;
using System.Domain.Entities.Users;

namespace System.Application.Features.Users.Events;


internal sealed class UserCreatedDomainEventHandler(IMailService mailService)
    : DomainEventDispatcher<UserCreatedDomainEvent>
{
    public override async Task Handle(
        UserCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        MailRequest mailRequest = new
        (
            to: ["saif43515@gmail.com"],
            subject: "Unhandled Exception Notification",
            body: $"An unhandled exception occurred during the handling of",
            from: "info@saifkhan.co.za",
            displayName: "Error Notification Service"
        );

        // Send the email notification
        await mailService.SendAsync(mailRequest, cancellationToken);
    }
}