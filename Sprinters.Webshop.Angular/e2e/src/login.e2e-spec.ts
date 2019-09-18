import { LoginPage } from './pageObjects/login.po';
import { browser, protractor } from 'protractor';
import { MainPage } from './pageObjects/main.po';
import { RegistratiePage } from './pageObjects/registratie.po';

describe('Login page', () => {

    const loginPage = new LoginPage();
    const mainPage = new MainPage();
    const registratiePage = new RegistratiePage();

    beforeEach(() => {
        loginPage.navigateTo();
    });

    it('should show error on invalid credentials', () => {
        loginPage.inputEmail('nietbestaandegebruiker@test.nl');
        loginPage.inputWachtwoord('wachtwoordKlant123!');
        loginPage.pushLoginButton();

        const error = loginPage.getError().getText();

        const until = protractor.ExpectedConditions;
        browser.wait(until.presenceOf(loginPage.getError()), 5000, 'Element taking too long to appear in the DOM');

        expect(error).toEqual('Er kon geen gebruiker gevonden worden met deze gegevens.');
    });

    it('should show redirect to home on success', () => {
        registratiePage.registreerGebruiker('e2edefaultlogin@kantilever.nl', 'wachtwoordKlant123!');
        loginPage.navigateTo();
        loginPage.inputEmail('e2edefaultlogin@kantilever.nl');
        loginPage.inputWachtwoord('wachtwoordKlant123!');
        loginPage.pushLoginButton();

        browser.waitForAngular();
        expect(browser.driver.getCurrentUrl()).toMatch('/');
        expect(mainPage.getSuccess().getText()).toEqual('Je bent succesvol ingelogd');
    });
});
