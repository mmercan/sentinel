import { Injectable } from '@angular/core';

export interface BadgeItem {
  type: string;
  value: string;
}

export interface ChildrenItems {
  state: string;
  name: string;
  type?: string;
}

export interface Menu {
  state: string;
  name: string;
  type: string;
  icon: string;
  badge?: BadgeItem[];
  children?: ChildrenItems[];
}

const MENUITEMS: Menu[] = [
  {
    state: '/',
    name: 'HOME',
    type: 'link',
    icon: 'basic-accelerator'
  }, {
    state: 'authentication',
    name: 'AUTHENTICATION',
    type: 'sub',
    icon: 'basic-lock-open',
    children: [
      {
        state: 'signin',
        name: 'SIGNIN'
      },
      {
        state: 'signup',
        name: 'SIGNUP'
      },
      {
        state: 'forgot',
        name: 'FORGOT'
      },
      {
        state: 'lockscreen',
        name: 'LOCKSCREEN'
      },
    ]
  }, {
    state: 'product',
    name: 'Products',
    type: 'sub',
    icon: 'basic-archive',
    children: [
      {
        state: 'list',
        name: 'list'
      },
      {
        state: 'analytics',
        name: 'Analytics'
      }
    ]
  }, {
    state: 'vendor',
    name: 'Vendors',
    type: 'sub',
    icon: 'basic-helm',
    children: [
      {
        state: 'list',
        name: 'list'
      },
      {
        state: 'add',
        name: 'Add'
      }
    ]
  }, {
    state: 'category',
    name: 'Categories',
    type: 'sub',
    icon: 'basic-folder',
    children: [
      {
        state: 'list',
        name: 'list'
      },
      {
        state: 'add',
        name: 'Add'
      }
    ]
  }, {
    state: 'docs',
    name: 'DOCS',
    type: 'link',
    icon: 'basic-sheet-txt'
  }
];

@Injectable()
export class MenuItems {
  getAll(): Menu[] {
    return MENUITEMS;
  }

  add(menu: Menu) {
    MENUITEMS.push(menu);
  }
}
