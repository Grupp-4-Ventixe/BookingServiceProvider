# BookingServiceProvider ![.NET](https://img.shields.io/badge/.NET-9.0-blue)

**BookingServiceProvider** är en mikrotjänst som ansvarar för hantering av bokningar kopplade till events.  
Tjänsten innehåller CRUD-operationer för bokningar samt relaterade objekt som kunduppgifter och adressinformation.  
Den kommunicerar med EventServiceProvider och integreras i bokningsflödet i frontend.

---

## Funktionalitet

- Skapa nya bokningar
- Hämta alla bokningar
- Filtrera bokningar via e-postadress
- Ta bort bokningar inklusive kopplad användare & adress
- Säkerhetslager med API-nyckel
- Swagger för dokumentation
- Testad via integrations- & enhetstester

---

## Endpoints

### `POST /api/bookings`  
Skapar en ny bokning.

**Request body:**
```json
{
  "eventId": "b2c6fa61-b7d6-4e38-808b-daf9480a46ee",
  "firstName": "Test",
  "lastName": "Testsson",
  "email": "Test@example.com",
  "streetName": "TestGatan 1",
  "postalCode": "12345",
  "city": "Stockholm",
  "ticketQuantity": 2
}
```

**Response:**
```text
"Booking added to booking list successfully."
```

### `GET /api/bookings`
Hämtar alla bokningar.

**Response:**
```json
[
  {
    "id": "booking-id",
    "eventId": "b2c6fa61-b7d6-4e38-808b-daf9480a46ee",
    "firstName": "Test",
    "lastName": "Testsson",
    "email": "Test@example.com",
    "city": "Stockholm",
    "ticketQuantity": 2
  }
]
```

### `GET /api/bookings/byemail?email`  
Hämtar bokningar baserat på e-postadress.

**Response:**
```json
[
  {
    "id": "booking-id",
    "eventId": "b2c6fa61-b7d6-4e38-808b-daf9480a46ee",
    "firstName": "Test",
    "lastName": "Testsson",
    "email": "Test@example.com",
    "ticketQuantity": 2
  }
]
```

### `DELETE /api/bookings/{id}`
Tar bort en bokning inklusive kopplad användare och adress.

**Response:**

```text
"Booking deleted"
```


> Endpoints kräver API-nyckel i headers:
> `X-API-KEY: c3VwZXJzZWNyZXQta2V5LTEyMzQ1`

---

## Testning

**Integrationstest:**
- BookingRepository (med EF Core InMemory-databas)

**Enhetstester:**
- BookingService
- BookingRepository (mockad med Moq)

**Täckta scenarier:**
- Lyckad/ogiltig bokning
- Hämta bokningar via e-post
- Misslyckad lagring eller borttagning

---

## Kom igång lokalt

1. Klona repot:
```bash
git clone https://github.com/Grupp-4-Ventixe/BookingServiceProvider.git
```
2. Öppna projektet i Visual Studio

3. Kör projektet:

```bash
Ctrl + F5
```
4. Besök Swagger:
```
https://localhost:{port}/swagger
```

---

## Teknologi och beroenden
- ASP.NET Core Web API (8.0)
- Entity Framework Core
- Swagger / Swashbuckle
- Moq, xUnit, FluentAssertions
- API-nyckel middleware
- React (frontend-koppling)

---

## Diagram

### Aktivitetsdiagram

**Skapa bokning**  
![Flödesdiagram Skapa](./diagrams/Flödesdiagram%20CreateBooking.svg)

### Sekvensdiagram

**Skapa bokning**  
![Sekvens Skapa](./diagrams/Sekvensdiagram%20Booking.svg)


**Radera bokning**  
![Sekvens Radera](./diagrams/Sekvensdiagram%20deleteBooking.svg)

---

## Publicering

Tjänsten var publicerad på Azure:  
 https://ventixe-4-bookingservice.azurewebsites.net/ (nu borttagen)

---
## Integrationer
BookingServiceProvider är kopplad till EventServiceProvider
och anropas via bokningsflödet i Event Details-vyn i frontend.

---

## Författare
Utvecklad genom parprogrammering av:

- Kimberly Hadjal

- Christoffer Öjhagen

---
