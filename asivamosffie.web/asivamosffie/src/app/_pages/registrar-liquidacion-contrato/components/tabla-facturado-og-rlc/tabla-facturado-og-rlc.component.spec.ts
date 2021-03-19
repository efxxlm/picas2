import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFacturadoOgRlcComponent } from './tabla-facturado-og-rlc.component';

describe('TablaFacturadoOgRlcComponent', () => {
  let component: TablaFacturadoOgRlcComponent;
  let fixture: ComponentFixture<TablaFacturadoOgRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFacturadoOgRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFacturadoOgRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
