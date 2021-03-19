import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleRlcComponent } from './verdetalle-rlc.component';

describe('VerdetalleRlcComponent', () => {
  let component: VerdetalleRlcComponent;
  let fixture: ComponentFixture<VerdetalleRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
