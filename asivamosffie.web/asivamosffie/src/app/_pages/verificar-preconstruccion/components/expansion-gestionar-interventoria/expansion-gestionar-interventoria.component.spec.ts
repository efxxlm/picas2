import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpansionGestionarInterventoriaComponent } from './expansion-gestionar-interventoria.component';

describe('ExpansionGestionarInterventoriaComponent', () => {
  let component: ExpansionGestionarInterventoriaComponent;
  let fixture: ComponentFixture<ExpansionGestionarInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpansionGestionarInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpansionGestionarInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
