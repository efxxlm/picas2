import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeFinalComponent } from './tabla-informe-final.component';

describe('TablaInformeFinalComponent', () => {
  let component: TablaInformeFinalComponent;
  let fixture: ComponentFixture<TablaInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
