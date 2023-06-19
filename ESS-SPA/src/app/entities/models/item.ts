
export interface IItem {
    id: number,
    name: string,
    status: string,
    createdAt: Date,
    itemTypeId: number,
    serialNumber: string,
    price: string,
    url: string,
    file?: FormData
}