import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitacionCerradaComponent } from './invitacion-cerrada.component';

describe('InvitacionCerradaComponent', () => {
  let component: InvitacionCerradaComponent;
  let fixture: ComponentFixture<InvitacionCerradaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitacionCerradaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitacionCerradaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
