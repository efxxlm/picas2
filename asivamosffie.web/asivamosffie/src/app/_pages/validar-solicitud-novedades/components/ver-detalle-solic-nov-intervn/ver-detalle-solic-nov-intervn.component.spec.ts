import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleSolicNovIntervnComponent } from './ver-detalle-solic-nov-intervn.component';

describe('VerDetalleSolicNovIntervnComponent', () => {
  let component: VerDetalleSolicNovIntervnComponent;
  let fixture: ComponentFixture<VerDetalleSolicNovIntervnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleSolicNovIntervnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleSolicNovIntervnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
