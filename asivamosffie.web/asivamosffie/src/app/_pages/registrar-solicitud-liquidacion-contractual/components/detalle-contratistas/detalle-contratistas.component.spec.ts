import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleContratistasComponent } from './detalle-contratistas.component';

describe('DetalleContratistasComponent', () => {
  let component: DetalleContratistasComponent;
  let fixture: ComponentFixture<DetalleContratistasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleContratistasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleContratistasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
