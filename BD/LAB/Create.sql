
CREATE TABLE Artysci
(
	Nazwa varchar(255),
	Typ varchar(255) CHECK (Typ = 'Solo' OR Typ = 'Zespol') NOT NULL,
	NumerKonta varchar(255),
	ImieMenadzera varchar(255),
	NazwiskoMenadzera varchar(255),
	PRIMARY KEY (Nazwa)
);

ALTER TABLE Artysci ADD ImieLidera varchar(255);
ALTER TABLE Artysci ADD NazwiskoLidera varchar(255);

CREATE TABLE KontraktyArtystow
(
	ID int IDENTITY(1,1),
	NazwaArtysty varchar(255) NOT NULL,
	DataPodpisania date,
	DataWygasniecia date,
	Typ varchar(255) CHECK (Typ = '360' OR Typ = 'Standardowa' OR Typ = 'P&D') NOT NULL,
	ProcentOdPlyt real CHECK (ProcentOdPlyt > 0 AND ProcentOdPlyt < 100) NOT NULL,
	ProcentOdTantiem real CHECK (ProcentOdTantiem>0 AND ProcentOdTantiem<100) NOT NULL,
	ID_PoprzedniegoKontraktu int,
	PRIMARY KEY (ID),
	FOREIGN KEY (NazwaArtysty) 
		REFERENCES Artysci(Nazwa)
		ON DELETE CASCADE,
	FOREIGN KEY (ID_PoprzedniegoKontraktu) 
		REFERENCES KontraktyArtystow(ID)
		ON DELETE NO ACTION
);

CREATE TABLE Plyty
(
	Tytul varchar(255),
	DataWydania date,
	ID_Kontraktu int NOT NULL,
	PRIMARY KEY (Tytul),
	FOREIGN KEY (ID_Kontraktu) 
		REFERENCES KontraktyArtystow(ID)
		ON DELETE CASCADE
);

CREATE TABLE WydaniaPlyt
(
	TytulPlyty varchar(255),
	Data date,
	Naklad int NOT NULL,
	KosztyWydania money NOT NULL,
	PRIMARY KEY (TytulPlyty, Data),
	FOREIGN KEY (TytulPlyty) 
		REFERENCES Plyty(Tytul)
		ON DELETE CASCADE
);

CREATE TABLE WplywyZPlyt
(
	TytulPlyty varchar(255),
	Miesiac date ,
	Kwota money NOT NULL,
	WyplataDlaArtysty money NOT NULL,
	PRIMARY KEY (TytulPlyty, Miesiac),
	FOREIGN KEY (TytulPlyty) 
		REFERENCES Plyty(Tytul)
		ON DELETE CASCADE
);

CREATE TABLE RozglosnieRadiowe
(
	Nazwa varchar(255),
	PRIMARY KEY (Nazwa)
);

CREATE TABLE KontraktyRozglosniRadiowych
(
	NazwaRozglosni varchar(255),
	DataPodpisania date,
	DataWygasniecia date NOT NULL,
	CenaEmisjiUtworu money NOT NULL,
	DP_PoprzedniegoKontraktu date,
	PRIMARY KEY (NazwaRozglosni, DataPodpisania),
	FOREIGN KEY (NazwaRozglosni) 
		REFERENCES RozglosnieRadiowe(Nazwa)
		ON DELETE CASCADE,
	FOREIGN KEY (NazwaRozglosni,DP_PoprzedniegoKontraktu) 
		REFERENCES KontraktyRozglosniRadiowych(NazwaRozglosni,DataPodpisania)
		ON DELETE NO ACTION
);

CREATE TABLE Zaliczki
(
	TytulPlyty varchar(255),
	DataWyplacenia date NOT NULL,
	Kwota money NOT NULL,
	PRIMARY KEY (TytulPlyty),
	FOREIGN KEY (TytulPlyty) 
		REFERENCES Plyty(Tytul) 
		ON DELETE CASCADE
);

CREATE TABLE NaleznosciZaTantiemy
(
	NazwaRozglosni varchar(255),
	NazwaArtysty varchar(255),
	Miesiac date,
	LiczbaPiosenek int NOT NULL,
	Kwota money NOT NULL,
	ID_KontraktuArtysty int NOT NULL,
	DataPodpisaniaKontraktu date NOT NULL,
	PRIMARY KEY (NazwaArtysty, NazwaRozglosni, Miesiac),
	FOREIGN KEY (ID_KontraktuArtysty) 
		REFERENCES KontraktyArtystow(ID)
		ON DELETE CASCADE,
	FOREIGN KEY (NazwaRozglosni,DataPodpisaniaKontraktu) 
		REFERENCES KontraktyRozglosniRadiowych(NazwaRozglosni,DataPodpisania)
		ON DELETE CASCADE
);

CREATE TABLE WplywyZTantiem
(
	NazwaRozglosni varchar(255),
	NazwaArtysty varchar(255),
	DataWplywu date NOT NULL,
    Miesiac date,
	Kwota money NOT NULL,
	ID_Kontraktu int NOT NULL,
	WyplataDlaArtysty money NOT NULL,
	PRIMARY KEY (NazwaArtysty, NazwaRozglosni, DataWplywu),
	FOREIGN KEY (ID_Kontraktu) 
		REFERENCES KontraktyArtystow(ID)
		ON DELETE NO ACTION,
	FOREIGN KEY (NazwaArtysty,NazwaRozglosni,Miesiac) 
		REFERENCES NaleznosciZaTantiemy(NazwaArtysty,NazwaRozglosni,Miesiac)
		ON DELETE CASCADE
);