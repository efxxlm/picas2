import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarInformacionAdicionalComponent } from './registrar-informacion-adicional.component';

describe('RegistrarInformacionAdicionalComponent', () => {
  let component: RegistrarInformacionAdicionalComponent;
  let fixture: ComponentFixture<RegistrarInformacionAdicionalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarInformacionAdicionalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarInformacionAdicionalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
