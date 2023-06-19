
export interface IItem {
    id: number,
    name: string,
    status: string,
    createdAt: Date,
    itemTypeId: number,
    serialNumber: string,
    url: string,
    file: FormData
}