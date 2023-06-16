import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { MasterAdminRoutingModule } from './master-admin-routing.module';
import { ManageOrganisationsComponent } from './manage-organisations/manage-organisations.component';
import { AvatarModule, BadgeModule, ButtonModule, CardModule, DropdownModule, FormModule, GridModule, ModalModule, TableModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';
import { UsersRoutingModule } from './manage-users/users/users-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomModule } from 'src/app/shared/modules/custom.module';
import { ManageCategoriesComponent } from './manage-categories/manage-categories.component';
import {ItemTypesComponent} from './manage-categories/item-types/item-types.component'
import {ItemsComponent} from './manage-categories/item-types/items/items.component';
import {ItemComponent} from './manage-categories/item-types/items/item/item.component';
import { CategoryComponent } from './manage-categories/category/category.component';
import {CategoryResolver} from '../../../shared/resolvers/category.resolver';

@NgModule({

    imports:[
        ReactiveFormsModule,
        UsersRoutingModule,
        CardModule,
        FormModule,
        IconModule,
        CommonModule,
        TableModule,
        GridModule,
        ButtonModule,
        MasterAdminRoutingModule,
        AvatarModule,
        DropdownModule,
        CustomModule,
        ModalModule,
        BadgeModule,
        FormsModule,
        CardModule
    ], 
    exports:[

    ],
    declarations:[
        ManageOrganisationsComponent,
        ManageCategoriesComponent,
        CategoryComponent,
        ItemTypesComponent,
        ItemComponent,
        ItemsComponent
    ],
    providers:[
        CategoryResolver
    ]
})

export class MasterAdminModule {}