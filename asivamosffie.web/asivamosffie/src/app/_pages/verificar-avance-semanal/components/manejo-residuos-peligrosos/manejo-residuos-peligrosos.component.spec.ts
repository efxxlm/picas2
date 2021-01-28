import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoResiduosPeligrososComponent } from './manejo-residuos-peligrosos.component';

describe('ManejoResiduosPeligrososComponent', () => {
  let component: ManejoResiduosPeligrososComponent;
  let fixture: ComponentFixture<ManejoResiduosPeligrososComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoResiduosPeligrososComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoResiduosPeligrososComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
