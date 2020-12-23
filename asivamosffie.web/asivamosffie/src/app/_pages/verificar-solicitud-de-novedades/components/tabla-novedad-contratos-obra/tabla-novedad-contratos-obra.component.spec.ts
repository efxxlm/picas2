import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaNovedadContratosObraComponent } from './tabla-novedad-contratos-obra.component';

describe('TablaNovedadContratosObraComponent', () => {
  let component: TablaNovedadContratosObraComponent;
  let fixture: ComponentFixture<TablaNovedadContratosObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaNovedadContratosObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaNovedadContratosObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
