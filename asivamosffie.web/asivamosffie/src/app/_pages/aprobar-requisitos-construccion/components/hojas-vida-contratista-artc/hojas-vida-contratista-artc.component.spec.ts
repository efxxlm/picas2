import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HojasVidaContratistaArtcComponent } from './hojas-vida-contratista-artc.component';

describe('HojasVidaContratistaArtcComponent', () => {
  let component: HojasVidaContratistaArtcComponent;
  let fixture: ComponentFixture<HojasVidaContratistaArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HojasVidaContratistaArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HojasVidaContratistaArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
