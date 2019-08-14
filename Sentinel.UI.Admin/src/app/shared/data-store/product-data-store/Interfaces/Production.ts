export interface Product {

    // productId: number;
    // name: string;
    // productNumber: string;
    // makeFlag: boolean;
    // finishedGoodsFlag: boolean;
    // color: string;
    // safetyStockLevel: number;
    // reorderPoint: number;
    // standardCost: number;
    // listPrice: number;
    // size: string;
    // sizeUnitMeasureCode: string;
    // weightUnitMeasureCode: string;
    // weight: number;
    // daysToManufacture: number;
    // productLine: string;
    // class: string;
    // style: string;
    // productSubcategoryId: number;
    // productModelId: number;
    // sellStartDate: Date;
    // sellEndDate: Date;
    // discontinuedDate: Date;
    // rowguid: string;
    // modifiedDate: Date;



    id: number;
    productCode: string;
    name: string;
    productUrl: string;
    active: boolean;
    useTabs: boolean;
    html: string;
    descriptionHtml: string;
    objectivesHtml: string;
    audienceHtml: string;
    prerequisitesHtml: string;
    topicsHtml: string;
    relatedHtml: string;
    roadmapsHtml: string;
    duration: string;
    durationType: string;
    createdOn: string;
    modifiedOn: string;
    technologyId: number;
    technologyName: string;
    technologyUrl: string;
    vendorId: number;
    vendorName: number;
    vendorUrl: string;
}
