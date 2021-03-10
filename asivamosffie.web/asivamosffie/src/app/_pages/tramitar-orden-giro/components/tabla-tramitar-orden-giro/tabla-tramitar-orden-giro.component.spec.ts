import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaTramitarOrdenGiroComponent } from './tabla-tramitar-orden-giro.component';

describe('TablaTramitarOrdenGiroComponent', () => {
  let component: TablaTramitarOrdenGiroComponent;
  let fixture: ComponentFixture<TablaTramitarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaTramitarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaTramitarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
