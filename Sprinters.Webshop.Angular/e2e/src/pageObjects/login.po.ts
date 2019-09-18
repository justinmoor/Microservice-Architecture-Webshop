import { browser, element, by } from 'protractor';

export class LoginPage {
    navigateTo() {
        return browser.get('/inloggen');
    }

    inputEmail(email: string) {
        element(by.id('email')).sendKeys(email);
    }

    inputWachtwoord(wachtwoord: string) {
        element(by.id('wachtwoord')).sendKeys(wachtwoord);
    }

    private getLoginButton() {
        return element(by.id('login'));
    }

    pushLoginButton() {
        this.getLoginButton().click();
    }

    getError() {
        return element(by.className('alert-danger'));
    }

    login(email: string, wachtwoord: string) {
        element(by.id('email')).sendKeys(email);
        element(by.id('wachtwoord')).sendKeys(wachtwoord);
        this.getLoginButton().click();
    }

}
