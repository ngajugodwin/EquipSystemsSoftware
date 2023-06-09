import { Component, OnInit } from '@angular/core';
import {navItems} from '../default-layout/_nav';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import {MenuService} from 'src/app/shared/services/menu-service/menu.service';
import {SUPER_ADMIN_ROLE} from '../../constants/app.constant';
import { INavData } from '@coreui/angular';
@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
})
export class DefaultLayoutComponent implements OnInit {

   // public navItems = navItems;
  public navItems: INavData[] = [];
  SUPER_ADMIN_ROLE: string = SUPER_ADMIN_ROLE;

  public perfectScrollbarConfig = {
    suppressScrollX: true,
  };

  constructor(private authService: AuthService, private menuService: MenuService) {}


  ngOnInit(): void {
   this.initMenu();
  }

  initMenu() {
    const roles = [SUPER_ADMIN_ROLE];
    const menus = this.menuService.getMenus(); // get all menus and sections
    this.navItems = menus;
    if (this.authService.roleMatch(roles)) {
      //validate the role and apply filter to get only organisation admin settings menu 
      this.navItems = menus.filter(menu => menu.name?.toLowerCase() !== 'organisation settings')
    } else {
      //validate the role and apply filter to get only super admin settings menu 
       this.navItems = menus.filter(menu => menu.name?.toLowerCase() !== 'master settings');
    }
  }
}
