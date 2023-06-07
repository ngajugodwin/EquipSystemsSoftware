import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ManageOrganisationsComponent} from './manage-organisations/manage-organisations.component';


const routes: Routes = [
    {
        path: 'manage-users',
        loadChildren: () => import('./manage-users/users/users.module').then(m => m.UsersModule)
    },
    {
        path: 'manage-organisations', 
        component: ManageOrganisationsComponent, 
        data: {title: 'Organisations'}
    }
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class MasterAdminRoutingModule {}