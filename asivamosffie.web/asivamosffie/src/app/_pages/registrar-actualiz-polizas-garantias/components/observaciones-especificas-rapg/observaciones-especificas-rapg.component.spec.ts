import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesEspecificasRapgComponent } from './observaciones-especificas-rapg.component';

describe('ObservacionesEspecificasRapgComponent', () => {
  let component: ObservacionesEspecificasRapgComponent;
  let fixture: ComponentFixture<ObservacionesEspecificasRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesEspecificasRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesEspecificasRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
