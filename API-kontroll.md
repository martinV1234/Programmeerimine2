# Get ja List päringute kontroll

## DTO-d, OperationResult ja leheküljed (23.10.2026)

Kõik seitse Get query't/handler'it tagastavad `OperationResult<XxxDto>` ning
List query'd/handler'id `OperationResult<PagedResult<XxxListItemDto>>`.
Kontroller kasutab olemasolevat `Result(result)` abimeetodit: edukas vastus
on JSON kujul `{ "value": ... }`, vead annavad 400 ning puuduv detail 404.
List-vastuse kirjed asuvad `value.results` all.

`Data/PagingExtensions.cs` on võetud õpetaja failist
`20261023-KooliProjekt-OperationResult-Dto.zip`. Lisatud on katkestustoken,
negatiivse lehesuuruse käsitlemine, ülempiir 100 ja kaitse leheküljenumbri
korrutamisel tekkiva ületäitumise vastu. Vaikimisi on leht 1 ja suurus 10;
leht <= 0 muutub 1-ks ning lehesuurus <= 0 muutub 10-ks.
Päringud järjestatakse ID järgi enne Skip/Take rakendamist.

Toetatud on mõlemad URL-id:

```http
GET /api/Bookings/list?page=2&pageSize=10
GET /api/Bookings/list/2/10
```

Samad marsruudid töötavad kõigi seitsme kontrolleriga.
Vana `/list` URL töötab samuti, kuid tagastab nüüd pakitud esimese lehe.
Swagger kirjeldab vastavaid DTO- ja OperationResult-skeeme.

### DTO-de väljade valik autorendi kasutajale

| Andmed | Loend | Detail lisaks |
| --- | --- | --- |
| Users | ID, nimi | E-post, telefon; parool ja IsAdmin ei kuulu vastustesse |
| Cars | ID, registreerimisnumber, tootja, mudel | Mudeli ID, km- ja tunnitariif |
| CarModels | ID, mudelinimi, tootja, tariifid | Tootja ID ja mudeli autode arv |
| CarManufacturers | ID, tootjanimi | Mudelite kokkuvõtted koos tariifidega |
| Bookings | ID, algus/lõpp, auto number, tootja/mudel, kasutaja nimi | Auto/kasutaja ID, läbisõidud ja broneerimise tariifid |
| Invoices | ID, kuupäevad, makstuse olek, kasutaja nimi | Kasutaja ID/e-post, arveread ja nende summa |
| InvoiceLines | ID, kirjeldus, hind, ühik, kogus, summa, broneeringu ID, auto number | Broneeringu algus ja lõpp |

DTO-d projitseeritakse SQL-päringus `Select` abil. Vastustes pole EF-i
entiteete ega ringviidetega navigatsioonide graafi. Loend ei laadi detailide
kollektsioone. DTO-de lisamine ei muuda andmebaasi skeemi ega ridu.

Praegustes algandmetes pole InvoiceLines seotud Invoices-kirjetega:
SeedData määrab arvereale ainult BookingId. Seetõttu on arve detailis
`lines: []` ja `total: 0`, kuni tegelikud arveread on arvega seotud.
DTO ei seosta ridu arvetega oletuslikult näiteks kasutaja või sama ID järgi.

### Uue lepingu kontroll

Kõigi seitsme kontrolleri esimene leht andis 200, `rowCount: 35`,
`pageCount: 4` ja 10 tulemust. Teise lehe esimene ID oli 11. Get/1 andis
200 ja `value.id: 1`; Get/2147483647 andis 404. Kontrolliti JSON-i
sisutiipi ning parooli ja IsAdmin puudumist loendi- ja detailvastustest.
Broneeringute 4. lehel oli 5 kirjet, 5. lehel 0. Nulli, negatiivseid ja
int.MaxValue piirjuhtumeid kontrolliti HTTP kaudu.

## Varasem kontroll enne DTO-de ja lehekülgede lisamist

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
