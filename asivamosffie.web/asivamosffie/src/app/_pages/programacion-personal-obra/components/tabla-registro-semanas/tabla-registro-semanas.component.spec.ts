import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistroSemanasComponent } from './tabla-registro-semanas.component';

describe('TablaRegistroSemanasComponent', () => {
  let component: TablaRegistroSemanasComponent;
  let fixture: ComponentFixture<TablaRegistroSemanasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistroSemanasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistroSemanasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
