import { Injectable } from '@angular/core';
import { INavData } from '@coreui/angular';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

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
    return this.navItems;
  }
}