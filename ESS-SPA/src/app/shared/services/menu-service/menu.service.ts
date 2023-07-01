import { Injectable } from '@angular/core';
import { INavData } from '@coreui/angular';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  navItemsV2: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    badge: {
      color: 'info',
      text: ''
      // text: 'NEW'
    }
  },
  {
    title: true,
    name: 'User'
  },
  {
    name: 'My Bookings',
    url: '/my-bookings',
    iconComponent: { name: 'cil-notes' }
  },
  {
    name: 'Shop Items',
    url: '/shop-items',
    iconComponent: { name: 'cil-cart' }
  },
  {
    name: 'Basket',
    url: '/basket',
    iconComponent: { name: 'cil-british-pound' }
  },
  {
    name: 'Checkout Items',
    url: '/checkout-items',
    iconComponent: { name: 'cil-cart' }
  }, 
  {
    name: 'Self Service',
    url: '/self-service',
    iconComponent: { name: 'cil-pencil' }
  },
  {
    name: 'Organisation Settings',
    url: '/organisation-admin',
    iconComponent: { name: 'cil-ApplicationsSettings'},
    children: [
      {
        name: 'Users',
        url: 'organisation-admin/manage-organisation-users',
        iconComponent: {name: 'cil-User'}
      }
    ]
  },
  {
    name: 'Master Settings',
    url: '/master-admin',
    iconComponent: { name: 'cil-ApplicationsSettings'},
    children: [
      {
        name: 'Users',
        url: 'master-admin/manage-users',
        iconComponent: {name: 'cil-User'}
      },
      {
        name: 'Organisations',
        url: 'master-admin/manage-organisations',
        iconComponent: {name: 'cil-people'}
      },
      {
        name: 'Reports',
        url: 'master-admin/reports',
        iconComponent: {name: 'cil-address-book'}
      },
      {
        name: 'Categories & Items',
        url: 'master-admin/manage-categories',
        iconComponent: {name: 'cil-settings'}
      },
      {
        name: 'Manage Bookings',
        url: 'master-admin/manage-bookings',
        iconComponent: {name: 'cil-bookmark'}
      }
    ]
  }
];

  navItems: INavData[] = [
    {
      name: 'Dashboard',
      url: '/dashboard',
      icon: 'icon-speedometer',
      // badge: {
      //   variant: 'info',
      //   text: 'NEW'
      // }
    },
    {
      title: true,
      name: 'Menus'
      // icon: 'https://images.unsplash.com/photo-1490818387583-1baba5e638af?ixlib=rb-1.2.1&auto=format&fit=crop&w=500&q=60'
    },
    {
      name: 'My Bookings',
      url: '/bookings',
      icon: 'fa fa-book'
    },
    {
      name: 'Self Service',
      url: '/self-service',
      icon: 'icon-user'
    },
    {
      name: 'Settings',
      url: '/super-admin',
      icon: 'icon-settings',
      children: [
        {
          name: 'Users',
          url: '/admin/users',
          icon: 'icon-user'
        },
        {
          name: 'Manage Bookings',
          url: '/admin/manage-bookings',
          icon: 'icon-drop'
        },
        {
          name: 'Bookings Report',
          url: '/admin/bookings-report',
          icon: 'fa fa-address-book-o '
        },
        {
          name: 'Manage Templates',
          url: '/admin/templates',
          icon: 'fa fa-file'
        },
        {
          name: 'Manage Classrooms',
          url: '/admin/manage-classrooms',
          icon: 'icon-graduation'
        },
        {
          name: 'Manage Categories',
          url: '/admin/manage-itemTypes',
          icon: 'icon-drop'
        },

      ]
    },
  ];

  constructor() { }

  getMenus() {
    return this.navItemsV2;
  }
}