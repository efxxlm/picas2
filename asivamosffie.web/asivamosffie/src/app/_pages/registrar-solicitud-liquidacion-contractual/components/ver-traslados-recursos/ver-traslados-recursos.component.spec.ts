import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerTrasladosRecursosComponent } from './ver-traslados-recursos.component';

describe('VerTrasladosRecursosComponent', () => {
  let component: VerTrasladosRecursosComponent;
  let fixture: ComponentFixture<VerTrasladosRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerTrasladosRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerTrasladosRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
