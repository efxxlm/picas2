import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleValidfspComponent } from './verdetalle-validfsp.component';

describe('VerdetalleValidfspComponent', () => {
  let component: VerdetalleValidfspComponent;
  let fixture: ComponentFixture<VerdetalleValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
