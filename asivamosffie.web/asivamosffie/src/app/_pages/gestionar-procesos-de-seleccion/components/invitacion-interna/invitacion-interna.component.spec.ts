import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitacionInternaComponent } from './invitacion-interna.component';

describe('InvitacionInternaComponent', () => {
  let component: InvitacionInternaComponent;
  let fixture: ComponentFixture<InvitacionInternaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitacionInternaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitacionInternaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
