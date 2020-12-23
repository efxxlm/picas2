import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDemandantesConvocantesDjComponent } from './form-demandantes-convocantes-dj.component';

describe('FormDemandantesConvocantesDjComponent', () => {
  let component: FormDemandantesConvocantesDjComponent;
  let fixture: ComponentFixture<FormDemandantesConvocantesDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDemandantesConvocantesDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDemandantesConvocantesDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
