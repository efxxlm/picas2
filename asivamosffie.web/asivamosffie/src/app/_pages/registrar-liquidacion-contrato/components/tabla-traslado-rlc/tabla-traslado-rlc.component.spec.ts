import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaTrasladoRlcComponent } from './tabla-traslado-rlc.component';

describe('TablaTrasladoRlcComponent', () => {
  let component: TablaTrasladoRlcComponent;
  let fixture: ComponentFixture<TablaTrasladoRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaTrasladoRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaTrasladoRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
