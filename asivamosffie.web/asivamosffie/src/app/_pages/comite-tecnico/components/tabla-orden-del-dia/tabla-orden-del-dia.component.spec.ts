import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOrdenDelDiaComponent } from './tabla-orden-del-dia.component';

describe('TablaOrdenDelDiaComponent', () => {
  let component: TablaOrdenDelDiaComponent;
  let fixture: ComponentFixture<TablaOrdenDelDiaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOrdenDelDiaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOrdenDelDiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
