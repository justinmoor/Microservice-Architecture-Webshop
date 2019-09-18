export class Product {
    artikelnummer: number;
    naam: string;
    beschrijving: string;
    prijs: number;
    prijsWithBtw: number;
    afbeeldingUrl: string;
    leverbaarVanaf: Date;
    leverbaarTot?: Date;
    leveranciercode: string;
    leverancier: string;
    categorieen: string[];
    voorraad: number;
}
