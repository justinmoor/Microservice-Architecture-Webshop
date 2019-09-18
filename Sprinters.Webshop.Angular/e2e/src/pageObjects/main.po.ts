import { browser, element, by } from 'protractor';

export class MainPage {
    navigateTo() {
        return browser.get('/');
    }

    getTitle() {
        return element(by.tagName('h1')).getText();
    }

    private getAddButton() {
        return element(by.className('btn btn-secondary'));
    }

    pushAddProductButton() {
        this.getAddButton().click();
    }

    getSuccess() {
        return element(by.className('alert-success'));
    }


}
