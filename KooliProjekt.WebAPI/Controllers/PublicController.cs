using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Public;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace KooliProjekt.WebAPI.Controllers;
public class PublicController : ApiControllerBase {
 readonly IMediator mediator;
 public PublicController(IMediator mediator) { this.mediator=mediator; }
 [AllowAnonymous, HttpPost("register")] public async Task<IActionResult> Register(RegisterCommand command,CancellationToken ct) => Result(await mediator.Send(command,ct));
 [HttpGet("profile")] public async Task<IActionResult> Profile(CancellationToken ct) => Result(await mediator.Send(new MyProfileQuery(),ct));
 [HttpPut("profile")] public async Task<IActionResult> UpdateProfile(UpdateMyProfileCommand command,CancellationToken ct) => Result(await mediator.Send(command,ct));
 [HttpGet("available-cars")] public async Task<IActionResult> AvailableCars([FromQuery] DateTime from,[FromQuery] DateTime until,CancellationToken ct) => Result(await mediator.Send(new AvailableCarsQuery(from,until),ct));
 [HttpGet("rentals")] public async Task<IActionResult> Rentals(CancellationToken ct) => Result(await mediator.Send(new MyRentalsQuery(),ct));
 [HttpGet("rentals/{id:int}")] public async Task<IActionResult> Rental(int id,CancellationToken ct) => Result(await mediator.Send(new MyRentalQuery(id),ct));
 [HttpPost("rentals")] public async Task<IActionResult> RentCar(RentCarCommand command,CancellationToken ct) => Result(await mediator.Send(command,ct));
 [HttpPost("rentals/{id:int}/finish")] public async Task<IActionResult> FinishRental(int id,FinishRentalInput input,CancellationToken ct) => Result(await mediator.Send(new FinishMyRentalCommand(id,input.FinishKm),ct));
 [HttpGet("invoices")] public async Task<IActionResult> Invoices(CancellationToken ct) => Result(await mediator.Send(new MyInvoicesQuery(),ct));
 [HttpGet("invoices/{id:int}")] public async Task<IActionResult> Invoice(int id,CancellationToken ct) => Result(await mediator.Send(new MyInvoiceQuery(id),ct));
}
public record FinishRentalInput(decimal FinishKm);
