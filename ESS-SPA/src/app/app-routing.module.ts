import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DefaultLayoutComponent } from './containers';
import { Page404Component } from './views/pages/page404/page404.component';
import { Page500Component } from './views/pages/page500/page500.component';
import { LoginComponent } from './views/pages/login/login.component';
import { RegisterComponent } from './views/pages/register/register.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { IndividualAccountComponent } from './views/pre-login/account-type/individual-account/individual-account.component';
import { OrganisationAccountComponent } from './views/pre-login/account-type/organisation-account/organisation-account.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./views/dashboard/dashboard.module').then((m) => m.DashboardModule)
      },
      // {
      //   path: 'theme',
      //   loadChildren: () =>
      //     import('./views/theme/theme.module').then((m) => m.ThemeModule)
      // },
      // {
      //   path: 'notifications',
      //   loadChildren: () =>
      //     import('./views/notifications/notifications.module').then((m) => m.NotificationsModule)
      // },
      // {
      //   path: 'widgets',
      //   loadChildren: () =>
      //     import('./views/widgets/widgets.module').then((m) => m.WidgetsModule)
      // },
      {
        path: 'pages',
        loadChildren: () =>
          import('./views/pages/pages.module').then((m) => m.PagesModule)
      },
      {
        path: 'account',
        loadChildren: () =>
          import('./views/pre-login/account.module').then((m) => m.AccountModule)
      },
      {
        path: 'shop-items',
        loadChildren: () => import('./views/post-login/shop-items/shop-items.module').then(m => m.ShopItemsModule)
      },
      {
        path: 'basket',
        loadChildren: () => import('./views/post-login/basket/basket.module').then(m => m.BasketModule)
      },
      {
        path: 'checkout',
        loadChildren: () => import('./views/post-login/checkout/checkout.module').then(m => m.CheckOutModule)
      },
      {
        path: 'checkout-items',
        loadChildren: () => import('./views/post-login/checkout-items/checkout-items.module').then(m => m.CheckOutItemsModule)
      },
      {
        path: 'self-service',
        loadChildren: () => import('./views/post-login/self-service/self-service.module').then(m => m.SelfServiceModule)
      },
      {
        path: 'master-admin',
    //    data: {roles: ['SuperAdmin', 'Owner']},
        loadChildren: () => import('./views/post-login/master-admin/master-admin.module').then(m => m.MasterAdminModule)
      },
      {
        path: 'organisation-admin',
    //    data: {roles: ['SuperAdmin', 'Owner']},
        loadChildren: () => import('./views/post-login/organisation-admin/organisation-admin.module').then(m => m.OrganisationAdminModule)
      },
    ]
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'individual-account',
    component: IndividualAccountComponent,
    data: {
      title: 'Individual Account Type'
    }
  },
  {
    path: 'organisation-account',
    component: OrganisationAccountComponent,
    data: {
      title: 'Organisation Account Type'
    }
  },
  {
    path: '404',
    component: Page404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: Page500Component,
    data: {
      title: 'Page 500'
    }
  }, 
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  { path: '**', component: Page404Component }
 // {path: '**', redirectTo: 'dashboard'}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
      anchorScrolling: 'enabled',
      initialNavigation: 'enabledBlocking'
      // relativeLinkResolution: 'legacy'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
