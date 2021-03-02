import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizacionPolizaComponent } from './actualizacion-poliza.component';

describe('ActualizacionPolizaComponent', () => {
  let component: ActualizacionPolizaComponent;
  let fixture: ComponentFixture<ActualizacionPolizaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizacionPolizaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizacionPolizaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
