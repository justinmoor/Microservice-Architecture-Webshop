import { TestBed } from '@angular/core/testing';
import { WinkelmandService } from './winkelmand.service';
import { Product } from '../../models/product';
import { WinkelmandItem } from '../../models/winkelmand-item';

describe('WinkelmandService', () => {
    let store;

    beforeEach(() => {
        store = {};
        TestBed.configureTestingModule({});

        const mockLocalStorage = {
            getItem: (key: string): string => {
                return key in store ? store[key] : null;
            },
            setItem: (key: string, value: string) => {
                store[key] = `${value}`;
            },
            removeItem: (key: string) => {
                delete store[key];
            },
            clear: () => {
                store = {};
            }
        };
        spyOn(localStorage, 'getItem')
            .and.callFake(mockLocalStorage.getItem);
        spyOn(localStorage, 'setItem')
            .and.callFake(mockLocalStorage.setItem);
        spyOn(localStorage, 'clear')
            .and.callFake(mockLocalStorage.clear);
        spyOn(localStorage, 'removeItem')
            .and.callFake(mockLocalStorage.removeItem);

    });

    it('should be created', () => {
        const service: WinkelmandService = TestBed.get(WinkelmandService);
        expect(service).toBeTruthy();
    });

	describe('addProduct', () => {
		it('should add item to local storage', () => {
			const item: Product = {
				artikelnummer: 12345,
				naam: 'Testproduct',
				beschrijving: 'TestBeschrijving',
				prijs: 6.75,
				prijsWithBtw: 8.17,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'gz',
				leverancier: 'Gazelle',
				categorieen: ['Fiets'],
				voorraad: 2,
			};
			const service: WinkelmandService = TestBed.get(WinkelmandService);

            service.addProduct(item);

            expect(localStorage.setItem).toHaveBeenCalledTimes(1);
            expect(service.items.length).toBe(1);
            const artikel = JSON.parse(store['winkelmand'])[0];
            expect(artikel.artikelnummer).toEqual(item.artikelnummer);
        });

		it('should add items to local storage', () => {
			const item: Product = {
				artikelnummer: 12345,
				naam: 'Testproduct',
				beschrijving: 'TestBeschrijving',
				prijs: 6.75,
				prijsWithBtw: 8.17,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'gz',
				leverancier: 'Gazelle',
				categorieen: ['Fiets'],
				voorraad: 2,
			};
			const item2: Product = {
				artikelnummer: 54321,
				naam: 'Testproduct2',
				beschrijving: 'TestBeschrijving2',
				prijs: 4.34,
				prijsWithBtw: 5.25,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'bt',
				leverancier: 'Batavus',
				categorieen: ['Fiets'],
				voorraad: 1,
			};
			const service: WinkelmandService = TestBed.get(WinkelmandService);

            service.addProduct(item);
            service.addProduct(item2);

            expect(localStorage.setItem).toHaveBeenCalledTimes(2);
            expect(service.items.length).toBe(2);
            const artikel = JSON.parse(store['winkelmand'])[0];
            expect(artikel.artikelnummer).toEqual(item.artikelnummer);
            const artikel2 = JSON.parse(store['winkelmand'])[1];
            expect(artikel2.artikelnummer).toEqual(item2.artikelnummer);
        });

		it('should increase number on existing item', () => {
			const item: Product = {
				artikelnummer: 12345,
				naam: 'Testproduct',
				beschrijving: 'TestBeschrijving',
				prijs: 6.75,
				prijsWithBtw: 8.17,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'gz',
				leverancier: 'Gazelle',
				categorieen: ['Fiets'],
				voorraad: 2,
			};
			const item2: Product = {
				artikelnummer: 12345,
				naam: 'Testproduct',
				beschrijving: 'TestBeschrijving',
				prijs: 6.75,
				prijsWithBtw: 8.17,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'gz',
				leverancier: 'Gazelle',
				categorieen: ['Fiets'],
				voorraad: 1,
			};
			const service: WinkelmandService = TestBed.get(WinkelmandService);

            service.addProduct(item);
            service.addProduct(item2);

			expect(service.items.length).toBe(1);
			const artikel = JSON.parse(store['winkelmand'])[0];
			expect(artikel.aantal).toEqual(2);
		});
	});
	describe('getProducts', () => {
		it('should get item from local storage', () => {
			const service: WinkelmandService = TestBed.get(WinkelmandService);
			const winkelItem = new WinkelmandItem(12345, 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg', 'Fiets', 6.75, 8.17, 1);
			store['winkelmand'] = JSON.stringify(winkelItem);

            service.getProducts();
            expect(service.items['artikelnummer']).toBe(winkelItem.artikelnummer);
        });

		it('should get item from local storage', () => {
			const service: WinkelmandService = TestBed.get(WinkelmandService);
			const winkelItem = new WinkelmandItem(12345, 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg', 'Band', 6.75, 8.17, 1);
			const winkelItem2 = new WinkelmandItem(54321, 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg', 'Fiets', 8.34, 10.09, 2);
			const winkelItems: WinkelmandItem[] = [
				winkelItem,
				winkelItem2
			];

            store['winkelmand'] = JSON.stringify(winkelItems);

            service.getProducts();

            expect(service.items.length).toBe(2);
        });
    });

	describe('aantal', () => {
		it('should return observable of aantalSubject', () => {
			const item: Product = {
				artikelnummer: 12345,
				naam: 'Testproduct',
				beschrijving: 'TestBeschrijving',
				prijs: 6.75,
				prijsWithBtw: 8.17,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'gz',
				leverancier: 'Gazelle',
				categorieen: ['Fiets'],
				voorraad: 2,
			};
			const item2: Product = {
				artikelnummer: 54321,
				naam: 'Testproduct2',
				beschrijving: 'TestBeschrijving2',
				prijs: 4.34,
				prijsWithBtw: 5.25,
				afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
				leverbaarVanaf: new Date(),
				leveranciercode: 'bt',
				leverancier: 'Batavus',
				categorieen: ['Fiets'],
				voorraad: 1,
			};
			const service: WinkelmandService = TestBed.get(WinkelmandService);

            service.addProduct(item);
            service.addProduct(item2);

            service.aantal().subscribe(value => {
                expect(value).toEqual(2);
            });
        });

		describe('resetWinkelmand', () => {
			it('should clear winkelmand', () => {
				const item: Product = {
					artikelnummer: 12345,
					naam: 'Testproduct',
					beschrijving: 'TestBeschrijving',
					prijs: 6.75,
					prijsWithBtw: 8.17,
					afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
					leverbaarVanaf: new Date(),
					leveranciercode: 'gz',
					leverancier: 'Gazelle',
					categorieen: ['Fiets'],
					voorraad: 2,
				};
				const item2: Product = {
					artikelnummer: 54321,
					naam: 'Testproduct2',
					beschrijving: 'TestBeschrijving2',
					prijs: 4.34,
					prijsWithBtw: 5.25,
					afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
					leverbaarVanaf: new Date(),
					leveranciercode: 'bt',
					leverancier: 'Batavus',
					categorieen: ['Fiets'],
					voorraad: 1,
				};
				const service: WinkelmandService = TestBed.get(WinkelmandService);
				spyOn(sessionStorage, 'removeItem');
				service.addProduct(item);
				service.addProduct(item2);

                service.resetWinkelmand();

                expect(service.items).toEqual([]);
                expect(localStorage.removeItem).toHaveBeenCalledTimes(1);
                expect(sessionStorage.removeItem).toHaveBeenCalledTimes(1);
                service.aantal().subscribe(value => {
                    expect(value).toEqual(0);
                });
            });
        });
        describe('addItem', () => {
            it('should increase item', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.addItem(winkelmandItem);

                expect(service.items[0].aantal).toBe(2);
                expect(localStorage.setItem).toHaveBeenCalledTimes(1);
            });

            it('should not increase item when other product', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };

                  const winkelmandItem2: WinkelmandItem = {
                    artikelnummer: 43212,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.addItem(winkelmandItem2);

                expect(service.items[0].aantal).toBe(1);
                expect(localStorage.setItem).toHaveBeenCalledTimes(0);
            });
        });

        describe('removeItem', () => {
            it('should decrease aantal', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 2,
                    prijsWithBtw: 14.52
                  };
                  const winkelmandItem2: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.removeItem(winkelmandItem2);

                expect(service.items[0].aantal).toBe(1);
                expect(localStorage.setItem).toHaveBeenCalledTimes(1);
            });

            it('should not remove item when aantal lower than 1', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                  const winkelmandItem2: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.removeItem(winkelmandItem2);

                expect(service.items[0].aantal).toBe(1);
                expect(localStorage.setItem).toHaveBeenCalledTimes(0);
            });

            it('should not remove item when other product', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };

                  const winkelmandItem2: WinkelmandItem = {
                    artikelnummer: 43212,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.removeItem(winkelmandItem2);

                expect(service.items[0].aantal).toBe(1);
                expect(localStorage.setItem).toHaveBeenCalledTimes(0);
            });
        });

        describe('removeProduct', () => {
            it('should remove item from list', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 2,
                    prijsWithBtw: 14.52
                  };
                  const winkelmandItem2: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 1,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);
                service.items.push(winkelmandItem);

                service.removeProduct(winkelmandItem2);

                expect(service.items.length).toBe(0);
                expect(localStorage.setItem).toHaveBeenCalledTimes(1);
            });

            it('should not remove item from list', () => {
                const winkelmandItem: WinkelmandItem = {
                    artikelnummer: 1234,
                    afbeeldingUrl: 'test.it',
                    naam: 'Test product',
                    prijs: 12.00,
                    aantal: 2,
                    prijsWithBtw: 14.52
                  };
                const service: WinkelmandService = TestBed.get(WinkelmandService);

                service.removeProduct(winkelmandItem);

                expect(service.items.length).toBe(0);
                expect(localStorage.setItem).toHaveBeenCalledTimes(0);
            });
        });
    });
});
