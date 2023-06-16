import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
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
    name: 'Bookings',
    url: '/my-bookings',
    iconComponent: { name: 'cil-drop' }
  },
  {
    name: 'Self Service',
    url: '/self-service',
    iconComponent: { name: 'cil-pencil' }
  },
  // {
  //   name: 'Components',
  //   title: true
  // },
  // // {
  // //   name: 'Notifications',
  // //   url: '/notifications',
  // //   iconComponent: { name: 'cil-bell' },
  // //   children: [
  // //     {
  // //       name: 'Alerts',
  // //       url: '/notifications/alerts'
  // //     },
  // //     {
  // //       name: 'Badges',
  // //       url: '/notifications/badges'
  // //     },
  // //     {
  // //       name: 'Modal',
  // //       url: '/notifications/modal'
  // //     },
  // //     {
  // //       name: 'Toast',
  // //       url: '/notifications/toasts'
  // //     }
  // //   ]
  // // },
  // {
  //   name: 'Widgets',
  //   url: '/widgets',
  //   iconComponent: { name: 'cil-calculator' },
  //   badge: {
  //     color: 'info',
  //     text: 'NEW'
  //   }
  // },
  // {
  //   title: true,
  //   name: 'Extras'
  // },
  // {
  //   name: 'Pages',
  //   url: '/login',
  //   iconComponent: { name: 'cil-star' },
  //   children: [
  //     {
  //       name: 'Login',
  //       url: '/login'
  //     },
  //     {
  //       name: 'Register',
  //       url: '/register'
  //     },
  //     {
  //       name: 'Error 404',
  //       url: '/404'
  //     },
  //     {
  //       name: 'Error 500',
  //       url: '/500'
  //     }
  //   ]
  // },
  {
    title: true,
    name: 'Organisation'
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
    title: true,
    name: 'Master'
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
        iconComponent: {name: 'cil-User'}
      },
      {
        name: 'Categories & Items',
        url: 'master-admin/manage-categories',
        iconComponent: {name: 'cil-User'}
      }
    ]
  }
];
