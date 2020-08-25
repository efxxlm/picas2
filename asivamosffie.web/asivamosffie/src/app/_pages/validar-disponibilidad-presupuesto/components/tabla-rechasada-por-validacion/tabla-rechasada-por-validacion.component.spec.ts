import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRechasadaPorValidacionComponent } from './tabla-rechasada-por-validacion.component';

describe('TablaRechasadaPorValidacionComponent', () => {
  let component: TablaRechasadaPorValidacionComponent;
  let fixture: ComponentFixture<TablaRechasadaPorValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRechasadaPorValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRechasadaPorValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
