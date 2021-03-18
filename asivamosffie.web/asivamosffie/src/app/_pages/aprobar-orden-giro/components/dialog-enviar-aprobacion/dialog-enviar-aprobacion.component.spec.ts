import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogEnviarAprobacionComponent } from './dialog-enviar-aprobacion.component';

describe('DialogEnviarAprobacionComponent', () => {
  let component: DialogEnviarAprobacionComponent;
  let fixture: ComponentFixture<DialogEnviarAprobacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogEnviarAprobacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogEnviarAprobacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
