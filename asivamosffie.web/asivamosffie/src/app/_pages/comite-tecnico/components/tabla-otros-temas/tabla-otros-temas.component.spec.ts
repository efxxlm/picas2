import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOtrosTemasComponent } from './tabla-otros-temas.component';

describe('TablaOtrosTemasComponent', () => {
  let component: TablaOtrosTemasComponent;
  let fixture: ComponentFixture<TablaOtrosTemasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOtrosTemasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOtrosTemasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
