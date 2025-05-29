# BookingServiceProvider ![.NET](https://img.shields.io/badge/.NET-9.0-purple)

**BookingServiceProvider** är en mikrotjänst som ansvarar för hantering av bokningar kopplade till events.

Tjänsten tillhandahåller CRUD-operationer för bokningar och tillhörande information såsom bokningsägare och adressuppgifter. Mikrotjänsten används exempelvis från EventServiceProvider och kopplas till frontend via bokningsflödet.

---

## Funktionalitet

- Skapa nya bokningar
- Hämta alla bokningar
- Hämta bokningar utifrån e-postadress
- Ta bort bokningar (inkl. ägare och adress)
- Swagger för API-dokumentation
- API-nyckelvalidering via middleware
- Integrationstester & enhetstester med Moq

---

## Endpoints

### POST `/api/bookings`
Skapar en ny bokning.

```json
{
  "eventId": "uuid",
  "firstName": "Anna",
  "lastName": "Andersson",
  "email": "anna@example.com",
  "streetName": "Gatan 1",
  "postalCode": "12345",
  "city": "Stockholm",
  "ticketQuantity": 2
}
Response:

200 OK – Booking added to booking list successfully

400 Bad Request – Ogiltig indata

500 Internal Server Error

GET /api/bookings
Hämtar alla bokningar.

Response:

200 OK – Lista med bokningar

500 Internal Server Error

GET /api/bookings/byemail?email=...
Hämtar bokningar kopplade till angiven e-postadress.

Response:

200 OK – Lista med matchande bokningar

400 Bad Request – Saknad e-postadress

500 Internal Server Error

DELETE /api/bookings/{id}
Tar bort en bokning inklusive ägare och adress.

Response:

200 OK – Booking deleted

404 Not Found – Bokning saknas

Autentisering
Alla endpoints skyddas med en API-nyckel som skickas i headern:

makefile
Kopiera
Redigera
x-api-key: din-nyckel
Tester
Enhetstester: BookingsService & BookingRepository med Moq

Integrationstest: BookingRepository (EF Core in-memory)

Täcker scenarier som:

Lyckad/ogiltig bokning

Hämta bokningar via e-post

Misslyckad lagring eller borttagning

Kom igång lokalt
Klona repot

bash
Kopiera
Redigera
git clone https://github.com/Grupp-4-Ventixe/BookingServiceProvider.git
Öppna i Visual Studio

Kör projektet: Ctrl + F5

Besök Swagger:

bash
Kopiera
Redigera
https://localhost:{port}/swagger

## Teknologi och beroenden

ASP.NET Core Web API (.NET 9)

Entity Framework Core

Swagger / Swashbuckle

Moq, xUnit, FluentAssertions

API-nyckelmiddleware

Frontend: React (integration)

Diagram
Aktivitetsdiagram – Skapa bokning
![image](https://github.com/user-attachments/assets/3b4faffb-8c26-4122-9f7f-77813a0c5cdb)



Sekvensdiagram – Skapa bokning
![image](https://github.com/user-attachments/assets/d1b6e530-836c-47d6-9699-99f70064f9d2)


Sekvensdiagram – Radera bokning
![image](https://github.com/user-attachments/assets/6081e169-b8c8-42cd-8326-be1a69807f57)
