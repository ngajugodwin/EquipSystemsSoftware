import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IDeliveryMethod } from 'src/app/entities/models/deliveryMethod';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.css']
})
export class CheckoutDeliveryComponent implements OnInit {
@Input() checkoutForm: FormGroup;
deliveryMethods: IDeliveryMethod[];

  constructor(private checkoutService: CheckoutService, private basketService: BasketService) { }

  ngOnInit() {
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

  setTheShippingPrice(selectedMethod: IDeliveryMethod) {
    this.basketService.setShippingPrice(selectedMethod);
  }

}
