CREATE TABLE [KljucnaTackaRazvoja] (
    [KljucnaTackaID]  int            NOT NULL IDENTITY,
    [Naziv]           nvarchar(100)  NOT NULL,
    [Opis]            nvarchar(500)  NOT NULL,
    [DatumPocetka]    datetime2      NOT NULL,
    [DatumZavrsetka]  datetime2      NOT NULL,
    CONSTRAINT [PK_KljucnaTackaRazvoja] PRIMARY KEY ([KljucnaTackaID])
);

CREATE TABLE [ClanTima] (
    [ClanTimaID]  int           NOT NULL IDENTITY,
    [Ime]         nvarchar(50)  NOT NULL,
    [Prezime]     nvarchar(50)  NOT NULL,
    [Email]       nvarchar(100) NOT NULL,
    [Uloga]       nvarchar(50)  NOT NULL,
    CONSTRAINT [PK_ClanTima] PRIMARY KEY ([ClanTimaID])
);

CREATE TABLE [Korisnik] (
    [KorisnikID]    int           NOT NULL IDENTITY,
    [KorisnickoIme] nvarchar(50)  NOT NULL,
    [Email]         nvarchar(100) NOT NULL,
    [LozinkaHes]    nvarchar(256) NOT NULL,
    [Salt]          nvarchar(64)  NOT NULL,
    CONSTRAINT [PK_Korisnik] PRIMARY KEY ([KorisnikID])
);

CREATE TABLE [Zadatak] (
    [ZadatakID]           int            NOT NULL IDENTITY,
    [Sifra]               nvarchar(20)   NOT NULL,
    [Naziv]               nvarchar(150)  NOT NULL,
    [Opis]                nvarchar(1000) NOT NULL,
    [TipZadatka]          nvarchar(20)   NOT NULL,
    [Prioritet]           nvarchar(20)   NOT NULL DEFAULT 'Srednji',
    [Status]              nvarchar(20)   NOT NULL DEFAULT 'Otvoren',
    [ProcenatZavrsenosti] int            NOT NULL DEFAULT 0,
    [RokZaZavrsetak]      datetime2      NOT NULL,
    [Napomena]            nvarchar(500)  NOT NULL DEFAULT '',
    [KljucnaTackaID]      int            NOT NULL,
    [ClanTimaID]          int            NOT NULL,
    [DatumKreiranja]      datetime2      NOT NULL,
    CONSTRAINT [PK_Zadatak] PRIMARY KEY ([ZadatakID]),
    CONSTRAINT [UQ_Zadatak_Sifra] UNIQUE ([Sifra]),
    CONSTRAINT [CK_Zadatak_TipZadatka] CHECK ([TipZadatka] IN ('Funkcija', 'Bag', 'Umetnost')),
    CONSTRAINT [CK_Zadatak_Prioritet] CHECK ([Prioritet] IN ('Nizak', 'Srednji', 'Visok')),
    CONSTRAINT [CK_Zadatak_Status] CHECK ([Status] IN ('Otvoren', 'U radu', 'Testiranje', 'Završeno', 'Kasni')),
    CONSTRAINT [CK_Zadatak_Procenat] CHECK ([ProcenatZavrsenosti] BETWEEN 0 AND 100),
    CONSTRAINT [FK_Zadatak_KljucnaTackaRazvoja] FOREIGN KEY ([KljucnaTackaID])
        REFERENCES [KljucnaTackaRazvoja] ([KljucnaTackaID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Zadatak_ClanTima] FOREIGN KEY ([ClanTimaID])
        REFERENCES [ClanTima] ([ClanTimaID]) ON DELETE NO ACTION
);

CREATE TABLE [StavkaZadatka] (
    [StavkaID]        int           NOT NULL IDENTITY,
    [ZadatakID]       int           NOT NULL,
    [RedniBroj]       int           NOT NULL,
    [NazivPodzadatka] nvarchar(200) NOT NULL,
    [Zavrseno]        bit           NOT NULL DEFAULT 0,
    [DatumKreiranja]  datetime2     NOT NULL,
    CONSTRAINT [PK_StavkaZadatka] PRIMARY KEY ([StavkaID]),
    CONSTRAINT [CK_StavkaZadatka_RedniBroj] CHECK ([RedniBroj] BETWEEN 1 AND 50),
    CONSTRAINT [FK_StavkaZadatka_Zadatak] FOREIGN KEY ([ZadatakID])
        REFERENCES [Zadatak] ([ZadatakID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Zadatak_KljucnaTackaID] ON [Zadatak] ([KljucnaTackaID]);
CREATE INDEX [IX_Zadatak_ClanTimaID]     ON [Zadatak] ([ClanTimaID]);
CREATE INDEX [IX_StavkaZadatka_ZadatakID] ON [StavkaZadatka] ([ZadatakID]);
