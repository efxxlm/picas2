import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeAnexosComponent } from './tabla-informe-anexos.component';

describe('TablaInformeAnexosComponent', () => {
  let component: TablaInformeAnexosComponent;
  let fixture: ComponentFixture<TablaInformeAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
