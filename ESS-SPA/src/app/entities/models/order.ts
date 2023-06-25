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
    bookingInformation: IBookingInformation;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subTotal: number;
    total: number;
    status: string;    
}

export interface IBookingInformation {
    bookingStatus: string;
    startDate: string;
    endDate: string;
    returnedDate: string;
}

export interface IOrderItem {
    itemId: number;
    itemName: string;
    pictureUrl: string;
    price: number;
    quantity: number;
}

export enum OrderPaymentStatus{
    Pending,
    PaymentReceived,
    PaymentFailed,
}

export enum OrderBookingStatus{
    Pending,
    Approved,
    Closed
}