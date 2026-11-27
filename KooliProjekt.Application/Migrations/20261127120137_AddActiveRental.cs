using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Repair only the recognizable original demo data. Never guess when a user
            // has more than one candidate invoice or booking for the relevant dates.
            migrationBuilder.Sql(@"
UPDATE l SET InvoiceId = i.Id
FROM InvoiceLines l
JOIN Bookings b ON b.Id = l.BookingId
JOIN Users u ON u.Id = b.UserId
JOIN Invoices i ON i.UserId = u.Id AND i.InvoiceDate = DATEADD(day, 1, CAST(b.Started AS date))
WHERE l.InvoiceId IS NULL AND l.LineItem = N'Autorendi läbisõit'
  AND l.Unit = N'km' AND l.Quantity = 1
  AND u.Email LIKE 'klient[0-9][0-9]@example.com'
  AND b.Started >= '20260101' AND b.Started < '20260205'
  AND b.Finished = DATEADD(hour, 4, b.Started)
  AND (SELECT COUNT(*) FROM Invoices ix WHERE ix.UserId = u.Id AND ix.InvoiceDate = i.InvoiceDate) = 1
  AND (SELECT COUNT(*) FROM Bookings bx WHERE bx.UserId = u.Id AND bx.Started = b.Started) = 1
  AND (SELECT COUNT(*) FROM InvoiceLines lx WHERE lx.BookingId = b.Id) = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bookings");
        }
    }
}
