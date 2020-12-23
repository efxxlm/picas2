import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetallePerfilesComponent } from './ver-detalle-perfiles.component';

describe('VerDetallePerfilesComponent', () => {
  let component: VerDetallePerfilesComponent;
  let fixture: ComponentFixture<VerDetallePerfilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetallePerfilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetallePerfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
