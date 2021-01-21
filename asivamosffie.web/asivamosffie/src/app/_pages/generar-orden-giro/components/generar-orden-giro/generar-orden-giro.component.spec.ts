import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarOrdenGiroComponent } from './generar-orden-giro.component';

describe('GenerarOrdenGiroComponent', () => {
  let component: GenerarOrdenGiroComponent;
  let fixture: ComponentFixture<GenerarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
