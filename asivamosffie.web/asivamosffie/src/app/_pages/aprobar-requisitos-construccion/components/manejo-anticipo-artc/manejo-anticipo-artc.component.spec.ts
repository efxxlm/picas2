import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoAnticipoArtcComponent } from './manejo-anticipo-artc.component';

describe('ManejoAnticipoArtcComponent', () => {
  let component: ManejoAnticipoArtcComponent;
  let fixture: ComponentFixture<ManejoAnticipoArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoAnticipoArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoAnticipoArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
