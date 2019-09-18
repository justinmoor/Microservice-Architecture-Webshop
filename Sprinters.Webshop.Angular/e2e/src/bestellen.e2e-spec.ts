import { LoginPage } from './pageObjects/login.po';
import { browser, protractor } from 'protractor';
import { AdresInvullenPage } from './pageObjects/adresInvullen.po';
import { BestellingSuccesvolPage } from './pageObjects/bestellingSuccesvol.po';
import { MainPage } from './pageObjects/main.po';
import { RegistratiePage } from './pageObjects/registratie.po';
import { WinkelMandPage } from './pageObjects/winkelmand.po';

describe('Bestellen page', () => {

    const mainPage = new MainPage();
    const loginPage = new LoginPage();
    const adresInvullenPage = new AdresInvullenPage();
    const bestellingSuccesvolPage = new BestellingSuccesvolPage();
    const registratiePage = new RegistratiePage();
    const winkelMandPage = new WinkelMandPage();

    beforeEach(() => {
        registratiePage.registreerGebruiker('e2edefaultbestelling@kantilever.nl', 'wachtwoordKlant123!');
    });

    it('should order a product succesfully', () => {
        loginPage.navigateTo();
        loginPage.login('e2edefaultbestelling@test.nl', 'wachtwoordKlant123!');
        browser.waitForAngular();

        mainPage.navigateTo();
        mainPage.pushAddProductButton();

        winkelMandPage.navigateTo();
        winkelMandPage.pushBestelButton();

        adresInvullenPage.pushPlaatsBestellingButton();

        const success = bestellingSuccesvolPage.getSuccess().getText();

        const until = protractor.ExpectedConditions;
        browser.wait(until.presenceOf(bestellingSuccesvolPage.getSuccess()), 15000, 'Element taking too long to appear in the DOM');

        expect(success).toEqual('Uw bestelling wordt verwerkt!');
    });
});
