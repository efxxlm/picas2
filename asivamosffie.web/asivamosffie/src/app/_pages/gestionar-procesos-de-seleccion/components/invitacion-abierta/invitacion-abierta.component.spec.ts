import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitacionAbiertaComponent } from './invitacion-abierta.component';

describe('InvitacionAbiertaComponent', () => {
  let component: InvitacionAbiertaComponent;
  let fixture: ComponentFixture<InvitacionAbiertaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitacionAbiertaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitacionAbiertaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
