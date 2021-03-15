import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarParametricasComponent } from './gestionar-parametricas.component';

describe('GestionarParametricasComponent', () => {
  let component: GestionarParametricasComponent;
  let fixture: ComponentFixture<GestionarParametricasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarParametricasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarParametricasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
