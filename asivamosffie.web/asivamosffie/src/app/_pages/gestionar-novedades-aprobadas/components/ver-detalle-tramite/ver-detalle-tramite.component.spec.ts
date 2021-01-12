import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleTramiteComponent } from './ver-detalle-tramite.component';

describe('VerDetalleTramiteComponent', () => {
  let component: VerDetalleTramiteComponent;
  let fixture: ComponentFixture<VerDetalleTramiteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleTramiteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleTramiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
