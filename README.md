# Evidencija zadataka na razvoju igre

Višeslojna veb aplikacija za praćenje i evidenciju zadataka u procesu razvoja video igre. Sistem omogućava timu da prati napredak, dodeljuje zadatke članovima tima i automatski eskalira prioritete kada rok ističe.

Projekat je razvijen kao seminarski rad iz predmeta **Razvoj veb softvera**, realizovan kroz četvoroslojnu N-Tier arhitekturu korišćenjem ASP.NET Core tehnologija.

---

## Funkcionalnosti

- **Autentifikacija** — Prijava i registracija korisnika uz SHA-256 hešovanje lozinki sa saltom
- **Evidencija zadataka** — Unos, pregled, izmena i brisanje zadataka sa master-detail unosom podzadataka
- **Ključne tačke razvoja** — Šifarnik tačka razvoja (Prototip, Alfa, Beta, Zlatna verzija)
- **Članovi tima** — Šifarnik članova razvojnog tima sa ulogama
- **Filtriranje** — Pregled zadataka sa filterom po statusu, tipu zadatka i ključnoj tački razvoja
- **Automatska eskalacija prioriteta** — Sistem automatski postavlja prioritet na Visok kada roku zadatka ostaje manje od parametrizovanog broja dana
- **Štampa kartice** — Štampa pojedinačne kartice zadatka u definisanom formatu
- **Štampa spiska** — Štampa filtriranog spiska zadataka
- **JavaScript validacije** — Klijentska validacija polja sa regularnim izrazima

---

## Tehnologije

| Kategorija | Tehnologija |
|---|---|
| Backend | C#, .NET 8, ASP.NET Core Web API, ASP.NET Core MVC |
| Baza podataka | MS SQL Server, Entity Framework Core, ADO.NET, Stored Procedures |
| Frontend | HTML5, Bootstrap 5, JavaScript |
| Arhitektura | N-Tier (4 sloja) |

---

## Arhitektura projekta


**SlojPodataka** upravlja perzistencijom nad MS SQL bazom podataka kroz Entity Framework Core i repozitorijum šablon uz podršku transakcija. Za specifične upite koristi se ADO.NET kroz DBUtils klase i uskladištene procedure.

**SlojPoslovneLogike** sadrži poslovna pravila parametrizovana kroz XML datoteke, što omogućava izmenu pravila bez rekompajliranja koda.

**SlojServisa** implementira REST API endpoint-e i vrši prevođenje entiteta u DTO modele.

**PrezentacioniSloj** realizuje korisnički interfejs kroz ASP.NET Core MVC sa klijentskom validacijom.

---

## Model podataka

| Tabela | Opis |
|---|---|
| `Korisnik` | Sistemska tabela za autentifikaciju |
| `KljucnaTackaRazvoja` | Šifarnik ključnih tačaka razvoja igre |
| `ClanTima` | Šifarnik članova razvojnog tima |
| `Zadatak` | Glavna tabela sa podacima o zadatku |
| `StavkaZadatka` | Detail tabela sa podzadacima (kaskadno brisanje) |

---

## Poslovno pravilo

**Automatska eskalacija prioriteta:**

> AKO je broj preostalih dana do roka zadatka manji ili jednak parametru `RokZaPrioritet` iz XML fajla, i status zadatka nije "Zavrseno", ONDA se prioritet automatski postavlja na "Visok".

Parametar se čita iz:

Podrazumevana vrednost je **3 dana** — može se promeniti bez rekompajliranja koda.

---

## Pokretanje na vašem računaru

### 1. Preduslovi

- Visual Studio 2022
- .NET 8 SDK
- SQL Server (Express ili Developer)
- SQL Server Management Studio (SSMS)

### 2. Kreiranje baze podataka

Pokrenite bat skriptu iz root foldera projekta:

> **Napomena:** Ako vaš SQL Server nije na `localhost`, otvorite `pokreni_bazu.bat` i promenite `-S localhost` na naziv vašeg servera.

### 3. Niz konekcije

Proverite i podesite naziv SQL Server instance u:

```json
"EvidencijaZadatakaDB": "Server=localhost;Database=EvidencijaZadatakaDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 4. Pokretanje u Visual Studio

Desni klik na Solution → **Set Startup Projects** → **Multiple startup projects**:

| Projekat | Akcija | Port |
|---|---|---|
| SlojServisa | Start | 5072 |
| PrezentacioniSloj | Start | 5073 |

Pritisnite **F5**.

### 5. Početni nalozi

| Korisničko ime | Lozinka |
|---|---|
| `admin` | `Admin123!` |
| `milovan` | `Milovan123!` |

---

## Autor

**Milovan Jovanov** — IT 17/22  
Tehnički fakultet "Mihajlo Pupin", Zrenjanin  
Predmet: Razvoj veb softvera (RVS), 2026.
