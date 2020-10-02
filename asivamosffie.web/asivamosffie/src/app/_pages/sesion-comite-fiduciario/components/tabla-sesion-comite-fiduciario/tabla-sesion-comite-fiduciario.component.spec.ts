import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TablaSesionComiteFiduciarioComponent } from './tabla-sesion-comite-fiduciario.component';



describe('TablaSesionComiteFiduciarioComponent', () => {
  let component: TablaSesionComiteFiduciarioComponent;
  let fixture: ComponentFixture<TablaSesionComiteFiduciarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSesionComiteFiduciarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSesionComiteFiduciarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
