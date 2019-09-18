import { WinkelmandItem } from './winkelmand-item';
import { Klant } from './Klant';

export class Bestelling {

    public klantId: string;
    public klant?: Klant;

    public adresregel1: string;
    public adresregel2: string;
    public plaats: string;
    public postcode: string;
    public besteldeArtikelen: WinkelmandItem[];
}
