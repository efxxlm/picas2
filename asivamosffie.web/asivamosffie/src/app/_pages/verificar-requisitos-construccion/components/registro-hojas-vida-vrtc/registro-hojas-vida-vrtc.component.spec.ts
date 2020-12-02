import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroHojasVidaVrtcComponent } from './registro-hojas-vida-vrtc.component';

describe('RegistroHojasVidaVrtcComponent', () => {
  let component: RegistroHojasVidaVrtcComponent;
  let fixture: ComponentFixture<RegistroHojasVidaVrtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistroHojasVidaVrtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistroHojasVidaVrtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
