export interface IBasket {
    id: number;
    userId: number;
    items: IBasketItem[];
    deliveryMethodId?: number;
    shippingPrice?: number;

}

export interface IBasketItem {
    quantity: number;
    itemId: number;
    price: number;
    type: string;
    picture: string;
    name: string
}

export class Basket implements IBasket {
    id: number;
    items: IBasketItem[] = [];
    userId: number;
}

export interface IBasketTotals {
    shipping: number;
    subTotal: number;
    total: number;
}

// export class TestBasket {
//     id: number;
//     items: any[] = [];
// }