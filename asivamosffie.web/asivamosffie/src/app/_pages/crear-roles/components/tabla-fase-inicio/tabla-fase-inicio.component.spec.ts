import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFaseInicioComponent } from './tabla-fase-inicio.component';

describe('TablaFaseInicioComponent', () => {
  let component: TablaFaseInicioComponent;
  let fixture: ComponentFixture<TablaFaseInicioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFaseInicioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFaseInicioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
