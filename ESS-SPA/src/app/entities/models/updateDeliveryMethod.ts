export class UpdateDeliveryMethod{
    basketId: number;
    deliveryMethodId: number;


    /**
     *
     */
    constructor(basketId: number, deliveryMethodId: number) {
        this.basketId = basketId;
        this.deliveryMethodId = deliveryMethodId;
        
    }
}