import { NgModule } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ManageOrganisationsComponent} from './manage-organisations/manage-organisations.component';
import {ManageCategoriesComponent} from './manage-categories/manage-categories.component';
import {CategoryComponent} from './manage-categories/category/category.component';
import { CategoryResolver } from "src/app/shared/resolvers/category.resolver";
import { ItemTypesComponent } from "./manage-categories/item-types/item-types.component";
import { ItemsComponent } from "./manage-categories/item-types/items/items.component";
import {ItemComponent} from "./manage-categories/item-types/items/item/item.component";
import { ItemTypeComponent } from "./manage-categories/item-types/item-type/item-type.component";
import { ItemResolver } from "src/app/shared/resolvers/item.resolver";
import { ItemTypeResolver } from "src/app/shared/resolvers/item-type.resolver";
import {ManageBookingsComponent} from './manage-bookings/manage-bookings.component';
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
        path: 'manage-bookings', 
        component: ManageBookingsComponent, 
        data: {title: 'Manage Bookings'}
    },
    // {
    //     path: 'manage-categories/new', 
    //     component: CategoryComponent, 
    //     data: {title: 'New Category'}
    // },
    // {
    //     path: 'manage-categories/edit/:id', 
    //     component: CategoryComponent, 
    //     data: {title: 'Edit Category'}, resolve: {category: CategoryResolver}
    // },
    {
        path: 'manage-categories/:id/item-types', 
        component: ItemTypesComponent, 
        data: {title: 'Item Types'}, resolve: {category: CategoryResolver}
    },
    // {
    //     path: 'manage-categories/item-types/new', 
    //     component: ItemTypeComponent, 
    //     data: {title: 'Item Types'}
    // },
    // {
    //     path: 'manage-categories/item-types/edit/:id', 
    //     component: ItemTypeComponent, 
    //     data: {title: 'Item Types'}, resolve: {itemType: ItemTypeResolver}
    // },

    // enddddddddddddddddd
    // {
    //     path: 'manage-categories/item-types/:id/items', 
    //     component: ItemsComponent, 
    //     data: {title: 'Item Types'}, resolve: {itemType: ItemTypeResolver}
    // },
    // {
    //     path: 'manage-categories/item-types/:id/items/new', 
    //     component: ItemsComponent, 
    //     data: {title: 'Item Types'}, resolve: {itemType: ItemTypeResolver}
    // },
    // {
    //     path: 'manage-categories/item-types/:id/item/edit/:id', 
    //     component: ItemComponent, 
    //     data: {title: 'Item Types'}, resolve: {itemType: ItemTypeResolver, item: ItemResolver}
    // },
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports:[RouterModule]
})

export class MasterAdminRoutingModule {}