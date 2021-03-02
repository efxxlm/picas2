import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaTotalOrdenesDeGiroComponent } from './tabla-total-ordenes-de-giro.component';

describe('TablaTotalOrdenesDeGiroComponent', () => {
  let component: TablaTotalOrdenesDeGiroComponent;
  let fixture: ComponentFixture<TablaTotalOrdenesDeGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaTotalOrdenesDeGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaTotalOrdenesDeGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
