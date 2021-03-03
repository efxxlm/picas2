import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarInformeComponent } from './verificar-informe.component';

describe('VerificarInformeComponent', () => {
  let component: VerificarInformeComponent;
  let fixture: ComponentFixture<VerificarInformeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarInformeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarInformeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
