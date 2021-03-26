import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeguimientoFinancieroComponent } from './seguimiento-financiero.component';

describe('SeguimientoFinancieroComponent', () => {
  let component: SeguimientoFinancieroComponent;
  let fixture: ComponentFixture<SeguimientoFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeguimientoFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeguimientoFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
