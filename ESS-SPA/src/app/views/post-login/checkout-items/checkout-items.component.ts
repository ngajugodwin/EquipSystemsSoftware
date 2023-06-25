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
import { Router } from '@angular/router';
import { PaymentComponent } from './payment/payment.component';

@Component({
  selector: 'app-checkout-items',
  templateUrl: './checkout-items.component.html',
  styleUrls: ['./checkout-items.component.css']
})
export class CheckoutItemsComponent implements OnInit, OnDestroy {
  private $disposable = new Subscription;
  deliveryMethods: IDeliveryMethod[];
  hasBasket = false;;

  checkoutForm: FormGroup

  constructor(private fb: FormBuilder,
    private router: Router, 
    private checkoutService: CheckoutService,
    private basketService: BasketService,
    private NgxModalService:NgxModalService,
    private authService: AuthService) { }

  ngOnInit() {
    this.getBasket();
    this.createCheckoutForm();
    this.getUserAddress();
    this.getDeliveryMethods();
    this.getDeliveryMethodValue();
  }

  getBasket() {
    this.basketService.basket$.subscribe(res => {
      if (res?.items) {
        this.hasBasket = true;
      }else{
        this.hasBasket = false;
      }
    })

    console.log(this.hasBasket);
  }

  ngOnDestroy(): void {
    this.$disposable.unsubscribe();
  }

  createCheckoutForm() {
    this.checkoutForm = this.fb.group({
      bookingForm: this.fb.group({
        startDate: [null, Validators.required],
        endDate: [null, Validators.required]
      }),
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipCode: [null, Validators.required],
        deliveryMethod: [null, Validators.required]
      }),
      // deliveryForm: this.fb.group({
      //   deliveryMethod: [null, Validators.required]
      // }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  createPaymentIntent() {
    let yy = this.checkoutForm.get('addressForm')?.get('deliveryMethod');

   // let bookingForm = this.checkoutForm.get('bookingForm')?.get('deliveryMethod');

    if (yy?.status === "INVALID") {
      alert('Please select at least one delivery method');
      return;
    }


      return this.basketService.createPaymentIntent().subscribe({
      next: (res) => {
        this.createOrder();
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

  getUserAddress() {
    this.authService.getUserAddress().subscribe({
      next: (res) => {
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

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasket();
    console.log(basket);
    if(basket?.deliveryMethodId !== null) {
      console.log(basket);
      this.checkoutForm.get('addressForm')?.get('deliveryMethod')?.patchValue(basket?.deliveryMethodId);
    }
  }


  setTheShippingPrice(selectedMethod: any) {
    var result = this.filterDeliverMethods(Number(selectedMethod.target.value));
    console.log(result);
    
    if (result !== null) {
      this.basketService.setShippingPrice(result);
      this.checkoutForm.get('addressForm')?.get('deliveryMethod')?.patchValue(result.id)
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


  createOrder () {
    const basket = this.basketService.getCurrentBasket();
    let orderToCreate: any
    if (basket) {
      orderToCreate = this.getOrderToCreate(basket);
    }

    this.$disposable = this.NgxModalService.addModal(PaymentComponent, {
      title: 'Payment',
      message: '',
      data: {
        orderToCreate,
        basket
      }
    })
    .subscribe((res: IBasket)=>{
      if (res) {
        this.router.navigate(['/my-bookings']);
      }
    });
  }

  // createOrder() {
  //   const basket = this.basketService.getCurrentBasket();
  //   if (basket) {
  //     const orderToCreate = this.getOrderToCreate(basket);

  //     console.log(orderToCreate);

  //     this.checkoutService.createOrder(orderToCreate).subscribe({
  //       next: (res) => {
  //         if (res) {
  //           this.basketService.deleteLocalBasket(res.basketId);
  //         }
  //       },
  //       error: (err: ErrorResponse) => {
  //         console.log(err);
  //       }
  //     })
  //   }
  // }

  cancel() {
    this.router.navigate(['/shop-items'])
  }

  private getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: basket.deliveryMethodId,
      shipToAddress: this.checkoutForm.get('addressForm')?.value,
      bookingInfoDto: this.checkoutForm.get('bookingForm')?.value
    };
  }


}
