import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarDdpComponent } from './gestionar-ddp.component';

describe('GestionarDdpComponent', () => {
  let component: GestionarDdpComponent;
  let fixture: ComponentFixture<GestionarDdpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarDdpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarDdpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
