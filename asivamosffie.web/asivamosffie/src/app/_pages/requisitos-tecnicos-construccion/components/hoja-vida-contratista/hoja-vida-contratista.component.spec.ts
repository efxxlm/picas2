import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HojaVidaContratistaComponent } from './hoja-vida-contratista.component';

describe('HojaVidaContratistaComponent', () => {
  let component: HojaVidaContratistaComponent;
  let fixture: ComponentFixture<HojaVidaContratistaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HojaVidaContratistaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HojaVidaContratistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
