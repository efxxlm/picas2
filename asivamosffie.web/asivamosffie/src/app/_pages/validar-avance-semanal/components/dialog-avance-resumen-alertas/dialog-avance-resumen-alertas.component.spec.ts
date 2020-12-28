import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogAvanceResumenAlertasComponent } from './dialog-avance-resumen-alertas.component';

describe('DialogAvanceResumenAlertasComponent', () => {
  let component: DialogAvanceResumenAlertasComponent;
  let fixture: ComponentFixture<DialogAvanceResumenAlertasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogAvanceResumenAlertasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogAvanceResumenAlertasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
