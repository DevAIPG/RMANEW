using Amphenol.RMA.AccesoDatos.Data;
using Amphenol.RMA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Xunit;

public class ApprovalFixtureFactoryTests
{
    [Theory]
    [InlineData(false, 1000)]
    [InlineData(true, 25000)]
    public void ApprovedTemplateProducesIndependentPendingFixture(bool directorStage, double expectedTotal)
    {
        using var m10 = new DbContextM10(new DbContextOptionsBuilder<DbContextM10>()
            .UseSqlServer("Server=unused;Database=unused;Integrated Security=true").Options);
        var approvedOn = DateTime.Now.AddDays(-1);
        var source = new CSEXSW_Rma
        {
            Id = 42, Rmarequest = "12345678", Status = "Approved", Sumbit = "Submitted",
            turno = "601761", date_approved = approvedOn, Crearcar = true, Car = "existing-car",
            Customer = "CUS001", Ship_To = "001", res_id_approver = 10, Totalrmavalues = 30000
        };
        var sourceLine = new csexsw_coustumer
        {
            Id = 99, RmaId = 42, CSEXSW_Rma = source, rma_seq_no = 5, Car = true,
            Coustumer = "ITEM001", Invoice = "12345", Loc = "001", Seq = 2,
            Qty = 3, Unit = 10000, Cost = 200, Action = "C", Retur = "001"
        };

        var fixture = ApprovalFixtureFactory.Build(m10, source, new[] { sourceLine }, 20, "Test approver",
            "test scenario", "T1234567", directorStage);

        Assert.Equal(0, fixture.Request.Id);
        Assert.Equal("T1234567", fixture.Request.Rmarequest);
        Assert.Equal("Pending", fixture.Request.Status);
        Assert.Equal("Submitted", fixture.Request.Sumbit);
        Assert.Equal("", fixture.Request.turno);
        Assert.Null(fixture.Request.date_approved);
        Assert.False(fixture.Request.Crearcar);
        Assert.Equal("", fixture.Request.Car);
        Assert.Equal(20, fixture.Request.res_id_approver);
        Assert.Equal(expectedTotal, fixture.Request.Totalrmavalues);
        Assert.Contains("template Id=42", fixture.Request.Comment);
        Assert.Equal(source.Customer, fixture.Request.Customer);
        Assert.Equal(source.Ship_To, fixture.Request.Ship_To);
        Assert.Equal(2, fixture.Lines.Count);
        Assert.NotSame(fixture.Lines[0], fixture.Lines[1]);
        Assert.Equal((decimal)expectedTotal, fixture.Lines.Sum(x => x.Qty * x.Unit));
        Assert.All(fixture.Lines, line =>
        {
            Assert.Equal(0, line.Id);
            Assert.Equal(0, line.RmaId);
            Assert.Equal(0, line.rma_seq_no);
            Assert.Null(line.CSEXSW_Rma);
            Assert.False(line.Car);
            Assert.Equal(sourceLine.Coustumer, line.Coustumer);
            Assert.Equal(sourceLine.Invoice, line.Invoice);
            Assert.Equal(sourceLine.Loc, line.Loc);
            Assert.Equal(sourceLine.Cost, line.Cost);
        });
        Assert.Equal("Approved", source.Status);
        Assert.Equal("601761", source.turno);
        Assert.Equal(approvedOn, source.date_approved);
        Assert.Equal(30000, source.Totalrmavalues);
        Assert.True(source.Crearcar);
        Assert.Equal(99, sourceLine.Id);
        Assert.Equal(42, sourceLine.RmaId);
        Assert.Equal(3m, sourceLine.Qty);
        Assert.Equal(10000m, sourceLine.Unit);
        Assert.Same(source, sourceLine.CSEXSW_Rma);
    }

    [Theory]
    [InlineData("M10Mesa01", "AIO-POS")]
    [InlineData("AIO-POS", "M10Mesa01")]
    public void FixtureCreationRejectsEitherProductionDestinationBeforeOpeningConnections(string m10Server, string erpServer)
    {
        var connections = new ForbidConnection();
        using var m10 = new DbContextM10(new DbContextOptionsBuilder<DbContextM10>()
            .UseSqlServer($"Server={m10Server};Database=M10;Integrated Security=true")
            .AddInterceptors(connections).Options);
        using var erp = new DbContext500(new DbContextOptionsBuilder<DbContext500>()
            .UseSqlServer($"Server={erpServer};Database=500;Integrated Security=true")
            .AddInterceptors(connections).Options);

        Assert.Throws<InvalidOperationException>(() => ApprovalFixtureFactory.Create(
            m10, erp, "RMA_TEST_CONCURRENT_ID", Array.Empty<int>()));
        Assert.Equal(0, connections.Attempts);
    }

    private sealed class ForbidConnection : DbConnectionInterceptor
    {
        public int Attempts { get; private set; }
        public override InterceptionResult ConnectionOpening(DbConnection connection,
            ConnectionEventData eventData, InterceptionResult result)
        {
            Attempts++;
            throw new Exception("This test must not access a database.");
        }
    }
}
