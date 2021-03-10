import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarOrdenGiroComponent } from './aprobar-orden-giro.component';

describe('AprobarOrdenGiroComponent', () => {
  let component: AprobarOrdenGiroComponent;
  let fixture: ComponentFixture<AprobarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
