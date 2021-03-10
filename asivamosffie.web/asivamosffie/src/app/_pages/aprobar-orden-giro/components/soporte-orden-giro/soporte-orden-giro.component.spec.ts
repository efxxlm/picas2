import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SoporteOrdenGiroComponent } from './soporte-orden-giro.component';

describe('SoporteOrdenGiroComponent', () => {
  let component: SoporteOrdenGiroComponent;
  let fixture: ComponentFixture<SoporteOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SoporteOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SoporteOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
