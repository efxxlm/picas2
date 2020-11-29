import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FlujoInversionRecursosArtcComponent } from './flujo-inversion-recursos-artc.component';

describe('FlujoInversionRecursosArtcComponent', () => {
  let component: FlujoInversionRecursosArtcComponent;
  let fixture: ComponentFixture<FlujoInversionRecursosArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FlujoInversionRecursosArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FlujoInversionRecursosArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
