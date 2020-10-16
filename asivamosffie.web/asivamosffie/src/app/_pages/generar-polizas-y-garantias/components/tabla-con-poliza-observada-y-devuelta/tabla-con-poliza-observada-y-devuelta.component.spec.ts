import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConPolizaObservadaYDevueltaComponent } from './tabla-con-poliza-observada-y-devuelta.component';

describe('TablaConPolizaObservadaYDevueltaComponent', () => {
  let component: TablaConPolizaObservadaYDevueltaComponent;
  let fixture: ComponentFixture<TablaConPolizaObservadaYDevueltaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConPolizaObservadaYDevueltaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConPolizaObservadaYDevueltaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
