-- 1.Zapytanie obliczaj�ce miesi�czn� wyp�at� dla artysty

SELECT 
	CASE WHEN (SELECT Sum(Kwota) FROM WplywyZPlyt 
			WHERE TytulPlyty = 'I') < (SELECT Kwota 
			FROM Zaliczki WHERE TytulPlyty = 'I') 
		 THEN 0
		 ELSE (SELECT Sum(Kwota) 
				FROM WplywyZPlyt 
				WHERE TytulPlyty = 'I' AND Month(Miesiac) = 12
				AND Year(Miesiac) = 1983)*
			  (SELECT ProcentOdPlyt 
				FROM KontraktyArtystow JOIN Plyty 
				ON ID_Kontraktu = ID
				WHERE Tytul = 'I')
	END;


-- 2.Zapytanie wy�wietlaj�ce miesi�czne przychody z tantiem, kt�re s� powy�ej przeci�tnych
--   miesi�cznych przychod�w z tantiem, uporz�dkowane malej�co

SELECT Year(Miesiac) AS Rok, Month(Miesiac) AS Miesiac, Sum(Kwota) As Suma
	FROM WplywyZTantiem 
	GROUP BY Year(Miesiac), Month(Miesiac)
	HAVING Sum(Kwota) > (SELECT Avg(Suma) FROM
							(SELECT Sum(Kwota) As Suma
							 FROM WplywyZTantiem 
							 GROUP BY Year(Miesiac), Month(Miesiac)) AS Sumy)
	ORDER BY Sum(Kwota) DESC;


-- 3.Zapytanie wy�wietlaj�ce kontrakty i sum� jaka wp�yn�a ze sprzeda�y p�yt
--   dzi�ki podwpisaniu danego kontraktu, uszeregowane malej�co wg. owej sumy

SELECT ID, KontraktyArtystow.NazwaArtysty, DataPodpisania, 
	   Sum(WplywyZPlyt.Kwota) AS WplywyZPlyt
	FROM KontraktyArtystow, Plyty, WplywyZPlyt
	WHERE ID = Plyty.ID_Kontraktu AND 
		  Tytul = TytulPlyty
	GROUP BY ID, KontraktyArtystow.NazwaArtysty, DataPodpisania
	ORDER BY Sum(WplywyZPlyt.Kwota) DESC;


-- 4.Zapytanie wy�wietlaj�ce p�yt� o najwi�kszym przychodzie

CREATE VIEW PlytyZWplywami AS
	SELECT Tytul, DataWydania, 
		Sum(Kwota) AS Przychod, Sum(WyplataDlaArtysty) AS WyplataDlaArtysty
		FROM Plyty JOIN WplywyZPlyt 
		ON Tytul = TytulPlyty
		GROUP BY Tytul, DataWydania;

Select * FROM PlytyZWplywami
	WHERE Przychod = (Select Max(Przychod) FROM PlytyZWplywami);


-- 5.Zapytanie wy�wietlaj�ce ��czne mies�czne wp�ywy z p�yt i tantiem, 
--   uporz�dkowane chronologicznie

SELECT Rok, Miesiac, Sum(Suma) AS Suma FROM
		(SELECT Year(Miesiac) AS Rok, Month(Miesiac) AS Miesiac, Sum(Kwota) AS Suma 
			FROM WplywyZPlyt
			GROUP BY Year(Miesiac), Month(Miesiac)
		UNION ALL
		SELECT Year(Miesiac) AS Rok, Month(Miesiac) AS Miesiac, Sum(Kwota) AS Suma 
			FROM WplywyZTantiem
			GROUP BY Year(Miesiac), Month(Miesiac))
	AS MiesieczneWplywy
	GROUP BY Rok, Miesiac
	ORDER BY Rok, Miesiac;

-- 6.Zapytanie wy�wietlaj�ce 5 dni miesi�ca, w kt�rych najcz�cie wp�ywaj� 
--   zap�aty za tantiemy z rozg�o�ni radiowych

SELECT TOP 5 Day(DataWplywu) AS [Dzien Miesiaca], 
	   Count(Day(DataWplywu)) AS [Ilosc Dni]
	FROM WplywyZTantiem
	GROUP BY Day(DataWplywu)
	ORDER BY Count(Day(DataWplywu)) DESC;

-- 7.Zapyanie wy�wietlaj�ce menad�er�w i liczb� p�yt kt�re sprzeda�y zespo�y,
--   kt�re prowadzili

SELECT ImieMenadzera, NazwiskoMenadzera, Sum(Naklad) AS [Sprzedane plyty]
	FROM Artysci, KontraktyArtystow, Plyty, WydaniaPlyt
	WHERE Nazwa = NazwaArtysty AND ID = ID_Kontraktu AND
		  Tytul = TytulPlyty
	GROUP BY ImieMenadzera, NazwiskoMenadzera
	ORDER BY Sum(Naklad) DESC;
