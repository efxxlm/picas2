import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRecursosPagadosComponent } from './tabla-recursos-pagados.component';

describe('TablaRecursosPagadosComponent', () => {
  let component: TablaRecursosPagadosComponent;
  let fixture: ComponentFixture<TablaRecursosPagadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRecursosPagadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRecursosPagadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
