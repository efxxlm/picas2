import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleObraVrtcComponent } from './verdetalle-obra-vrtc.component';

describe('VerdetalleObraVrtcComponent', () => {
  let component: VerdetalleObraVrtcComponent;
  let fixture: ComponentFixture<VerdetalleObraVrtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleObraVrtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleObraVrtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
