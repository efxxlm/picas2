import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleMuestrasComponent } from './ver-detalle-muestras.component';

describe('VerDetalleMuestrasComponent', () => {
  let component: VerDetalleMuestrasComponent;
  let fixture: ComponentFixture<VerDetalleMuestrasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleMuestrasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleMuestrasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
