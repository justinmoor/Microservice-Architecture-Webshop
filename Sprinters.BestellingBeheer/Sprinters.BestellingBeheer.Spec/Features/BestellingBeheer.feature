Feature: BestellingBeheer
	Zodat de risico's worden voorkomen
	Als eigenaar van Kantilever
	Wil ik dat de bestelling(en) die ervoor zorgen dat het krediet boven de €500 gaat, naar de sales worden verstuurd

Scenario Outline: Een bestelling is geplaatst
	Given dat er een bestelling van "<TotaalBedrag>" is geplaatst
	And het Krediet van "<KredietBedrag>" van de klant boven de limiet van "<LimietBedrag>" is
	When de bestelling is geplaatst
	Then de bestelling naar sales wordt "<Toestand>" doorgestuurd

	Examples: 
	| TotaalBedrag | KredietBedrag | LimietBedrag | Toestand |
	| 499.00       | 0             | 500          | niet     |
	| 500          | 0             | 500          | niet     |
	| 100          | 175.80        | 500          | niet     |
	| 480          | 20            | 500          | niet     |
	| 500.01       | 0             | 500          | wel      |
	| 1            | 499.50        | 500          | wel      |