import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleCronogramaComponent } from './tabla-detalle-cronograma.component';

describe('TablaDetalleCronogramaComponent', () => {
  let component: TablaDetalleCronogramaComponent;
  let fixture: ComponentFixture<TablaDetalleCronogramaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleCronogramaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
