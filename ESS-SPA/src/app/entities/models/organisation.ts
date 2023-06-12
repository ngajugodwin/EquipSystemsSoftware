
export interface IOrganisation {
    id: number;
    name: string;
    registrationNumber: string;
    address: string;
    dateOfIncorporation: Date;
    status: string;
}

export class Organisation {
    name: string;
    registrationNumber: string;
    address: string;
    dateOfIncorporation: Date;
}