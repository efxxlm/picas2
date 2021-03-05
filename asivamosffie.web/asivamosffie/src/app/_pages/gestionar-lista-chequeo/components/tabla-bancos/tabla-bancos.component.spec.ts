import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaBancosComponent } from './tabla-bancos.component';

describe('TablaBancosComponent', () => {
  let component: TablaBancosComponent;
  let fixture: ComponentFixture<TablaBancosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaBancosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaBancosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
