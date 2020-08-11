import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevoComponent } from './registrar-nuevo.component';

describe('RegistrarNuevoComponent', () => {
  let component: RegistrarNuevoComponent;
  let fixture: ComponentFixture<RegistrarNuevoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
