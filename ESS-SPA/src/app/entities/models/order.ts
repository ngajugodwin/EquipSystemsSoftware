import {IAddress} from './address';

export interface IOrderToCreate {
    basketId: number;
    deliveryMethodId: number;
    shipToAddress: IAddress
}


export interface IOrder {
    id: number;
    borrowerEmail: string;
    orderDate: string;
    shipToAddress: IAddress;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subtotal: number;
    total: number;
    status: string;
}

export interface IOrderItem {
    itemId: number;
    itemName: string;
    pictureUrl: string;
    price: number;
    quantity: number;
}
