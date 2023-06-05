import { NgModule } from '@angular/core';
import { AccountRoutingModule } from './account-routing.module';
// import { IndividualAccountComponent } from './account-type/individual-account/individual-account.component';
import { IndividualAccountComponent } from './account-type/individual-account/individual-account.component';
import { OrganisationAccountComponent } from './account-type/organisation-account/organisation-account.component';
import { ButtonModule, CardModule, FormModule, GridModule } from '@coreui/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { CommonModule } from '@angular/common';


@NgModule({

    imports:[
        AccountRoutingModule,
        CardModule,
        ButtonModule,
        GridModule,
        IconModule,
        FormModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ], 
    exports:[

    ],
    declarations:[
      //  IndividualAccountComponent,
        OrganisationAccountComponent,
        IndividualAccountComponent
    ],
    providers:[

    ]
})

export class AccountModule {}