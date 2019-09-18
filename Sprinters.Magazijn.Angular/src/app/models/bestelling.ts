import { BesteldeArtikelen } from './BesteldeArtikelen';
import { Klant } from './Klant';

export class Bestelling {
    id: number;
    voornaam: string;
    achternaam: string;
    adres: string;
    adresRegel2?: any;
    plaats: string;
    postcode: string;
    inBehandeling: boolean;
    besteldeArtikelen: BesteldeArtikelen[];
    klant?: Klant;

}
