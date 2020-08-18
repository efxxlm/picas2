import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistrarOtrosTemasComponent } from './tabla-registrar-otros-temas.component';

describe('TablaRegistrarOtrosTemasComponent', () => {
  let component: TablaRegistrarOtrosTemasComponent;
  let fixture: ComponentFixture<TablaRegistrarOtrosTemasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarOtrosTemasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarOtrosTemasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
