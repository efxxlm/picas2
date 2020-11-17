import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratoDeObraComponent } from './tabla-contrato-de-obra.component';

describe('TablaContratoDeObraComponent', () => {
  let component: TablaContratoDeObraComponent;
  let fixture: ComponentFixture<TablaContratoDeObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratoDeObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratoDeObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
