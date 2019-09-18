import { browser, element, by } from 'protractor';

export class AdresInvullenPage {
    navigateTo() {
        return browser.get('/adresinvullen');
    }

    inputAdresRegel(adresRegel: string) {
        element(by.id('adresregel1')).sendKeys(adresRegel);
    }

    inputPostcode(postcode: string) {
        element(by.id('postcode')).sendKeys(postcode);
    }

    inputPlaats(plaats: string) {
        element(by.id('plaats')).sendKeys(plaats);
    }

    private getPlaatsBestellingButton() {
        return element(by.id('plaatsBestelling'));
    }

    pushPlaatsBestellingButton() {
        this.getPlaatsBestellingButton().click();
    }

}
