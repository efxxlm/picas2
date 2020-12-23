import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleDiagnosticoComponent } from './detalle-diagnostico.component';

describe('DetalleDiagnosticoComponent', () => {
  let component: DetalleDiagnosticoComponent;
  let fixture: ComponentFixture<DetalleDiagnosticoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleDiagnosticoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleDiagnosticoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
