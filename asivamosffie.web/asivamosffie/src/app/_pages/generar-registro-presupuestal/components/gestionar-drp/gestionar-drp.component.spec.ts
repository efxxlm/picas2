import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarDrpComponent } from './gestionar-drp.component';

describe('GestionarDrpComponent', () => {
  let component: GestionarDrpComponent;
  let fixture: ComponentFixture<GestionarDrpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarDrpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarDrpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
