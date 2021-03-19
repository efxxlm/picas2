import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFacturadoOgGtlcComponent } from './tabla-facturado-og-gtlc.component';

describe('TablaFacturadoOgGtlcComponent', () => {
  let component: TablaFacturadoOgGtlcComponent;
  let fixture: ComponentFixture<TablaFacturadoOgGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFacturadoOgGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFacturadoOgGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
