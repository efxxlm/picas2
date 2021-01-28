import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDescuentosDirTecnicaComponent } from './obs-descuentos-dir-tecnica.component';

describe('ObsDescuentosDirTecnicaComponent', () => {
  let component: ObsDescuentosDirTecnicaComponent;
  let fixture: ComponentFixture<ObsDescuentosDirTecnicaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDescuentosDirTecnicaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDescuentosDirTecnicaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
