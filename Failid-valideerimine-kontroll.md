# Failid ja valideerimine — 13.11.2026

Muudatused on projektis C:\Users\Martin.Veeberg\source\repos\Programmeerimine2.
Õpetaja näite IFileClient, FileStore ja LocalFileClient muster on kohandatud autorendi projektile.

## Failid

Pildid kuuluvad Car objektile. appsettings.json sisaldab FilesRoot väärtust App_Data/uploads.
Suhteline tee lahendatakse WebAPI projekti juurkausta suhtes. Failid asuvad cars/{CarId}/{juhuslik ID}.{laiend} kaustades.
Faili lisamine tagastab salvestatud failinime, mida kasutatakse lugemisel ja kustutamisel.
Sama algnimega üleslaadimised säilivad eraldi. Andmebaasis failikirjeid praeguses mudelis pole; migratsiooni pole vaja.
Piltidega auto kustutamiseks tuleb esmalt selle pildid eemaldada.

| Tegevus | HTTP päring |
| --- | --- |
| AddFileCommand | POST /api/cars/{carId}/files — multipart/form-data, väli file |
| ListFilesQuery | GET /api/cars/{carId}/files |
| GetFileCommand | GET /api/cars/{carId}/files/{fileName} |
| DeleteFileCommand | DELETE /api/cars/{carId}/files/{fileName} |

Lubatud on PNG, JPEG ja WebP, suurus 1 bait kuni 5 MB. Kontrollitakse laiendi vastavust faili signatuurile.
See kontroll ei dekodeeri kogu pilti. Vigane sisend tagastab 400, puuduv fail GET päringus 404.
Kontrollitakse auto olemasolu, failinime pikkust, keelatud märke, reserveeritud nimesid ja kataloogist väljumise katseid.
LocalFileClient kontrollib lisaks salvestusjuure piire ning ei järgi sümbollinke ega junction-kaustu.

## Validaatorid

Kõigil 28 olemasoleval päringul ja käsul ning neljal uuel failitoimingul on oma validaator.
ValidationBehavior käivitab need enne handlerit ja järjestikku, sest EF DbContext ei toeta paralleelseid päringuid.

| Valdkond | Reeglid |
| --- | --- |
| Kõik Get/Delete | ID > 0 ja kirje olemasolu; Delete keelab sõltuvate kirjete kustutamise |
| Kõik Save | ID >= 0; muutmisel peab kirje olemas olema |
| Kõik List | Page > 0; PageSize 1–100 |
| User | nimi 2–25, telefon 5–20, korrektne ja unikaalne e-post, parool kuni 255; kõik nõutud |
| CarManufacturer | nõutud ja unikaalne nimi kuni 100 |
| CarModel | nõutud nimi kuni 100, tootja olemasolu, nimi tootja piires unikaalne, KmRate 0.01–10 ja HourlyRate 0.01–100 |
| Car | nõutud unikaalne registreerimisnumber kuni 20, mudeli olemasolu |
| Booking | kuupäevad nõutud, lõpp hilisem kui algus, kilomeetrid mitte negatiivsed ja lõpukilomeetrid >= algkilomeetrid; tariifid nagu mudelil; auto ja kasutaja olemasolu |
| Invoice | kuupäevad nõutud, tähtaeg >= arve kuupäev; kasutaja olemasolu |
| InvoiceLine | kirjeldus nõutud kuni 255, ühik nõutud kuni 20; Price 0–100000, Quantity 0.01–100000, Total 0–1000000; Total = Price × Quantity ümardatult kahe komakohani; broneering olemas |

Rahaväljade ja kilomeetrite puhul on lubatud kuni kaks komakohta. Täiendavad tekstipiirangud ja unikaalsusreeglid on siin dokumenteeritud.
InvoiceLine seos arvega arvestab olemasolevat EF varivõtit InvoiceId.

## Kontrollimine

Käivita projekti juurkaustas:

    dotnet build KooliProjekt.sln
    dotnet run --project KooliProjekt.Checks

Checks on eraldi konsooliprojekt, mis kasutab päris SQL Server LocalDB andmebaasi ja päris failisüsteemi ajutist kausta.
Iga käivitus loob unikaalse testandmebaasi ning eemaldab selle finally-plokis.

Tulemused: 257 edukat kontrolli, kõik 32 validaatorit hõlmatud. Kontrollitud on korrektseid ja vigaseid sisendeid, pikkusi, vahemikke, komakohti, seoseid, duplikaate, muutmist, kustutamise piiranguid ning failitoiminguid MediatR valideerimisahela kaudu.
Lisaks tehti 16 edukat HTTP-kontrolli eraldi testandmebaasiga: Swaggeri skeem, üleslaadimine, identsete baitide allalaadimine, loend, omanike eraldatus, vigane sisu, olematu auto, tühi ja üle 5 MB fail, kustutamine ja puuduv fail.
Build õnnestus. Projektis olid juba NuGeti hoiatused: dubleeritud EF Design viide ning System.Security.Cryptography.Pkcs 9.0.11 turvahoiatused. Neid sõltuvusi selle ülesande käigus ei uuendatud.

Visual Studio oli arvutis avatud selle projektiga. Visual Studio Code'i paigaldust ei leitud; failimuudatused on samas kohalikus projektis ja nähtavad Visual Studios.
