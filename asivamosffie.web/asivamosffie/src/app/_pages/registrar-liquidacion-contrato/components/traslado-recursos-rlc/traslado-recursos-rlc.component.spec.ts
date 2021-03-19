import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrasladoRecursosRlcComponent } from './traslado-recursos-rlc.component';

describe('TrasladoRecursosRlcComponent', () => {
  let component: TrasladoRecursosRlcComponent;
  let fixture: ComponentFixture<TrasladoRecursosRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrasladoRecursosRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrasladoRecursosRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
