import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarOrdenGiroComponent } from './verificar-orden-giro.component';

describe('VerificarOrdenGiroComponent', () => {
  let component: VerificarOrdenGiroComponent;
  let fixture: ComponentFixture<VerificarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
