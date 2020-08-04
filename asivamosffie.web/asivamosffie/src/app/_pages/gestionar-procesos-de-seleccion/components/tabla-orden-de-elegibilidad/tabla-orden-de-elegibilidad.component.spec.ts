import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOrdenDeElegibilidadComponent } from './tabla-orden-de-elegibilidad.component';

describe('TablaOrdenDeElegibilidadComponent', () => {
  let component: TablaOrdenDeElegibilidadComponent;
  let fixture: ComponentFixture<TablaOrdenDeElegibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOrdenDeElegibilidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOrdenDeElegibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
