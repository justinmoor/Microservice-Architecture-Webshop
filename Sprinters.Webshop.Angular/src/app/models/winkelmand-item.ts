export class WinkelmandItem {
    constructor(
        public artikelnummer: number,
        public afbeeldingUrl: string,
        public naam: string,
        public prijs: number,
        public prijsWithBtw: number,
        public aantal: number,
        ) { }
}

