
import { NgModule } from '@angular/core';
import {StatusFilterComponent} from '../components/status-filter/status-filter.component';
import { CommonModule } from '@angular/common';
import { ModalModule } from '@coreui/angular';
import { PaginationModule } from 'ngx-bootstrap/pagination';
// import { SmartPaginationModule } from '@coreui/angular';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {PagerPaginationComponent} from '../../../app/views/post-login/pager-pagination/pager-pagination.component';
import {PagingHeaderComponent} from '../../views/post-login/paging-header/paging-header.component';
import {OrderTotalsComponent} from '../components/order-totals/order-totals.component';
import {CdkStepperModule} from '@angular/cdk/stepper';
import {StepperComponent} from '../components/stepper/stepper.component';
import {TextInputComponent} from '../components/text-input/text-input.component';
// import {CdkStepperModule} from '@angular/cdk/stepper';
import {BasketSummaryComponent} from '../components/basket-summary/basket-summary.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        ModalModule,
        CdkStepperModule,
        ReactiveFormsModule,
        CdkStepperModule,
        PaginationModule.forRoot()
    ],
    exports: [
        // PagerPaginationComponent,
        StatusFilterComponent,
        PaginationModule,
        PagerPaginationComponent,
        PagingHeaderComponent,
        OrderTotalsComponent,
        CdkStepperModule,     
        StepperComponent,
        ReactiveFormsModule,
        TextInputComponent,
        BasketSummaryComponent
    ],
    declarations: [
        StatusFilterComponent,
        PagerPaginationComponent,
        PagingHeaderComponent,
        OrderTotalsComponent,
        StepperComponent,
        TextInputComponent,
        BasketSummaryComponent
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