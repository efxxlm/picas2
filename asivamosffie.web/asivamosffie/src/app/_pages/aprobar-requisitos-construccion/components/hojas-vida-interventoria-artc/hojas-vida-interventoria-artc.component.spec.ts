import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HojasVidaInterventoriaArtcComponent } from './hojas-vida-interventoria-artc.component';

describe('HojasVidaInterventoriaArtcComponent', () => {
  let component: HojasVidaInterventoriaArtcComponent;
  let fixture: ComponentFixture<HojasVidaInterventoriaArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HojasVidaInterventoriaArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HojasVidaInterventoriaArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
