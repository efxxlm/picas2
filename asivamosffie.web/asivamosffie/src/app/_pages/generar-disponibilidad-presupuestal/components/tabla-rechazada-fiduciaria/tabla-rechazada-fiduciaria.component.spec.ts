import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRechazadaFiduciariaComponent } from './tabla-rechazada-fiduciaria.component';

describe('TablaRechazadaFiduciariaComponent', () => {
  let component: TablaRechazadaFiduciariaComponent;
  let fixture: ComponentFixture<TablaRechazadaFiduciariaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRechazadaFiduciariaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRechazadaFiduciariaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
