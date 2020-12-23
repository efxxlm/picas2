import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContrIntrvnFdosConstrComponent } from './tabla-contr-intrvn-fdos-constr.component';

describe('TablaContrIntrvnFdosConstrComponent', () => {
  let component: TablaContrIntrvnFdosConstrComponent;
  let fixture: ComponentFixture<TablaContrIntrvnFdosConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContrIntrvnFdosConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContrIntrvnFdosConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
