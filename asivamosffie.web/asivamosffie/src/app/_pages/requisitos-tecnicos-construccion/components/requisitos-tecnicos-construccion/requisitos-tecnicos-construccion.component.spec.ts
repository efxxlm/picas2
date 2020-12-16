import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequisitosTecnicosConstruccionComponent } from './requisitos-tecnicos-construccion.component';

describe('RequisitosTecnicosConstruccionComponent', () => {
  let component: RequisitosTecnicosConstruccionComponent;
  let fixture: ComponentFixture<RequisitosTecnicosConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequisitosTecnicosConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequisitosTecnicosConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
