import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFacturadoOgGbftrecComponent } from './tabla-facturado-og-gbftrec.component';

describe('TablaFacturadoOgGbftrecComponent', () => {
  let component: TablaFacturadoOgGbftrecComponent;
  let fixture: ComponentFixture<TablaFacturadoOgGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFacturadoOgGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFacturadoOgGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
