import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistrarRequisitosComponent } from './tabla-registrar-requisitos.component';

describe('TablaRegistrarRequisitosComponent', () => {
  let component: TablaRegistrarRequisitosComponent;
  let fixture: ComponentFixture<TablaRegistrarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
