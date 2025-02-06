# 1234
CREATE TABLE Pracownicy (
    id_pracownika INT IDENTITY(1,1) PRIMARY KEY,
    imie          VARCHAR(50) NOT NULL,
    nazwisko      VARCHAR(50) NOT NULL,
    pesel         VARCHAR(20) NOT NULL UNIQUE
);
CREATE TABLE Pracownicy_Zlecenia (
    id_pracownika       INT PRIMARY KEY,
    adres               VARCHAR(200) NOT NULL,
    data_zawarcia_umowy DATE NOT NULL,
    CONSTRAINT FK_PracownicyZlecenia_Pracownicy
         FOREIGN KEY (id_pracownika)
         REFERENCES Pracownicy(id_pracownika)
);
CREATE TABLE Klienci (
    id_klienta     INT IDENTITY(1,1) PRIMARY KEY,
    imie           VARCHAR(50) NOT NULL,
    nazwisko       VARCHAR(50) NOT NULL,
    adres          VARCHAR(200) NOT NULL,
    pesel          VARCHAR(20) NOT NULL,
);
CREATE TABLE Przypisania_Klienci_Pracownicy (
    id_klienta   INT NOT NULL,
    id_pracownika INT NOT NULL,
    PRIMARY KEY (id_klienta, id_pracownika),
    CONSTRAINT FK_Przypisania_Klienci
         FOREIGN KEY (id_klienta)
         REFERENCES Klienci(id_klienta)
         ON DELETE CASCADE,
    CONSTRAINT FK_Przypisania_Pracownicy
         FOREIGN KEY (id_pracownika)
         REFERENCES Pracownicy_Zlecenia(id_pracownika)
         ON DELETE CASCADE
);
