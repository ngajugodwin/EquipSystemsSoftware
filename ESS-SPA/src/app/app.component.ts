import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { IconSetService } from '@coreui/icons-angular';
import { iconSubset } from './icons/icon-subset';
import { Title } from '@angular/platform-browser';
import { BasketService } from './shared/services/basket-service/basket.service';
import { ErrorResponse } from './entities/models/errorResponse';
import { IUser } from './entities/models/user';
import { AuthService } from './shared/services/auth-service/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
})
export class AppComponent implements OnInit {
  title = 'EquipSystems Software';

  jwtHelper = new JwtHelperService();

  constructor(
    private router: Router,
    private titleService: Title,
    private iconSetService: IconSetService,
    private authService: AuthService,
    private basketService: BasketService
  ) {
    titleService.setTitle(this.title);
    iconSetService.icons = { ...iconSubset };
  }

  ngOnInit(): void {

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
    });
    this.initUserProfile();
    this.initCustomerBasket();
   
  }

  initCustomerBasket() {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(Number(basketId)).subscribe({
        next: (res) => {
        }, error: (err: ErrorResponse) => {
         
        }
      })
    }
  }

  initUserProfile() {
    const token = localStorage.getItem('token');

    if(!token)
      return;

    const user: IUser = JSON.parse(localStorage.getItem('user') || '');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = user;
    }
  }



}
