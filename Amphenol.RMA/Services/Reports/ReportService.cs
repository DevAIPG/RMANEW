using Amphenol.RMA.Models;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Amphenol.RMA.Services.Reports
{
    public class ReportService
    {
        private readonly IConfiguration _configuration;
        private const string PdfContentType = "application/pdf";

        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
        }
        public async Task<ReportDownloadDto> GenerateReturnMaterialsAuthorizationPDF(string rma)
        {
            if (string.IsNullOrWhiteSpace(rma))
            {
                throw new ArgumentException(
                    "An RMA number is required.",
                    nameof(rma));
            }

            string reportPath = Path.Combine(AppContext.BaseDirectory, "Reports", "ReturnMaterialsAuthorization.frx");

            if (!File.Exists(reportPath))
            {
                throw new FileNotFoundException(
                    "The RMA report template was not found.",
                    reportPath);
            }

            string connectionString =
                _configuration.GetConnectionString("Connection100")
                ?? throw new InvalidOperationException("Connection string 'Connection100' was not found.");

            byte[] reportTemplate =
                await File.ReadAllBytesAsync(reportPath);

            using var report = new Report();
            using var reportStream = new MemoryStream(reportTemplate);

            report.Load(reportStream);

            DataConnectionBase connection =
                report.Dictionary.Connections
                    .Cast<DataConnectionBase>()
                    .FirstOrDefault(item => item.Name == "MacolaConnection")
                ?? throw new InvalidOperationException("The report connection 'MacolaConnection' was not found.");

            connection.ConnectionString = connectionString;

            if (!int.TryParse(rma, out int rmaNumber))
            {
                throw new ArgumentException($"'{rma}' is not a valid numeric RMA number.", nameof(rma));
            }

            report.SetParameterValue("@rma_no", rmaNumber);

            bool prepared = report.Prepare();

            if (!prepared || report.PreparedPages.Count == 0)
            {
                throw new InvalidOperationException($"The report did not produce any pages for RMA {rma}.");
            }

            using var output = new MemoryStream();

            using var pdfExport = new PDFSimpleExport
            {
                ShowProgress = false
            };

            report.Export(pdfExport, output);

            return new ReportDownloadDto
            {
                ContentType = PdfContentType,
                Data = output.ToArray(),
                FileName = $"RMA_{rma}.pdf"
            };
        }
    }
}
