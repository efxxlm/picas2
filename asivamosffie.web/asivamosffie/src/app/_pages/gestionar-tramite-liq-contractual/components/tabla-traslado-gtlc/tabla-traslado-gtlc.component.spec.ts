import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaTrasladoGtlcComponent } from './tabla-traslado-gtlc.component';

describe('TablaTrasladoGtlcComponent', () => {
  let component: TablaTrasladoGtlcComponent;
  let fixture: ComponentFixture<TablaTrasladoGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaTrasladoGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaTrasladoGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
