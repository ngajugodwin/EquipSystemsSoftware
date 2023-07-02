import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { loadStripe, Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement } from '@stripe/stripe-js';
import { NgxModalComponent } from 'ngx-modalview';
import { Basket, IBasket } from 'src/app/entities/models/basket';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { ModalData } from 'src/app/entities/models/modalData';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';


@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent extends NgxModalComponent<ModalData, IBasket> implements OnInit, AfterViewInit , OnDestroy, ModalData{
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef<HTMLInputElement>;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc') cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardNumberComplete = false;
  cardExpiryComplete = false;
  cardCvcComplete = false;
  cardErrors: any;
  loading = false;

  paymentForm: FormGroup;

  title: string;
  message: string;
  data: any;

  constructor(private fb: FormBuilder, private basketService: BasketService, 
    private toasterService: ToasterService,
    private checkoutService: CheckoutService) { 
    super();
  }
  

  ngOnInit() {
   this.paymentForm = this.fb.group({
    nameOnCard: ['']
   });
   this.initStripeComponents();
  }

  ngAfterViewInit() {
    
  }

  async initStripeComponents(): Promise<void> {
    await loadStripe('pk_test_51NM6bmHdGjjKRw2W5DipSvypq9Kn8PcDtiBq21i2lRZTErgbGhwxaZWpZ9v1wUBbjVrARWCl0mdd7jg6oQgks13l00xLditF5H').then(stripe => {
      this.stripe = stripe;
      const elements = stripe?.elements();

      if (elements) {
        this.cardNumber = elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on('change', event => {
          this.cardNumberComplete = event.complete;
          if (event.error) this.cardErrors = event.error.message;
          else this.cardErrors = null;
        })

        this.cardExpiry = elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on('change', event => {
          this.cardExpiryComplete = event.complete;
          if (event.error) this.cardErrors = event.error.message;
          else this.cardErrors = null;
        })

        this.cardCvc = elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on('change', event => {
          this.cardCvcComplete = event.complete;
          if (event.error) this.cardErrors = event.error.message;
          else this.cardErrors = null;
        })
      }
    });   
  }

  createPaymentForm () {
    this.paymentForm = this.fb.group({

    })
  }
  
  save() {
    const orderToCreate = this.data.orderToCreate
    const basket: IBasket = this.data.basket;
    const accountName = this.paymentForm.controls['nameOnCard'].value;

      this.checkoutService.createOrder(orderToCreate).subscribe({
        next: (res) => {
          if (res) {
            this.stripe?.confirmCardPayment(basket.clientSecret?.toString()!, {
              payment_method: {
                card: this.cardNumber!,
                billing_details: {
                  name: accountName
                }
              }
            }).then(res => {
              if(res.paymentIntent) {       
                this.basketService.deleteBasket(basket.id);        
               this.basketService.deleteLocalBasket(basket.id);  
               this.result = basket;             
               this.close()             
              } else {
                this.toasterService.showError('SUCCESS', 'Payment failed');
              }
            })
          }
        },
        error: (err: ErrorResponse) => {
          this.toasterService.showError(err.title, err.message);
        }
      })
     }

  ngOnDestroy(): void {
    this.cardNumber?.destroy();
    this.cardExpiry?.destroy();
    this.cardCvc?.destroy();
  }
}
