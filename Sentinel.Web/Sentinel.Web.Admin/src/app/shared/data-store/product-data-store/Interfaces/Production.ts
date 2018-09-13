export interface Product {

    productId: number;
    name: string;
    productNumber: string;
    makeFlag: boolean;
    finishedGoodsFlag: boolean;
    color: string;
    safetyStockLevel: number;
    reorderPoint: number;
    standardCost: number;
    listPrice: number;
    size: string;
    sizeUnitMeasureCode: string;
    weightUnitMeasureCode: string;
    weight: number;
    daysToManufacture: number;
    productLine: string;
    class: string;
    style: string;
    productSubcategoryId: number;
    productModelId: number;
    sellStartDate: Date;
    sellEndDate: Date;
    discontinuedDate: Date;
    rowguid: string;
    modifiedDate: Date;
}
