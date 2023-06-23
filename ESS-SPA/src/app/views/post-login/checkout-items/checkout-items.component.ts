import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { ReviewBasketComponent } from './review-basket/review-basket.component';
import { NgxModalService } from "ngx-modalview";
import { IDeliveryMethod } from 'src/app/entities/models/deliveryMethod';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';
import { IBasket } from 'src/app/entities/models/basket';

@Component({
  selector: 'app-checkout-items',
  templateUrl: './checkout-items.component.html',
  styleUrls: ['./checkout-items.component.css']
})
export class CheckoutItemsComponent implements OnInit, OnDestroy {
  private $disposable = new Subscription;
  deliveryMethods: IDeliveryMethod[];


  checkoutForm: FormGroup

  constructor(private fb: FormBuilder, 
    private checkoutService: CheckoutService,
    private basketService: BasketService,
    private NgxModalService:NgxModalService,
    private authService: AuthService) { }

  ngOnInit() {
    this.createCheckoutForm();
    this.getUserAddress();
    this.getDeliveryMethods();
  }

  ngOnDestroy(): void {
    this.$disposable.unsubscribe();
  }

  createCheckoutForm() {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipCode: [null, Validators.required],

      }),
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required]
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  getUserAddress() {
    this.authService.getUserAddress().subscribe({
      next: (res) => {
        console.log(res);
        if (res) {
          this.checkoutForm.get('addressForm')?.patchValue(res);
        }
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

  getDeliveryMethods() {
    this.checkoutService.getDeliveryMethods().subscribe({
      next: (res: IDeliveryMethod[]) => {
        this.deliveryMethods = res;
        console.log(res);
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }


  setTheShippingPrice(selectedMethod: any) {
    var result = this.filterDeliverMethods(Number(selectedMethod.target.value));
    
    if (result !== null) {
      this.basketService.setShippingPrice(result);
      this.checkoutForm.get('deliveryForm')?.get('deliveryMethod')?.patchValue(result.id)
    }
    
  }

  filterDeliverMethods(deliverMethodId: number): IDeliveryMethod | null{
   let result = this.deliveryMethods.find(x => x.id === deliverMethodId);  
  return result ?? null;    
  }

  onReviewBasket() {
    this.$disposable = this.NgxModalService.addModal(ReviewBasketComponent, {
      title: 'Review Basket',
      message: '',
      data: {}
    })
    .subscribe(()=>{
       
    });
  }

  createOrder() {
    const basket = this.basketService.getCurrentBasket();
    if (basket) {
      const orderToCreate = this.getOrderToCreate(basket);

      console.log(orderToCreate);

      this.checkoutService.createOrder(orderToCreate).subscribe({
        next: (res) => {
          if (res) {
            this.basketService.deleteLocalBasket(res.basketId);
          }
        },
        error: (err: ErrorResponse) => {
          console.log(err);
        }
      })
    }
  }

  cancel() {}

  private getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: Number(this.checkoutForm.get('deliveryForm')?.get('deliveryMethod')?.value),
      shipToAddress: this.checkoutForm.get('addressForm')?.value
    };
  }


}
