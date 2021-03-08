import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TramitarOrdenGiroComponent } from './tramitar-orden-giro.component';

describe('TramitarOrdenGiroComponent', () => {
  let component: TramitarOrdenGiroComponent;
  let fixture: ComponentFixture<TramitarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TramitarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TramitarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
