import { RegistratiePage } from './pageObjects/registratie.po';
import { browser, protractor } from 'protractor';
import { MainPage } from './pageObjects/main.po';

describe('Registratie page', () => {

    const registratiePage = new RegistratiePage();
    const mainPage = new MainPage();

    beforeEach(() => {
        registratiePage.navigateTo();
    });

    it('should return the title', () => {
        expect(registratiePage.getTitle()).toEqual('Registreer');
    });

    it('should show password error', () => {
        registratiePage.inputEmail('e2etest@test.nl');
        registratiePage.inputWachtwoord('geheim');
        registratiePage.inputWachtwoordHerhaal('geheim');

        registratiePage.inputVoornaam('Testnaam');
        registratiePage.inputAchternaam('TestAchternaam');
        registratiePage.inputTelefoon('0612345678');

        registratiePage.inputAdresregel('TestAdres 12');
        registratiePage.inputPostcode('1234AB');
        registratiePage.inputPlaats('Utrecht');

        registratiePage.pushRegistreerButton();

        const error = registratiePage.getError().getText();

        const until = protractor.ExpectedConditions;
        browser.wait(until.presenceOf(registratiePage.getError()), 5000, 'Element taking too long to appear in the DOM');

        expect(error).toEqual('Wachtwoord niet sterk genoeg.');
    });


    it('should register an user', () => {
        registratiePage.inputEmail('e2enieuwetestgebruiker@kantilever.nl');
        registratiePage.inputWachtwoord('wachtwoordKlant123!');
        registratiePage.inputWachtwoordHerhaal('wachtwoordKlant123!');

        registratiePage.inputVoornaam('Testnaam');
        registratiePage.inputAchternaam('TestAchternaam');
        registratiePage.inputTelefoon('0612345678');

        registratiePage.inputAdresregel('TestAdres 12');
        registratiePage.inputPostcode('1234AB');
        registratiePage.inputPlaats('Utrecht');

        registratiePage.pushRegistreerButton();

        browser.waitForAngular();
        expect(browser.driver.getCurrentUrl()).toMatch('/');
        expect(mainPage.getSuccess().getText()).toEqual('Account succesvol aangemaakt');
    });

});
