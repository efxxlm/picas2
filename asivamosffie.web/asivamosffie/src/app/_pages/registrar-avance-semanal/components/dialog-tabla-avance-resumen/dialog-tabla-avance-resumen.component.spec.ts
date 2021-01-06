import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogTablaAvanceResumenComponent } from './dialog-tabla-avance-resumen.component';

describe('DialogTablaAvanceResumenComponent', () => {
  let component: DialogTablaAvanceResumenComponent;
  let fixture: ComponentFixture<DialogTablaAvanceResumenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogTablaAvanceResumenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogTablaAvanceResumenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
