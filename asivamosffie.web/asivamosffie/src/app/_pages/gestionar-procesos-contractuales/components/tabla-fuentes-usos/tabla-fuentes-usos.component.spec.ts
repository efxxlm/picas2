import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFuentesUsosComponent } from './tabla-fuentes-usos.component';

describe('TablaFuentesUsosComponent', () => {
  let component: TablaFuentesUsosComponent;
  let fixture: ComponentFixture<TablaFuentesUsosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFuentesUsosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFuentesUsosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
