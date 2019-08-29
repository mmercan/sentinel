import { TestBed, ComponentFixture, async } from '@angular/core/testing';
import { TestComponent } from '../test-tools/test/test.component';
import { CommonModule } from '@angular/common';
import { By } from '@angular/platform-browser';
import { ToggleFullscreenDirective } from './toggle-fullscreen.directive';
import * as screenfull from 'screenfull';

describe('ToggleFullscreenDirective', () => {

  let component: TestComponent;
  let fixture: ComponentFixture<TestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [CommonModule],
      providers: [],
      declarations: [ToggleFullscreenDirective, TestComponent],
    }).overrideComponent(TestComponent, {
      set: {
        template: '<div class="mytestclass" appToggleFullscreen>NoAuth</div>'
      }
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should onclick exists', () => {
    const directiveEl = fixture.debugElement.query(By.directive(ToggleFullscreenDirective));
    expect(directiveEl).not.toBeNull();

    const directiveInstance = directiveEl.injector.get(ToggleFullscreenDirective);
    expect(directiveInstance.onClick).not.toBeNull();

    directiveInstance.onClick();
    expect(directiveInstance.onClick()).toEqual(true);
  });

});
