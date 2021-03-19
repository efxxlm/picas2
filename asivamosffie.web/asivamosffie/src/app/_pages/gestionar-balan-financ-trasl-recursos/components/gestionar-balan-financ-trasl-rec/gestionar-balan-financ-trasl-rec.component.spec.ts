import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarBalanFinancTraslRecComponent } from './gestionar-balan-financ-trasl-rec.component';

describe('GestionarBalanFinancTraslRecComponent', () => {
  let component: GestionarBalanFinancTraslRecComponent;
  let fixture: ComponentFixture<GestionarBalanFinancTraslRecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarBalanFinancTraslRecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarBalanFinancTraslRecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
