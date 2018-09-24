import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, forwardRef } from '@angular/core';
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
export class StorageComponent implements ControlValueAccessor, OnInit, OnChanges {
  @ViewChild('location') location;
  _name?: string = undefined;
  _value: any = null;

  get value(): any { return this._value; }

  set value(v: any) {
    if (v !== this._value) {
      this._value = v;
      this.onChange(v);
    }
  }


  widget;

  onChange = (_) => { };
  onTouched = () => { };

  @Input()
  get name(): string {
    return this._name;
  }

  set name(value: string) {
    this._name = value;
    if (value) {
      this._name = value;
    }
  }

  constructor() {
  }
  ngOnInit() {
    this.widget = this.location.nativeElement;
  }

  ngOnChanges(event) {

  }

  writeValue(value: any): void {
    this.value = value;
    this.onChange(value);
  }


  registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }
  registerOnTouched(fn: () => void): void { this.onTouched = fn; }

  setDisabledState?(isDisabled: boolean): void {
  }
}
