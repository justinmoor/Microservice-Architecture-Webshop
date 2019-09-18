import { Routes } from '@angular/router';
import { BestellingPage } from './pages/bestelling/bestelling.page';
import { PrintLayoutComponent } from './components/print-layout/print-layout.component';
import { FactuurComponent } from './components/factuur/factuur.component';
import { AdresComponent } from './components/adres/adres.component';
import { MagazijnDashboardPage } from './pages/magazijn-dashboard/magazijn-dashboard.page';
import { SalesComponent } from './pages/sales/sales.component';
import { BetalingPage } from './pages/betaling/betaling.page';
import { InloggenPage } from './pages/inloggen/inloggen.page';
import { BeheerPage } from './pages/beheer/beheer.page';
import { KlantGuard } from './guards/klant/klant.guard';
import { MagazijnGuard } from './guards/magazijn/magazijn.guard';
import { SalesGuard } from './guards/sales/sales.guard';

export const routes: Routes = [
	{
		path: 'inloggen',
		component: InloggenPage
	},
	{
		path: 'magazijn-dashboard',
		component: MagazijnDashboardPage,
		canActivate: [KlantGuard, MagazijnGuard]
	},
	{
		path: 'laatste-bestelling',
		component: BestellingPage,
		canActivate: [KlantGuard, MagazijnGuard]
	},
	{
		path: 'sales',
		component: SalesComponent,
		canActivate: [KlantGuard, SalesGuard]
	},
	{
		path: 'print',
		outlet: 'print',
		component: PrintLayoutComponent,
		children: [
			{ path: 'factuur', component: FactuurComponent }
		],
		canActivate: [KlantGuard, MagazijnGuard]
	},
	{
		path: 'print',
		outlet: 'print',
		component: PrintLayoutComponent,
		children: [
			{ path: 'adres', component: AdresComponent }
		],
		canActivate: [KlantGuard, MagazijnGuard]
	},
	{
		path: 'betaling',
		component: BetalingPage,
		canActivate: [KlantGuard, SalesGuard]
	},
	{
		path: "beheer",
		component: BeheerPage,
		canActivate: [KlantGuard]
	},
	{
		path: '**',
		component: BeheerPage,
		canActivate: [KlantGuard]
	},

];
