import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaMesatrabajoCcComponent } from './registrar-nueva-mesatrabajo-cc.component';

describe('RegistrarNuevaMesatrabajoCcComponent', () => {
  let component: RegistrarNuevaMesatrabajoCcComponent;
  let fixture: ComponentFixture<RegistrarNuevaMesatrabajoCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaMesatrabajoCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaMesatrabajoCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
