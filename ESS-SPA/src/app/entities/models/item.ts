
export interface IItem {
    id: number,
    name: string,
    status: string,
    createdAt: Date,
    itemTypeId: number,
    serialNumber: string,
    price: number,
    url: string,
    file?: FormData,
    description: string,
    availableQuantity: number,
    categoryId: number;
    itemState: string;
}