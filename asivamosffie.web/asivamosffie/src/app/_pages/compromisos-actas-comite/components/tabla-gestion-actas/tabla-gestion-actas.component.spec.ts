import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionActasComponent } from './tabla-gestion-actas.component';

describe('TablaGestionActasComponent', () => {
  let component: TablaGestionActasComponent;
  let fixture: ComponentFixture<TablaGestionActasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionActasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionActasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
