import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { tablaComentariosActaComponent } from './tabla-comentarios-acta.component';


describe('tablaComentariosActaComponent', () => {
  let component: tablaComentariosActaComponent;
  let fixture: ComponentFixture<tablaComentariosActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ tablaComentariosActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(tablaComentariosActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
