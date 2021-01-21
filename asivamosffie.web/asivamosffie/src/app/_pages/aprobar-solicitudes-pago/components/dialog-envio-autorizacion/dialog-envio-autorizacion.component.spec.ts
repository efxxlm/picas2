import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogEnvioAutorizacionComponent } from './dialog-envio-autorizacion.component';

describe('DialogEnvioAutorizacionComponent', () => {
  let component: DialogEnvioAutorizacionComponent;
  let fixture: ComponentFixture<DialogEnvioAutorizacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogEnvioAutorizacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogEnvioAutorizacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
