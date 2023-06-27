import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

import {
  PERFECT_SCROLLBAR_CONFIG,
  PerfectScrollbarConfigInterface,
  PerfectScrollbarModule,
} from 'ngx-perfect-scrollbar';

// Import routing module
import { AppRoutingModule } from './app-routing.module';

// Import app component
import { AppComponent } from './app.component';


// Import containers
import {
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
} from './containers';

import {
  AvatarModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonGroupModule,
  CardModule,
  DropdownModule,
  FooterModule,
  FormModule,
  GridModule,
  HeaderModule,
  ListGroupModule,
  NavModule,
  ProgressModule,
  SharedModule,
  SidebarModule,
  TabsModule,
  UtilitiesModule
} from '@coreui/angular';

import { IconModule, IconSetService } from '@coreui/icons-angular';

import {ItemService} from './shared/services/item-service/item.service';
import {ItemTypeService} from './shared/services/itemTypes-service/item-type.service';
import {CategoryService} from './shared/services/category-service/category.service';
import {RoleService} from './shared/services/role-service/role.service';
import {ManageAdminOrganisationService} from './shared/services/manage-admin-organisaton-service/manage-admin-organisation.service';
import {AuthService} from './shared/services/auth-service/auth.service';
import {UserService} from './shared/services/user-service/user.service';
import {OrganisationService} from './shared/services/organisation-service/organisation.service';
import { AuthGuard } from './shared/guards/auth.guard';
import { JwtInterceptorProvider } from './shared/interceptors/jwt.interceptor';
import { HttpClientModule } from '@angular/common/http';
import { MasterAdminModule } from './views/post-login/master-admin/master-admin.module';

import {CustomModule} from './shared/modules/custom.module';
import {CheckoutService} from './shared/services/checkout-service/checkout.service';
import {OrderService} from './shared/services/order-service/order.service';
import {OrdersReportService} from './shared/services/orders-report-service/orders-report.service';
import {ToasterService} from './shared/services/toaster-service/toaster.service';
import { NgxSmartModalModule, NgxSmartModalService } from 'ngx-smart-modal';
import { ModalDialogModule } from 'ngx-modal-dialog';
import { DefaultNgxModalOptionConfig, NgxModalView, defaultNgxModalOptions } from 'ngx-modalview';
import { FileUploadModule } from 'ng2-file-upload';
// import { FileUploadModule } from 'ng2-file-upload/file-upload/file-upload.module';
// import { FileUploadModule } from '../../node_modules/ng2-file-upload/file-upload/file-upload.module';
import {SelfService} from './shared/services/self-service/self.service';    

import { UploaderModule } from "angular-uploader";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ToastrModule } from 'ngx-toastr';

export function tokenGetter() {
  return localStorage.getItem('token');
}

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
};

const APP_CONTAINERS = [
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
];

@NgModule({
  declarations: [
    AppComponent, 
    
    ...APP_CONTAINERS
    
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AvatarModule,
    BreadcrumbModule,
    FooterModule,
    DropdownModule, 
    GridModule,
    HeaderModule,
    SidebarModule,
    IconModule,
    PerfectScrollbarModule,
    NavModule,
    FormModule,
    UtilitiesModule,
    ButtonGroupModule,
    ReactiveFormsModule,
    SidebarModule,
    SharedModule,
    TabsModule,
    ListGroupModule,
    ProgressModule,
    BadgeModule,
    ListGroupModule,
    CardModule,
    HttpClientModule,
    CustomModule,
    UploaderModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    PaginationModule.forRoot(),
    FileUploadModule,
    NgxModalView.forRoot({container: 'modal-container'}, {...defaultNgxModalOptions, ...{
      closeOnEscape: true,
      closeOnClickOutside: true,
      wrapperDefaultClasses: 'modal fade-anim',
      wrapperClass: 'in',
      animationDuration: 400,
      autoFocus: true,
      draggable: true
    }}),

    // NgxSmartModalModule.forRoot(),
    ModalDialogModule.forRoot()
   // AccountModule

  ],
  providers: [
    // {
    //   provide: LocationStrategy,
    //   useClass: HashLocationStrategy,
    // },
    {
      provide: DefaultNgxModalOptionConfig,
      useValue: {...defaultNgxModalOptions, ...{ closeOnEscape: true, closeOnClickOutside: true }}
    },
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
    IconSetService,
    Title,

    // Code
    SelfService,
    ToasterService,
    AuthService,
    UserService,
    OrderService,
    OrganisationService,
    ManageAdminOrganisationService,
    RoleService,
    CategoryService,
    ItemTypeService,
    ItemService,
    CheckoutService,
    JwtInterceptorProvider,
    AuthGuard,
    NgxSmartModalService,
    OrdersReportService
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
