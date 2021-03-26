import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FichaProyectoComponent } from './ficha-proyecto.component';

describe('FichaProyectoComponent', () => {
  let component: FichaProyectoComponent;
  let fixture: ComponentFixture<FichaProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FichaProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FichaProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
