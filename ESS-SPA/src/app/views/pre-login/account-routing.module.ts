import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';

import {OrganisationAccountComponent} from './account-type/organisation-account/organisation-account.component'
import {IndividualAccountComponent} from './account-type/individual-account/individual-account.component'

const routes: Routes = [
    {
        path: 'individual-account',
        component: IndividualAccountComponent,
        data: {
          title: 'Individual Account Type'
        }
      },
      {
        path: 'organisation-account',
        component: OrganisationAccountComponent,
        data: {
          title: 'Organisation Account Type'
        }
      },
  ];
  
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AccountRoutingModule {}