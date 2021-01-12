import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarRevisionNovedadComponent } from './registrar-revision-novedad.component';

describe('RegistrarRevisionNovedadComponent', () => {
  let component: RegistrarRevisionNovedadComponent;
  let fixture: ComponentFixture<RegistrarRevisionNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarRevisionNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarRevisionNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
