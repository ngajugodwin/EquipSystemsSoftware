import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountType } from 'src/app/entities/models/accountType';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model:any = {};
  isProcessing = false;
  accountType: AccountType;

  constructor(private authService: AuthService, 
    private toasterService: ToasterService,
    private router: Router, private basketService: BasketService) { }

  login() {
    this.authService.clearStorage();
    this.isProcessing = true;
    this.authService.login(this.model).subscribe({
      next: () => {            
        this.toasterService.showSuccess('SUCCESS', 'Authentication Successful');
        this.router.navigate(['/dashboard']);
        
      }, error: (error: ErrorResponse) => {
        this.isProcessing = false;
        this.toasterService.showError(error.title, error.message);

      },
      complete:() => {
        this.initCustomerBasket();  
      }
    });
  }

  // initCustomerBasket() {
  //   const basketId = localStorage.getItem('basket_id');

  //   if (basketId) {
  //     this.basketService.getBasket(Number(basketId)).subscribe({
  //       next: (res) => {
  //           console.log('Init Basket');
  //       }, error: (err: ErrorResponse) => {
  //         this.toasterService.showError(err.title, err.message);

  //       }
  //     })
  //   }
  // }

  initCustomerBasket() {
    this.basketService.getCurrentLoggedInUserBasket(this.authService.getCurrentUser().id);
  }
}
