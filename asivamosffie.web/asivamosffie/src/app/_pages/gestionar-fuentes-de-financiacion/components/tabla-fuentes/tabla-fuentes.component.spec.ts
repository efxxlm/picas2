import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFuentesComponent } from './tabla-fuentes.component';

describe('TablaFuentesComponent', () => {
  let component: TablaFuentesComponent;
  let fixture: ComponentFixture<TablaFuentesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFuentesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFuentesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
