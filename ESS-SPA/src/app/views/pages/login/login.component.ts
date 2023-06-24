import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountType } from 'src/app/entities/models/accountType';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model:any = {};
  isProcessing = false;
  accountType: AccountType;

  constructor(private authService: AuthService, private router: Router, private basketService: BasketService) { }

  login() {
    this.authService.clearStorage();
    this.isProcessing = true;
    this.authService.login(this.model).subscribe({
      next: (response) => {
        this.initCustomerBasket();
        this.router.navigate(['/dashboard']);
      console.log('Login successfully');
      console.log(response);
      }, error: (error: ErrorResponse) => {
        this.isProcessing = false;
        console.log(error);
      }
    });
  }

  initCustomerBasket() {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(Number(basketId)).subscribe({
        next: (res) => {
            console.log('Init Basket');
        }, error: (err: ErrorResponse) => {
          console.log(err);
        }
      })
    }
  }


}
