import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RegistrarSesionComiteFiduciarioComponent } from './registrar-sesion-comite-fiduciario.component';

describe('RegistrarSesionComiteFiduciarioComponent', () => {
  let component: RegistrarSesionComiteFiduciarioComponent;
  let fixture: ComponentFixture<RegistrarSesionComiteFiduciarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarSesionComiteFiduciarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarSesionComiteFiduciarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
