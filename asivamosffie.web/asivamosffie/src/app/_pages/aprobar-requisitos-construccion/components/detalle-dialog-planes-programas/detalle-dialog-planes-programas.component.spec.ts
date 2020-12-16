import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleDialogPlanesProgramasComponent } from './detalle-dialog-planes-programas.component';

describe('DetalleDialogPlanesProgramasComponent', () => {
  let component: DetalleDialogPlanesProgramasComponent;
  let fixture: ComponentFixture<DetalleDialogPlanesProgramasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleDialogPlanesProgramasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleDialogPlanesProgramasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
