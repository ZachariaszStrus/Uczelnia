-- 1.Zapytanie obliczaj¹ce miesiêczn¹ wyp³atê dla artysty

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


-- 2.Zapytanie wyœwietlaj¹ce miesiêczne przychody z tantiem, które s¹ powy¿ej przeciêtnych
--   miesiêcznych przychodów z tantiem, uporz¹dkowane malej¹co

SELECT Year(Miesiac) AS Rok, Month(Miesiac) AS Miesiac, Sum(Kwota) As Suma
	FROM WplywyZTantiem 
	GROUP BY Year(Miesiac), Month(Miesiac)
	HAVING Sum(Kwota) > (SELECT Avg(Suma) FROM
							(SELECT Sum(Kwota) As Suma
							 FROM WplywyZTantiem 
							 GROUP BY Year(Miesiac), Month(Miesiac)) AS Sumy)
	ORDER BY Sum(Kwota) DESC;


-- 3.Zapytanie wyœwietlaj¹ce kontrakty i sumê jaka wp³ynê³a ze sprzeda¿y p³yt
--   dziêki podwpisaniu danego kontraktu, uszeregowane malej¹co wg. owej sumy

SELECT ID, KontraktyArtystow.NazwaArtysty, DataPodpisania, 
	   Sum(WplywyZPlyt.Kwota) AS WplywyZPlyt
	FROM KontraktyArtystow, Plyty, WplywyZPlyt
	WHERE ID = Plyty.ID_Kontraktu AND 
		  Tytul = TytulPlyty
	GROUP BY ID, KontraktyArtystow.NazwaArtysty, DataPodpisania
	ORDER BY Sum(WplywyZPlyt.Kwota) DESC;


-- 4.Zapytanie wyœwietlaj¹ce p³ytê o najwiêkszym przychodzie

CREATE VIEW PlytyZWplywami AS
	SELECT Tytul, DataWydania, 
		Sum(Kwota) AS Przychod, Sum(WyplataDlaArtysty) AS WyplataDlaArtysty
		FROM Plyty JOIN WplywyZPlyt 
		ON Tytul = TytulPlyty
		GROUP BY Tytul, DataWydania;

Select * FROM PlytyZWplywami
	WHERE Przychod = (Select Max(Przychod) FROM PlytyZWplywami);


-- 5.Zapytanie wyœwietlaj¹ce ³¹czne miesêczne wp³ywy z p³yt i tantiem, 
--   uporz¹dkowane chronologicznie

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

-- 6.Zapytanie wyœwietlaj¹ce 5 dni miesi¹ca, w których najczêœcie wp³ywaj¹ 
--   zap³aty za tantiemy z rozg³oœni radiowych

SELECT TOP 5 Day(DataWplywu) AS [Dzien Miesiaca], 
	   Count(Day(DataWplywu)) AS [Ilosc Dni]
	FROM WplywyZTantiem
	GROUP BY Day(DataWplywu)
	ORDER BY Count(Day(DataWplywu)) DESC;

-- 7.Zapyanie wyœwietlaj¹ce menad¿erów i liczbê p³yt które sprzeda³y zespo³y,
--   które prowadzili

SELECT ImieMenadzera, NazwiskoMenadzera, Sum(Naklad) AS [Sprzedane plyty]
	FROM Artysci, KontraktyArtystow, Plyty, WydaniaPlyt
	WHERE Nazwa = NazwaArtysty AND ID = ID_Kontraktu AND
		  Tytul = TytulPlyty
	GROUP BY ImieMenadzera, NazwiskoMenadzera
	ORDER BY Sum(Naklad) DESC;
