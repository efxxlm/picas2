import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratoObraArtcComponent } from './tabla-contrato-obra-artc.component';

describe('TablaContratoObraArtcComponent', () => {
  let component: TablaContratoObraArtcComponent;
  let fixture: ComponentFixture<TablaContratoObraArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratoObraArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratoObraArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
