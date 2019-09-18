import { browser, element, by } from 'protractor';

export class WinkelMandPage {
    navigateTo() {
        return browser.get('/winkelmand');
    }

    getAantal() {
        const items = element.all(by.css('#items-body app-winkelmand-item'));

        const itemsAantal = items.first().element(by.className('aantal')).getText();

        return itemsAantal;
    }

    private getBestelButton() {
        return element(by.id('bestellingPlaatsen'));
    }


    pushBestelButton() {
        this.getBestelButton().click();
    }
}
