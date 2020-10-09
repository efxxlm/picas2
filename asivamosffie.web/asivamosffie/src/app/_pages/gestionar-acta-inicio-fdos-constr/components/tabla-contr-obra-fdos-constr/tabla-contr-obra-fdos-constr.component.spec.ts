import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContrObraFdosConstrComponent } from './tabla-contr-obra-fdos-constr.component';

describe('TablaContrObraFdosConstrComponent', () => {
  let component: TablaContrObraFdosConstrComponent;
  let fixture: ComponentFixture<TablaContrObraFdosConstrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContrObraFdosConstrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContrObraFdosConstrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
