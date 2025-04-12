SELECT 
I."Nr Indeksu",
I."Nazwa Przedmiotu",
SUM(
  CASE
    WHEN I.Poprawa IS NOT NULL THEN  I.Poprawa* P."Punkty ECTS"
    ELSE  I.Ocena * P."Punkty ECTS"
END) / SUM(P."Punkty ECTS") AS SredniaWazona
FROM Indeks I
join Przedmioty P on I."Nazwa Przedmiotu" = P."Nazwa Przedmiotu"
WHERE "Nr Indeksu" IN 
    (SELECT "Nr Indeksu" FROM student S WHERE S.Miasto='Poznań')
GROUP BY I."Nr Indeksu", I."Nazwa Przedmiotu";
