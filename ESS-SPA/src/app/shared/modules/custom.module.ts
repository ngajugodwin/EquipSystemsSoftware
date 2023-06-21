
import { NgModule } from '@angular/core';
import {StatusFilterComponent} from '../components/status-filter/status-filter.component';
import { CommonModule } from '@angular/common';
import { ModalModule } from '@coreui/angular';
import { PaginationModule } from 'ngx-bootstrap/pagination';
// import { SmartPaginationModule } from '@coreui/angular';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {PagerPaginationComponent} from '../../../app/views/post-login/pager-pagination/pager-pagination.component';
import {PagingHeaderComponent} from '../../views/post-login/paging-header/paging-header.component';
import {OrderTotalsComponent} from '../components/order-totals/order-totals.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        ModalModule,
        PaginationModule.forRoot()
    ],
    exports: [
        // PagerPaginationComponent,
        StatusFilterComponent,
        PaginationModule,
        PagerPaginationComponent,
        PagingHeaderComponent,
        OrderTotalsComponent
    ],
    declarations: [
        StatusFilterComponent,
        PagerPaginationComponent,
        PagingHeaderComponent,
        OrderTotalsComponent
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