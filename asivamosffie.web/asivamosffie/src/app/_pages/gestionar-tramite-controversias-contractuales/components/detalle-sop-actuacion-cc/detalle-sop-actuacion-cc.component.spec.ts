import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleSopActuacionCcComponent } from './detalle-sop-actuacion-cc.component';

describe('DetalleSopActuacionCcComponent', () => {
  let component: DetalleSopActuacionCcComponent;
  let fixture: ComponentFixture<DetalleSopActuacionCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleSopActuacionCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleSopActuacionCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
