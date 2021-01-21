import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleOrdenGiroGogComponent } from './detalle-orden-giro-gog.component';

describe('DetalleOrdenGiroGogComponent', () => {
  let component: DetalleOrdenGiroGogComponent;
  let fixture: ComponentFixture<DetalleOrdenGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleOrdenGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleOrdenGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
