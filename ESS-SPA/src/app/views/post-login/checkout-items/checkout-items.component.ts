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
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

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
    private toasterService: ToasterService,
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
    });
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
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  createPaymentIntent() {
    let dm = this.checkoutForm.get('addressForm')?.get('deliveryMethod');

    if (dm?.status === "INVALID") {
      alert('Please select at least one delivery method');
      return;
    }


      return this.basketService.createPaymentIntent().subscribe({
      next: (res) => {
        this.createOrder();
      },
      error: (err: ErrorResponse) => {
       this.toasterService.showError(err.title, err.message);
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
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  getDeliveryMethods() {
    this.checkoutService.getDeliveryMethods().subscribe({
      next: (res: IDeliveryMethod[]) => {
        this.deliveryMethods = res;
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasket();
    if(basket?.deliveryMethodId !== null) {
      this.checkoutForm.get('addressForm')?.get('deliveryMethod')?.patchValue(basket?.deliveryMethodId);
    }
  }


  setTheShippingPrice(selectedMethod: any) {
    var result = this.filterDeliverMethods(Number(selectedMethod.target.value));
    
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
