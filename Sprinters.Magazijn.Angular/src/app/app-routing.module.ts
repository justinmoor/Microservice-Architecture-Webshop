import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PrintLayoutComponent } from './components/print-layout/print-layout.component';
import { FactuurComponent } from './components/factuur/factuur.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
