import { CommonModule } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { AddMinValueCheckComponent } from './add-min-value-check.component';

describe('AddMinValueCheckComponent', () => {
  let component: AddMinValueCheckComponent;
  let fixture: ComponentFixture<AddMinValueCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule, NgbCollapseModule.forRoot()],
      declarations: [AddMinValueCheckComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMinValueCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
