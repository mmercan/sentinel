import {
  Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, forwardRef, ChangeDetectionStrategy
} from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';


@Component({
  selector: 'app-storage',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => StorageComponent),
    multi: true
  }],
  templateUrl: './storage.component.html',
  styleUrls: ['./storage.component.scss']
})
export class StorageComponent implements ControlValueAccessor, OnInit {
  // @ViewChild('location') location; widget; differ: any;

  _value: any = null;
  valuestringfy: string;

  @Input()
  get value(): any { return this._value; }
  set value(v: any) {
    console.log('change');
    if (v && v !== this._value) {
      this._value = v; this.onChange(this._value); this.setItems();
      this.valuestringfy = JSON.stringify(this._value);
    }
  }

  _name?: string = undefined;
  @Input()
  get name(): string { return this._name; }
  set name(value: string) { this._name = value; this.getItems(); }

  _type?: string = undefined;
  @Input()
  get type(): string { return this._type; }
  set type(value: string) { this._type = value; this.getItems(); }

  _debug?: boolean = undefined;
  @Input()
  public get debug(): boolean { return this._debug; }
  public set debug(value: boolean) { this._debug = value; }

  onChange = (_) => { };
  onTouched = () => { };
  constructor() { }
  ngOnInit() {
    // this.widget = this.location.nativeElement;
  }

  writeValue(value: any): void { if (value) { this._value = value; } }

  registerOnChange(fn: any) {
    // console.log('registerOnChange');
    this.onChange = fn;

    setTimeout(() => this.getItems(), 0);
    // this.getItems();
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void { }

  getItems(): any {
    console.log('getItems called' + this._name + ' : ' + this._type);
    if (this._name && this._type) {
      let stored: any;
      switch (this._type) {
        case 'local':
          stored = localStorage.getItem(this._name);
          break;
        case 'session':
          stored = sessionStorage.getItem(this._name);
          break;
        case 'cache':
          // stored = CacheStorage.getItem(this._name);
          break;

        default:
          stored = localStorage.getItem(this._name);
          break;
      }
      if (stored) {
        this.valuestringfy = stored;
        this._value = JSON.parse(stored);
        this.onChange(this._value);
        console.log('cached');
      } else {
        this.onChange(this._value);
      }
    }
  }

  setItems(): any {
    if (this._name && this._type && this._value) {
      const store = JSON.stringify(this._value);
      switch (this._type) {
        case 'local':
          localStorage.setItem(this._name, store);
          break;
        case 'session':
          sessionStorage.setItem(this._name, store);
          break;
        case 'cache':
          // stored = CacheStorage.getItem(this._name);
          break;

        default:
          localStorage.setItem(this._name, store);
          break;
      }
    }
  }

  // ngDoCheck() {
  //   // const change = this.differ.diff(this.value);
  //   // console.log('ngDoCheck');
  //   // console.log(change);
  // }
}
