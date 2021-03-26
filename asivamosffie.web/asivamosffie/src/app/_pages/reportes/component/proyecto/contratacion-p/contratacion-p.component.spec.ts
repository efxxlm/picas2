import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContratacionPComponent } from './contratacion-p.component';

describe('ContratacionPComponent', () => {
  let component: ContratacionPComponent;
  let fixture: ComponentFixture<ContratacionPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContratacionPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContratacionPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
