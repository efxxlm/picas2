import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordVigenciasValorComponent } from './accord-vigencias-valor.component';

describe('AccordVigenciasValorComponent', () => {
  let component: AccordVigenciasValorComponent;
  let fixture: ComponentFixture<AccordVigenciasValorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccordVigenciasValorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccordVigenciasValorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
