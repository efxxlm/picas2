import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAprobarOrdenGiroComponent } from './tabla-aprobar-orden-giro.component';

describe('TablaAprobarOrdenGiroComponent', () => {
  let component: TablaAprobarOrdenGiroComponent;
  let fixture: ComponentFixture<TablaAprobarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAprobarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAprobarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
