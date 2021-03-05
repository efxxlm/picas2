import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarReclamacionComponent } from './ver-detalleeditar-reclamacion.component';

describe('VerDetalleeditarReclamacionComponent', () => {
  let component: VerDetalleeditarReclamacionComponent;
  let fixture: ComponentFixture<VerDetalleeditarReclamacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarReclamacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarReclamacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
