# Get ja List päringute kontroll

Kontrollitud 16.10.2026 kohaliku SQL LocalDB andmebaasiga.

Kõigil seitsmel DbSetil on oma feature-kaust, Get ja List query koos handler'iga ning ApiControllerBase'ist päriv kontroller. Kontrollerid küsivad andmeid IMediator.Send kaudu.

| Kontroller | List | Get olemasoleva ID-ga | Get olematu ID-ga | Get ID 0 | Get ID -1 | Get ilma ID-ta |
| --- | --- | --- | --- | --- | --- | --- |
| Users | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| Cars | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| Bookings | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| Invoices | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| InvoiceLines | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| CarModels | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |
| CarManufacturers | 200, 35 kirjet | 200 | 404 | 404 | 404 | 404 |

Olemasoleva kirje ID oli 1; olematu kirje kontrolliks kasutati ID-d 2147483647. Get vastuse ID võrreldi küsitud ID-ga. Kokku läbis 42 HTTP-kontrolli.

## Visual Studio breakpoint'i kontroll

Visual Studio debugger ühendati töötava KooliProjekt.WebAPI protsessiga (Debug > Attach to Process).

1. CarsController.cs real 31 seati breakpoint `result == null` haru `return NotFound()` reale.
2. Kutsuti `GET /api/Cars/get/2147483647`.
3. Debugger peatus real 31; Autos näitas `result = null`, ID oli 2147483647. F5 järel tagastas API 404.
4. CarsController.cs real 23 seati breakpoint ID kontrolli `return NotFound()` reale.
5. Kutsuti `GET /api/Cars/get/0`.
6. Debugger peatus real 23; ID oli 0. F5 järel tagastas API 404.

## Kontrolli kordamine

Käivita Visual Studios WebAPI projekt `http` profiiliga. Ava `KooliProjekt.WebAPI/KooliProjekt.WebAPI.http`, kus on kõigi seitsme kontrolleri kuus näidispäringut. Vaikimisi aadress on `http://localhost:5086`; muuda faili alguses aadressi, kui kasutad muud porti.

Lahendus kompileerus käsuga `dotnet build KooliProjekt.sln --no-restore`: 0 viga. Olemasolev hoiatus NU1504 puudutab WebAPI projektifaili dubleeritud Microsoft.EntityFrameworkCore.Design PackageReference'i.
