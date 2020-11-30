import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRequisitosTecnicosComponent } from './tabla-requisitos-tecnicos.component';

describe('TablaRequisitosTecnicosComponent', () => {
  let component: TablaRequisitosTecnicosComponent;
  let fixture: ComponentFixture<TablaRequisitosTecnicosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRequisitosTecnicosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRequisitosTecnicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
