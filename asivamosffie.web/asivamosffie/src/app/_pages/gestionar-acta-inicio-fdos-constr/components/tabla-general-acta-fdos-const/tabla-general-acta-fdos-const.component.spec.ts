import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGeneralActaFdosConstComponent } from './tabla-general-acta-fdos-const.component';

describe('TablaGeneralActaFdosConstComponent', () => {
  let component: TablaGeneralActaFdosConstComponent;
  let fixture: ComponentFixture<TablaGeneralActaFdosConstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGeneralActaFdosConstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGeneralActaFdosConstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
