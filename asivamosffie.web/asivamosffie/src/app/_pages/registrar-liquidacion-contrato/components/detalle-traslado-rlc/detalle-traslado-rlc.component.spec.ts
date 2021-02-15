import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTrasladoRlcComponent } from './detalle-traslado-rlc.component';

describe('DetalleTrasladoRlcComponent', () => {
  let component: DetalleTrasladoRlcComponent;
  let fixture: ComponentFixture<DetalleTrasladoRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTrasladoRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTrasladoRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
