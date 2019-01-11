import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

  constructor() { }


  get(key: string): string {
    const val = localStorage.getItem(key);
    return val;
  }
  set(key: string, value: string) {
    localStorage.setItem(key, value);
  }

  getobject(key: string): any {
    const valstr = localStorage.getItem(key);
    if (valstr) {
      const val = JSON.parse(valstr);
      return val;
    } else {
      return undefined;
    }
  }

  setobject(key: string, value: any) {
    const valstr = JSON.stringify(value);
    localStorage.setItem(key, valstr);
  }



}
