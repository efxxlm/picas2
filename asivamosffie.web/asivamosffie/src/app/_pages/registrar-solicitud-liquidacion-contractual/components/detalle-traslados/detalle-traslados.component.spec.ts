import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTrasladosComponent } from './detalle-traslados.component';

describe('DetalleTrasladosComponent', () => {
  let component: DetalleTrasladosComponent;
  let fixture: ComponentFixture<DetalleTrasladosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTrasladosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTrasladosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
