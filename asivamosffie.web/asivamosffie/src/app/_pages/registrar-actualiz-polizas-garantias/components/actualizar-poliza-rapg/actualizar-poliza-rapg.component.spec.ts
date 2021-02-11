import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarPolizaRapgComponent } from './actualizar-poliza-rapg.component';

describe('ActualizarPolizaRapgComponent', () => {
  let component: ActualizarPolizaRapgComponent;
  let fixture: ComponentFixture<ActualizarPolizaRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarPolizaRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarPolizaRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
