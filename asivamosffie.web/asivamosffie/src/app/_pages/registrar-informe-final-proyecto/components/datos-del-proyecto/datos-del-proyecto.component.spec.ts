import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosDelProyectoComponent } from './datos-del-proyecto.component';

describe('DatosDelProyectoComponent', () => {
  let component: DatosDelProyectoComponent;
  let fixture: ComponentFixture<DatosDelProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosDelProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosDelProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
