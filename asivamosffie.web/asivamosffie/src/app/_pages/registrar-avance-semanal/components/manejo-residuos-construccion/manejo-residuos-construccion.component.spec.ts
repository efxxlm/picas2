import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoResiduosConstruccionComponent } from './manejo-residuos-construccion.component';

describe('ManejoResiduosConstruccionComponent', () => {
  let component: ManejoResiduosConstruccionComponent;
  let fixture: ComponentFixture<ManejoResiduosConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoResiduosConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoResiduosConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
