import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarActaFirmadaAirComponent } from './dialog-cargar-acta-firmada-air.component';

describe('DialogCargarActaFirmadaAirComponent', () => {
  let component: DialogCargarActaFirmadaAirComponent;
  let fixture: ComponentFixture<DialogCargarActaFirmadaAirComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarActaFirmadaAirComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarActaFirmadaAirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
