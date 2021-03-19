import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeFinalRlcComponent } from './tabla-informe-final-rlc.component';

describe('TablaInformeFinalRlcComponent', () => {
  let component: TablaInformeFinalRlcComponent;
  let fixture: ComponentFixture<TablaInformeFinalRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeFinalRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeFinalRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
