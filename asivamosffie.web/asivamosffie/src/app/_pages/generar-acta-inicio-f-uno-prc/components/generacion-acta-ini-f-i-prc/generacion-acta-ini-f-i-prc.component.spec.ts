import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneracionActaIniFIPreconstruccionComponent } from './generacion-acta-ini-f-i-prc.component';

describe('GeneracionActaIniFIPreconstruccionComponent', () => {
  let component: GeneracionActaIniFIPreconstruccionComponent;
  let fixture: ComponentFixture<GeneracionActaIniFIPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneracionActaIniFIPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneracionActaIniFIPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
