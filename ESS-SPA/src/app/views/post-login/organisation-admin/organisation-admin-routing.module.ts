import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ManageOrganisationUsersComponent } from "./manage-organisation-users/manage-organisation-users.component";
import { OrganisationUserComponent } from "./manage-organisation-users/organisation-user/organisation-user.component";
import { UserResolver } from "src/app/shared/resolvers/user.resolver";
import {ChangeUserPasswordComponent} from '../organisation-admin/manage-organisation-users/change-user-password/change-user-password.component';


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
        data: {title: 'Manage Organisation Users / Edit Organisation User'}, resolve: {user: UserResolver}
    },
    {
        path: 'manage-organisation-users/change-password/:id',
        component: ChangeUserPasswordComponent,
        data: {title: 'Manage Organisation Users / Change User Password'}, resolve: {user: UserResolver}
    }
    // {
    //     path: 'manage-organisation-users/changepassword/:id',
    //     component: ChangeUserPasswordComponent,
    //     data: {title: 'Manage Organisation Users / Change User Password'}, resolve: {user: UserResolver}
    // }
  
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class OrganisationAdminRoutingModule {}