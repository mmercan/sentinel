// import { CommonModule } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { AddMaxValueCheckComponent } from './add-max-value-check.component';

describe('AddMaxValueCheckComponent', () => {
  let component: AddMaxValueCheckComponent;
  let fixture: ComponentFixture<AddMaxValueCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [NgbCollapseModule.forRoot()],
      declarations: [AddMaxValueCheckComponent],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMaxValueCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
