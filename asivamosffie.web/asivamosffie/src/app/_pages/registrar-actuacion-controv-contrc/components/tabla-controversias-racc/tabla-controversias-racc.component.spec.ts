import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaControversiasRaccComponent } from './tabla-controversias-racc.component';

describe('TablaControversiasRaccComponent', () => {
  let component: TablaControversiasRaccComponent;
  let fixture: ComponentFixture<TablaControversiasRaccComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaControversiasRaccComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaControversiasRaccComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
