import { browser, element, by } from 'protractor';

export class RegistratiePage {
    navigateTo() {
        return browser.get('/account-aanmaken');
    }

    getTitle() {
        return element(by.tagName('h1')).getText();
    }

    inputEmail(email: string) {
        element(by.id('email')).sendKeys(email);
    }

    inputWachtwoord(wachtwoord: string) {
        element(by.id('wachtwoord')).sendKeys(wachtwoord);
    }

    inputWachtwoordHerhaal(wachtwoord_herhaal: string) {
        element(by.id('wachtwoord_herhaal')).sendKeys(wachtwoord_herhaal);
    }

    inputVoornaam(voornaam: string) {
        element(by.id('voornaam')).sendKeys(voornaam);
    }

    inputAchternaam(achternaam: string) {
        element(by.id('achternaam')).sendKeys(achternaam);
    }

    inputTelefoon(telefoon: string) {
        element(by.id('telefoon')).sendKeys(telefoon);
    }

    inputAdresregel(adresRegel: string) {
        element(by.id('adresregel')).sendKeys(adresRegel);
    }

    inputPostcode(postcode: string) {
        element(by.id('postcode')).sendKeys(postcode);
    }

    inputPlaats(plaats: string) {
        element(by.id('plaats')).sendKeys(plaats);
    }

    private getRegisterButton() {
        return element(by.id('registreer'));
    }

    pushRegistreerButton() {
        this.getRegisterButton().click();
    }

    getError() {
        return element(by.className('alert-danger'));
    }

    registreerGebruiker(email: string, wachtwoord: string) {
      this.navigateTo();
      this.inputEmail(email);
      this.inputWachtwoord(wachtwoord);
      this.inputWachtwoordHerhaal(wachtwoord);

      this.inputVoornaam('Testnaam');
      this.inputAchternaam('TestAchternaam');
      this.inputTelefoon('0612345678');

      this.inputAdresregel('TestAdres 12');
      this.inputPostcode('1234AB');
      this.inputPlaats('Utrecht');

      this.pushRegistreerButton();
      browser.waitForAngular();
    }

}
