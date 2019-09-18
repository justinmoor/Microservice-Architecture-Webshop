import { Routes } from '@angular/router';
import { MainPage } from './pages/main/main.page';
import { WinkelMandPage } from './pages/winkelmand/winkelmand.page';
import { AdresInvullenPage } from './pages/adres-invullen/adres-invullen.page';
import { BestellingSuccesvolPage } from './pages/bestelling-succesvol/bestelling-succesvol.page';
import { BesteldGuard } from './guards/besteld/besteld.guard';
import { InloggenPage } from './pages/inloggen/inloggen.page';
import { AuthGuard } from './guards/auth/auth.guard';
import { RegistratiePage } from './pages/registratie/registratie.page';
import { ProductDetailsPage } from './pages/product-details/product-details.page';
import { ZoekPage } from './pages/zoek/zoek.page';

export const routes: Routes = [
    {
        path: 'inloggen',
        component: InloggenPage
    },
    {
        path: 'winkelmand',
        component: WinkelMandPage
    },
    {
        path: 'adresinvullen',
        component: AdresInvullenPage,
        canActivate: [AuthGuard]
    },
    {
        path: 'bestellingsuccesvol',
        component: BestellingSuccesvolPage,
        canActivate: [BesteldGuard, AuthGuard]
    },
    {
        path: 'account-aanmaken',
        component: RegistratiePage,
    },
    {
        path: 'zoeken',
        component: ZoekPage,
    },
    {
        path: 'product/:id',
        component: ProductDetailsPage,
    },
    {
        path: '**',
        component: MainPage
    },

];
