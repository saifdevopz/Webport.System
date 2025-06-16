using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tenant.Infrastructure.QuestPDF;

namespace Tenant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TestController() : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult Verify()
        {
            return Ok("You are authorized");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public ActionResult VerifyAdmin()
        {
            return Ok("You are authorized as Admin");
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public ActionResult VerifyUser()
        {
            return Ok("You are authorized as User");
        }

        [HttpGet("pdf")]
        public ActionResult GeneratePDF()
        {
            var document = CreateDocument();
            var pdf = document.GeneratePdf();
            return File(pdf, "application/pdf", "netcode-hub");
        }

        [HttpGet("pdf2")]
        public ActionResult GetInvoice()
        {
            var model = InvoiceDocumentDataSource.GetInvoiceDetails();
            var document = new InvoiceDocument(model);
            var pdf = document.GeneratePdf();
            return File(pdf, "application/pdf", "webport-pdf");
        }

        private static Document CreateDocument()
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header().Text("This is Page Header")
                    .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(1, Unit.Centimetre);
                        x.Item().Text(Placeholders.LoremIpsum());
                        x.Item().Image(Placeholders.Image(200, 100));
                        x.Item().Text(Placeholders.Question());
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            });
        }
    }
}
