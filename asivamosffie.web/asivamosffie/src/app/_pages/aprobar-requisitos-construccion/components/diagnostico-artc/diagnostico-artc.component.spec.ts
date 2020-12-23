import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiagnosticoArtcComponent } from './diagnostico-artc.component';

describe('DiagnosticoArtcComponent', () => {
  let component: DiagnosticoArtcComponent;
  let fixture: ComponentFixture<DiagnosticoArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiagnosticoArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiagnosticoArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
