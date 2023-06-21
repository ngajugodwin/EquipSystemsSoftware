import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { ClassToggleService, HeaderComponent } from '@coreui/angular';
import { Observable } from 'rxjs';
import { IBasket } from 'src/app/entities/models/basket';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent implements OnInit{
  basket$: Observable<IBasket | null>;

 @Input() sidebarId: string = "sidebar";

  public newMessages = new Array(4)
  public newTasks = new Array(5)
  public newNotifications = new Array(5)

  constructor(private classToggler: ClassToggleService, private authService: AuthService, private basketService: BasketService) {
    super();
  }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;  
  }

  logOut() {
    this.authService.logout();
  }


}
