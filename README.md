# Programmeerimine2

Martin Veeberg
TA-25A

Car Renting schema:
<img width="866" height="1004" alt="image" src="https://github.com/user-attachments/assets/9052f9b9-dafb-45f7-8599-1076bf8eda83" />
# Autode rentimise süsteem

## Klassid

### User
- userId: int
- name: String
- email: String
- password: String
- role: Role
- register()
- login()

### Car
- carId: int
- carNumber: String
- mark: String
- model: String
- type: String
- timeRate: decimal
- kmRate: decimal
- isAvailable(): boolean

### Booking
- bookingId: int
- startTime: DateTime
- endTime: DateTime
- startKm: decimal
- endKm: decimal
- status: Status
- start()
- end()
- calculateCost(): decimal

### Invoice
- invoiceId: int
- invoiceNo: String
- invoiceDate: DateTime
- status: Status
- total: decimal
- markAsPaid()

### InvoiceLine
- invoiceLineId: int
- description: String
- amount: decimal

## Seosed

- User 1 : 0..* Booking
- Booking 1 : 1 Car
- Booking 1 : 0..1 Invoice
- User 1 : 0..* Invoice
- Invoice 1 : 1..* InvoiceLine

## Kasutus

1. Kasutaja registreerib konto ja logib sisse.
2. Kasutaja valib vaba auto.
3. Kasutaja broneerib auto.
4. Rendihinna arvutamisel kasutatakse aega ja läbitud kilomeetreid.
5. Kasutaja lõpetab broneeringu.
6. Süsteem koostab arve.
7. Arvele lisatakse rida `Renditeenus auto-number`.
8. Administraator saab arve märkida makstuks.

## Rendihinna arvutus

```text
Rendihind =
rendiminutid × ajatariif
+
läbitud kilomeetrid × kilomeetritariif
