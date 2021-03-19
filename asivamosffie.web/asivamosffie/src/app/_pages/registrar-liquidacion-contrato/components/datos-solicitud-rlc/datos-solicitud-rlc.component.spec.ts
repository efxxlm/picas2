import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosSolicitudRlcComponent } from './datos-solicitud-rlc.component';

describe('DatosSolicitudRlcComponent', () => {
  let component: DatosSolicitudRlcComponent;
  let fixture: ComponentFixture<DatosSolicitudRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosSolicitudRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosSolicitudRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
