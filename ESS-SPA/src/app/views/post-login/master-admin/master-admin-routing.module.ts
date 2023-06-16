import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ManageOrganisationsComponent} from './manage-organisations/manage-organisations.component';
import {ManageCategoriesComponent} from './manage-categories/manage-categories.component';
import {CategoryComponent} from './manage-categories/category/category.component';
import { CategoryResolver } from "src/app/shared/resolvers/category.resolver";

const routes: Routes = [
    {
        path: 'manage-users',
        loadChildren: () => import('./manage-users/users/users.module').then(m => m.UsersModule)
    },
    {
        path: 'manage-organisations', 
        component: ManageOrganisationsComponent, 
        data: {title: 'Organisations'}
    },
    {
        path: 'manage-categories', 
        component: ManageCategoriesComponent, 
        data: {title: 'Categories'}
    },
    {
        path: 'manage-categories/new', 
        component: CategoryComponent, 
        data: {title: 'New Category'}
    },
    {
        path: 'manage-categories/edit/:id', 
        component: CategoryComponent, 
        data: {title: 'Edit Caetgory'}, resolve: {category: CategoryResolver}
    },
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class MasterAdminRoutingModule {}