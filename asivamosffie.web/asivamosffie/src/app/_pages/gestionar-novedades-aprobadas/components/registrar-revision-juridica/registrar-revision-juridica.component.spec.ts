import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarRevisionJuridicaComponent } from './registrar-revision-juridica.component';

describe('RegistrarRevisionJuridicaComponent', () => {
  let component: RegistrarRevisionJuridicaComponent;
  let fixture: ComponentFixture<RegistrarRevisionJuridicaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarRevisionJuridicaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarRevisionJuridicaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
