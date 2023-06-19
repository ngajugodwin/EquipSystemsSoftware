import {Pagination}from './pagination';

export class ItemParams {
    itemTypeId = 0;
    categoryId = 0;  
    sort = 'name';
    search: string;
}

export class ItemTypeParams {
    categoryId = 0;  
}

export class CategoryParams {
}