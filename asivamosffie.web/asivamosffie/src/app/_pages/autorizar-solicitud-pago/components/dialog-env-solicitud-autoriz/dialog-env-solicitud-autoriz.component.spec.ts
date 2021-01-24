import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogEnvSolicitudAutorizComponent } from './dialog-env-solicitud-autoriz.component';

describe('DialogEnvSolicitudAutorizComponent', () => {
  let component: DialogEnvSolicitudAutorizComponent;
  let fixture: ComponentFixture<DialogEnvSolicitudAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogEnvSolicitudAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogEnvSolicitudAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
