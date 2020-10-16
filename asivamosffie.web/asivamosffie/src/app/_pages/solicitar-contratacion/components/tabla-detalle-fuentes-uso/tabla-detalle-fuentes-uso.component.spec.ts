import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleFuentesUsoComponent } from './tabla-detalle-fuentes-uso.component';

describe('TablaDetalleFuentesUsoComponent', () => {
  let component: TablaDetalleFuentesUsoComponent;
  let fixture: ComponentFixture<TablaDetalleFuentesUsoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleFuentesUsoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleFuentesUsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
