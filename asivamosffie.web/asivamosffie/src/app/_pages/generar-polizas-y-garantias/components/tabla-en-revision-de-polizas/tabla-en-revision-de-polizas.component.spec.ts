import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEnRevisionDePolizasComponent } from './tabla-en-revision-de-polizas.component';

describe('TablaEnRevisionDePolizasComponent', () => {
  let component: TablaEnRevisionDePolizasComponent;
  let fixture: ComponentFixture<TablaEnRevisionDePolizasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEnRevisionDePolizasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEnRevisionDePolizasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
