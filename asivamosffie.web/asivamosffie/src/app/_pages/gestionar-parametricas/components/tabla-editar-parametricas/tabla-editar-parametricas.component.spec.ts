import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEditarParametricasComponent } from './tabla-editar-parametricas.component';

describe('TablaEditarParametricasComponent', () => {
  let component: TablaEditarParametricasComponent;
  let fixture: ComponentFixture<TablaEditarParametricasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEditarParametricasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEditarParametricasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
