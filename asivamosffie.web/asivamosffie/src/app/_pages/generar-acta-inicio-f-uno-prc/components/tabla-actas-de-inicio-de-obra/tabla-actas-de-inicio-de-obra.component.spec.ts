import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaActasDeInicioDeObraComponent } from './tabla-actas-de-inicio-de-obra.component';

describe('TablaActasDeInicioDeObraComponent', () => {
  let component: TablaActasDeInicioDeObraComponent;
  let fixture: ComponentFixture<TablaActasDeInicioDeObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaActasDeInicioDeObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaActasDeInicioDeObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
