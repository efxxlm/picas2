import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesInformeFinalComponent } from './observaciones-informe-final.component';

describe('ObservacionesInformeFinalComponent', () => {
  let component: ObservacionesInformeFinalComponent;
  let fixture: ComponentFixture<ObservacionesInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
