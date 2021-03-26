import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PolizasSegurosComponent } from './polizas-seguros.component';

describe('PolizasSegurosComponent', () => {
  let component: PolizasSegurosComponent;
  let fixture: ComponentFixture<PolizasSegurosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PolizasSegurosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PolizasSegurosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
