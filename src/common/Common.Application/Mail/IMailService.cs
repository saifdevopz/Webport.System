﻿namespace Common.Application.Mail;
public interface IMailService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}