import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarInformeFinalComponent } from './aprobar-informe-final.component';

describe('AprobarInformeFinalComponent', () => {
  let component: AprobarInformeFinalComponent;
  let fixture: ComponentFixture<AprobarInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
