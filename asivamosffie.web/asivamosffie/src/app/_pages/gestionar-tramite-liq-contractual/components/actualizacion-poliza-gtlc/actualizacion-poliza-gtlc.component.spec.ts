import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizacionPolizaGtlcComponent } from './actualizacion-poliza-gtlc.component';

describe('ActualizacionPolizaGtlcComponent', () => {
  let component: ActualizacionPolizaGtlcComponent;
  let fixture: ComponentFixture<ActualizacionPolizaGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizacionPolizaGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizacionPolizaGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
