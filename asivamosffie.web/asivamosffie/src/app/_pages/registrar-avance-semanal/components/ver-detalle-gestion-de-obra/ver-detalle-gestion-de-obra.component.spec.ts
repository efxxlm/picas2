import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleGestionDeObraComponent } from './ver-detalle-gestion-de-obra.component';

describe('VerDetalleGestionDeObraComponent', () => {
  let component: VerDetalleGestionDeObraComponent;
  let fixture: ComponentFixture<VerDetalleGestionDeObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleGestionDeObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleGestionDeObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
