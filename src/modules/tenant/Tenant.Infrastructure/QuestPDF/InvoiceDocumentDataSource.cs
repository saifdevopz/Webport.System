using QuestPDF.Helpers;

namespace Tenant.Infrastructure.QuestPDF;

public static class InvoiceDocumentDataSource
{
    private static readonly Random Random = new();

    public static InvoiceModel GetInvoiceDetails()
    {
        var items = Enumerable
            .Range(1, 25)
            .Select(_ => GenerateRandomOrderItem())
            .ToList();

#pragma warning disable CA5394 // Do not use insecure randomness
        return new InvoiceModel
        {
            InvoiceNumber = Random.Next(1_000, 10_000),
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now + TimeSpan.FromDays(14),

            SellerAddress = GenerateRandomAddress(),
            CustomerAddress = GenerateRandomAddress(),

            Items = items,
            Comments = Placeholders.Paragraph()
        };
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    private static OrderItem GenerateRandomOrderItem()
    {
#pragma warning disable CA5394 // Do not use insecure randomness
        return new OrderItem
        {
            Name = Placeholders.Label(),
            Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
            Quantity = Random.Next(1, 10)
        };
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    private static Address GenerateRandomAddress()
    {
        return new Address
        {
            CompanyName = Placeholders.Name(),
            Street = Placeholders.Label(),
            City = Placeholders.Label(),
            State = Placeholders.Label(),
            Email = Placeholders.Email(),
            Phone = Placeholders.PhoneNumber()
        };
    }
}