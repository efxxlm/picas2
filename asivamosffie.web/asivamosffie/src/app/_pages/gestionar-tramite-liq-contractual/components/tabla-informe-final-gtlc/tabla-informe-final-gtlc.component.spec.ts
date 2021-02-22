import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeFinalGtlcComponent } from './tabla-informe-final-gtlc.component';

describe('TablaInformeFinalGtlcComponent', () => {
  let component: TablaInformeFinalGtlcComponent;
  let fixture: ComponentFixture<TablaInformeFinalGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeFinalGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeFinalGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
