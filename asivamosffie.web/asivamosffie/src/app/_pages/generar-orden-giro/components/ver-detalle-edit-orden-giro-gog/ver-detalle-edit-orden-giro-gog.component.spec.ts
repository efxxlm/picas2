import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditOrdenGiroGogComponent } from './ver-detalle-edit-orden-giro-gog.component';

describe('VerDetalleEditOrdenGiroGogComponent', () => {
  let component: VerDetalleEditOrdenGiroGogComponent;
  let fixture: ComponentFixture<VerDetalleEditOrdenGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditOrdenGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditOrdenGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
