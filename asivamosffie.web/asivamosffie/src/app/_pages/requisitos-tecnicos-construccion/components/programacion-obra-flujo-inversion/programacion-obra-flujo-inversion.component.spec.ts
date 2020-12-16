import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramacionObraFlujoInversionComponent } from './programacion-obra-flujo-inversion.component';

describe('ProgramacionObraFlujoInversionComponent', () => {
  let component: ProgramacionObraFlujoInversionComponent;
  let fixture: ComponentFixture<ProgramacionObraFlujoInversionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramacionObraFlujoInversionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramacionObraFlujoInversionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
