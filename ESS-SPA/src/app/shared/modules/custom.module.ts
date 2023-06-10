
import { NgModule } from '@angular/core';
import {StatusFilterComponent} from '../components/status-filter/status-filter.component';
import { CommonModule } from '@angular/common';
import { ModalModule, PaginationModule } from '@coreui/angular';
// import { SmartPaginationModule } from '@coreui/angular';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
@NgModule({
    imports: [
        CommonModule,
        PaginationModule,
        RouterModule,
        FormsModule,
        ModalModule
    ],
    exports: [
        // PagerPaginationComponent,
        StatusFilterComponent,
    ],
    declarations: [
        StatusFilterComponent,
    ],
    providers: [
        // BsModalRef,
    ],
    entryComponents: [
        // ChangePasswordComponent,
        // DataUploadComponent
    ],
})

export class CustomModule {}