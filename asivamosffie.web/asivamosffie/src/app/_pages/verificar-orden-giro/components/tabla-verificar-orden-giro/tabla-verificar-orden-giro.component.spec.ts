import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaVerificarOrdenGiroComponent } from './tabla-verificar-orden-giro.component';

describe('TablaVerificarOrdenGiroComponent', () => {
  let component: TablaVerificarOrdenGiroComponent;
  let fixture: ComponentFixture<TablaVerificarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaVerificarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaVerificarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
