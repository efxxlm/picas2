import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeFinalAnexosComponent } from './tabla-informe-final-anexos.component';

describe('TablaInformeFinalAnexosComponent', () => {
  let component: TablaInformeFinalAnexosComponent;
  let fixture: ComponentFixture<TablaInformeFinalAnexosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeFinalAnexosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeFinalAnexosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
