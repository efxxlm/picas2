import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizacionPolizaRlcComponent } from './actualizacion-poliza-rlc.component';

describe('ActualizacionPolizaRlcComponent', () => {
  let component: ActualizacionPolizaRlcComponent;
  let fixture: ComponentFixture<ActualizacionPolizaRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizacionPolizaRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizacionPolizaRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
