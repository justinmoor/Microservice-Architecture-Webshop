import { browser, element, by } from 'protractor';

export class BestellingSuccesvolPage {
    navigateTo() {
        return browser.get('/bestellingsuccesvol');
    }

    getSuccess() {
        return element(by.className('alert-success'));
    }

}
