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
      declarations: [AddMinValueCheckComponent],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMinValueCheckComponent);
    component = fixture.componentInstance;
    component.healthCheckResult = {
      name: 'WorkingSet(450,000KB)',
      status: 'Healthy',
      description: 'min=450000, current=241556',
      duration: '00:00:00.0016270',
      type: 'AddMaxValueCheck',
      data: {
        type: 'AddMaxValueCheck',
        max: 450000,
        current: 241556,
      },
      exception: null,
    };
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
