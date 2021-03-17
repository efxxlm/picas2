import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerEjecucionFinancieraComponent } from './ver-ejecucion-financiera.component';

describe('VerEjecucionFinancieraComponent', () => {
  let component: VerEjecucionFinancieraComponent;
  let fixture: ComponentFixture<VerEjecucionFinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerEjecucionFinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerEjecucionFinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
