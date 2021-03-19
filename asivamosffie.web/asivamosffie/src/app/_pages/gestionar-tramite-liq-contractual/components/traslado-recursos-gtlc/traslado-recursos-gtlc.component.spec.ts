import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrasladoRecursosGtlcComponent } from './traslado-recursos-gtlc.component';

describe('TrasladoRecursosGtlcComponent', () => {
  let component: TrasladoRecursosGtlcComponent;
  let fixture: ComponentFixture<TrasladoRecursosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrasladoRecursosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrasladoRecursosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
