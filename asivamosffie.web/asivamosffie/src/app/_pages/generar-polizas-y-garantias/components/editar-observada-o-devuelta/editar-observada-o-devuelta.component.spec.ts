import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarObservadaODevueltaComponent } from './editar-observada-o-devuelta.component';

describe('EditarObservadaODevueltaComponent', () => {
  let component: EditarObservadaODevueltaComponent;
  let fixture: ComponentFixture<EditarObservadaODevueltaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarObservadaODevueltaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarObservadaODevueltaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
