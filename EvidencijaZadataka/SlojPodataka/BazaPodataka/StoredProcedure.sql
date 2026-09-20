USE EvidencijaZadatakaDB;
GO

CREATE OR ALTER PROCEDURE sp_DajUkupanBrojZadataka
AS
BEGIN
    SELECT COUNT(*) FROM Zadatak;
END
GO

CREATE OR ALTER PROCEDURE sp_DajBrojZadatakaPoStatusu
    @Status NVARCHAR(20)
AS
BEGIN
    SELECT COUNT(*) FROM Zadatak WHERE Status = @Status;
END
GO

CREATE OR ALTER PROCEDURE sp_DajZadatkePrekoRoka
    @BrojDana INT
AS
BEGIN
    SELECT
        z.ZadatakID,
        z.Sifra,
        z.Naziv,
        z.Status,
        z.Prioritet,
        z.RokZaZavrsetak,
        DATEDIFF(DAY, GETDATE(), z.RokZaZavrsetak) AS DaniDoRoka
    FROM Zadatak z
    WHERE z.Status NOT IN ('Zavrseno', 'Kasni')
      AND DATEDIFF(DAY, GETDATE(), z.RokZaZavrsetak) <= @BrojDana
    ORDER BY z.RokZaZavrsetak ASC;
END
GO