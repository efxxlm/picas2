import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProcesoFirmasComponent } from './tabla-proceso-firmas.component';

describe('TablaProcesoFirmasComponent', () => {
  let component: TablaProcesoFirmasComponent;
  let fixture: ComponentFixture<TablaProcesoFirmasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProcesoFirmasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProcesoFirmasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
