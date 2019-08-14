import {
  Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, forwardRef, ChangeDetectionStrategy, DoCheck,
  KeyValueDiffer, KeyValueDiffers, OnDestroy
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
export class StorageComponent implements ControlValueAccessor, OnInit, DoCheck, OnChanges, OnDestroy {

  // @ViewChild('location') location; widget; differ: any;
  private _differ: any;
  private storageLoaded = false;
  _value: any = null;
  valuestringfy: string;

  @Input()
  get value(): any { return this._value; }
  set value(v: any) {
    console.log('change');
    if (v && v !== this._value) {
      this._value = v; this.onChange(this._value);
      this.valuestringfy = JSON.stringify(this._value);
    }
    if (!this._differ && v) {
      this._differ = this._differs.find(v).create();
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

  _save?: boolean = undefined;
  @Input()
  public get save(): boolean { return this._save; }
  public set save(value: boolean) { this._save = value; }

  onChange = (_) => { };
  onTouched = () => { };
  constructor(private _differs: KeyValueDiffers, ) { }
  ngOnInit() {
    // this.widget = this.location.nativeElement;
  }

  writeValue(value: any): void { if (value) { this.value = value; } }

  registerOnChange(fn: any) {
    // console.log('registerOnChange');
    this.onChange = fn;
    setTimeout(() => this.getItems(), 0);
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
        this.storageLoaded = true;
      } else {
        this.onChange(this._value);
        this.storageLoaded = true;
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

  ngDoCheck() {


  }

  private applyChanges(changes: any): any {
    console.log('applyChanges');
    // console.log(changes);
    this.setItems();
  }

  ngOnChanges(changes: { [propName: string]: any }) {
    console.log('Change detected:');
  }
  ngOnDestroy(): void {

    if (this._save && this.storageLoaded && this._differ) {
      const changes = this._differ.diff(this._value);
      if (changes) {
        this.applyChanges(changes);
      }
    }
  }

}
