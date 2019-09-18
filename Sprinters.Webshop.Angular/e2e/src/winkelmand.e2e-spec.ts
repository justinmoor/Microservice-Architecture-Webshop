import { MainPage } from './pageObjects/main.po';
import { WinkelMandPage } from './pageObjects/winkelmand.po';

describe('Main page', () => {

    const mainPage = new MainPage();

    const winkelmandPage = new WinkelMandPage();


    beforeEach(() => {
        mainPage.navigateTo();
    });

    it('should return the title', () => {
        expect(mainPage.getTitle()).toEqual('Producten');
    });

    it('should add product to shopping card', () => {
        mainPage.pushAddProductButton();
        mainPage.pushAddProductButton();

        winkelmandPage.navigateTo();

        const aantal = winkelmandPage.getAantal();

        expect(aantal).toBe('2');
    });

});
