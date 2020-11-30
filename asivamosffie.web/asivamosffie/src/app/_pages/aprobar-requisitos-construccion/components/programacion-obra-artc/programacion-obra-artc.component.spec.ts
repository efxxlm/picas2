import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramacionObraArtcComponent } from './programacion-obra-artc.component';

describe('ProgramacionObraArtcComponent', () => {
  let component: ProgramacionObraArtcComponent;
  let fixture: ComponentFixture<ProgramacionObraArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramacionObraArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramacionObraArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
