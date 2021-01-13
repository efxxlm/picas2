import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleVfspComponent } from './verdetalle-vfsp.component';

describe('VerdetalleVfspComponent', () => {
  let component: VerdetalleVfspComponent;
  let fixture: ComponentFixture<VerdetalleVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
