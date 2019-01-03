import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor() {

    console.log('SettingsService started');

    for (const settingName in settings) {
      if (settings.hasOwnProperty(settingName) && settings[settingName].hasOwnProperty('storageKey')) {
        const setting = settings[settingName];
        const storageKey = setting['storageKey'];
        const storage = localStorage.getItem(storageKey);
        if (!storage) {
          setting.availableColumns = [];
          this.save(storageKey, setting);
        } else {

        }
      }
    }

  }



  save(cachename: string, settingstosave: any) {
    const stringSettings = JSON.stringify(settingstosave);
    localStorage.setItem(cachename, stringSettings);
  }

}

const settings = {
  product: {
    storageKey: 'settings.product',
    allColumns: [
      { name: 'Id', prop: 'id', flexGrow: 1, sortable: 'true' },
      { name: 'Product Code', prop: 'productCode', flexGrow: 1, sortable: 'true' },
      { name: 'Name', prop: 'name', flexGrow: 3, sortable: 'true' },
      { name: 'productUrl', prop: 'productUrl', flexGrow: 1, sortable: 'true' },
      { name: 'active', prop: 'active', flexGrow: 1, sortable: 'true' },
      { name: 'Html', prop: 'html', flexGrow: 1, sortable: 'true' },
      { name: 'Description Html', prop: 'descriptionHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Objectives Html', prop: 'objectivesHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Audience Html', prop: 'audienceHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Prerequisites Html', prop: 'prerequisitesHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Topics Html', prop: 'topicsHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Related Html', prop: 'relatedHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Roadmaps Html', prop: 'roadmapsHtml', flexGrow: 1, sortable: 'true' },
      { name: 'Duration', prop: 'duration', flexGrow: 1, sortable: 'true' },
      { name: 'Duration Type', prop: 'durationType', flexGrow: 1, sortable: 'true' },
      { name: 'Created On', prop: 'createdOn', flexGrow: 1, sortable: 'true' },
      { name: 'Modified On', prop: 'modifiedOn', flexGrow: 1, sortable: 'true' },
      { name: 'Technology Id', prop: 'technologyId', flexGrow: 1, sortable: 'true' },
      { name: 'Technology Name', prop: 'technologyName', flexGrow: 1, sortable: 'true' },
      { name: 'Technology Url', prop: 'technologyUrl', flexGrow: 1, sortable: 'true' },
      { name: 'Vendor Id', prop: 'vendorId', flexGrow: 1, sortable: 'true' },
      { name: 'Vendor Name', prop: 'vendorName', flexGrow: 1, sortable: 'true' },
      { name: 'Vendor Url', prop: 'vendorUrl', flexGrow: 1, sortable: 'true' }
    ],
    selectedColumns: [
      { name: 'Product Code', prop: 'productCode', flexGrow: 1, sortable: 'true' },
      { name: 'Name', prop: 'name', flexGrow: 2, sortable: 'true' },
      { name: 'Active', prop: 'active', flexGrow: 1, sortable: 'true' }],
    availableColumns: [],
    filters: {}
  }
};
