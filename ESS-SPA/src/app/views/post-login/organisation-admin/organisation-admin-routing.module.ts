import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ManageOrganisationUsersComponent } from "./manage-organisation-users/manage-organisation-users.component";
import { OrganisationUserComponent } from "./manage-organisation-users/organisation-user/organisation-user.component";


const routes: Routes = [
    {
        path: 'manage-organisation-users', 
        component: ManageOrganisationUsersComponent, 
        data: {title: 'Manage Organisation Users'}, 
        pathMatch: 'full',
    },
    {
        path: 'manage-organisation-users/new',
        component: OrganisationUserComponent,
        data: {title: 'Manage Organisation Users / New Organisation User'}, 
    },
    {
        path: 'manage-organisation-users/edit/:id',
        component: OrganisationUserComponent,
        data: {title: 'Manage Organisation Users / Edit Organisation User'}, 
    }
  
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class OrganisationAdminRoutingModule {}