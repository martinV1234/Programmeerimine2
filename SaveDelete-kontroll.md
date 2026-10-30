# Save ja Delete käskude kontroll (30.10.2026)

Õpetaja näide: `20261030-KooliProject-SaveDeleteCommand.zip`.
Näite MediatR, OperationResult ja ITransactional muster on kohandatud autorendi
seitsmele feature'ile, kasutades projekti enda välju ja seoseid.

## Kasutamine

Kõigil kontrolleritel on `POST /api/{feature}/save` ja
`DELETE /api/{feature}/delete`. Save keha sisaldab kirje lihtvälju;
`id: 0` loob uue kirje ning olemasolev ID muudab seda.
Delete keha on näiteks `{ "id": 36 }`.
Edukas käsk tagastab 200 ja `{}`. Loodud ID leiab List päringuga.
Käsud töötavad olemasoleva TransactionalBehavior kaudu tehingus.
Save kontrollib viidatud kirjete olemasolu ja muudetava kirje ID-d.
Delete kasutab ExecuteDeleteAsync-i ja andmebaasi olemasolevaid seosereegleid.

## Tegelik kontroll

API käivitati Development keskkonnas aadressil http://localhost:5187/swagger
ning eraldi LocalDB andmebaasiga `KooliProjekt-SaveDelete-Test-20261030`.
Tavapärast arendusandmebaasi ei muudetud. Testandmebaasi genereeriti algandmed.
Päringud käivitati Swagger UI Try it out / Execute kaudu.

| Feature | Loomine (id=0) | Muutmine (id=36) | Kustutamine (id=36) | GET pärast kustutamist |
| --- | --- | --- | --- | --- |
| Users | 200 | 200 | 200 | 404 |
| CarManufacturers | 200 | 200 | 200 | 404 |
| CarModels | 200 | 200 | 200 | 404 |
| Cars | 200 | 200 | 200 | 404 |
| Bookings | 200 | 200 | 200 | 404 |
| Invoices | 200 | 200 | 200 | 404 |
| InvoiceLines | 200 | 200 | 200 | 404 |

List päringutega kinnitati kõigi loodud kirjete olemasolu (ID 36).
Kustutamise järel kontrolliti GET päringuid HTTP kaudu.
Muudeti nime, registreerimisnumbrit, arverea kirjeldust, broneeringu
lõpukilomeetreid või arve makstuse olekut.

## Vigaste andmete tähelepanekud

| Swaggeris saadetud sisend | Tegelik tulemus |
| --- | --- |
| Cars/save: carModelId=999999 | 400, propertyErrors.carModelId |
| CarManufacturers/save: id=999999 | 400, propertyErrors.id |
| Cars/save: id="abc" | 400, ASP.NET JSON tüübi valideerimise viga |
| CarModels/save: kmRate=-5, hourlyRate=-10 | 200, negatiivsed tariifid salvestati |
| Users/save: name="", phone="x", email="not-email", password="" | 200, vigased väärtused salvestati |
| Cars/delete: id=999999 | 200, kuigi vastavat kirjet pole |

Save käskudel pole praegu väljade FluentValidation reegleid. Entity klassi
annotatsioonid ei valideeri eraldi käsuobjekti automaatselt.
Ülesande kohaselt neid täheldatud valideerimispuudusi kohe ei parandatud.

`dotnet build KooliProjekt.sln` õnnestus (0 viga). Olemasolevad NuGet hoiatused:
EF Design topeltviide ning System.Security.Cryptography.Pkcs 9.0.11 paketiga
seotud turbehoiatused.
