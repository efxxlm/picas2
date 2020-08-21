import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SesionesConvocadasComponent } from './sesiones-convocadas.component';

describe('SesionesConvocadasComponent', () => {
  let component: SesionesConvocadasComponent;
  let fixture: ComponentFixture<SesionesConvocadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SesionesConvocadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SesionesConvocadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
