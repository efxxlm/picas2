import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistarReclamacionAseguradoraCcComponent } from './registar-reclamacion-aseguradora-cc.component';

describe('RegistarReclamacionAseguradoraCcComponent', () => {
  let component: RegistarReclamacionAseguradoraCcComponent;
  let fixture: ComponentFixture<RegistarReclamacionAseguradoraCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistarReclamacionAseguradoraCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistarReclamacionAseguradoraCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
