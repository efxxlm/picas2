import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarReclamacionAsegCcComponent } from './actualizar-reclamacion-aseg-cc.component';

describe('ActualizarReclamacionAsegCcComponent', () => {
  let component: ActualizarReclamacionAsegCcComponent;
  let fixture: ComponentFixture<ActualizarReclamacionAsegCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarReclamacionAsegCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarReclamacionAsegCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
