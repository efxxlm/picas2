import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaCronogramaComponent } from './tabla-cronograma.component';

describe('TablaCronogramaComponent', () => {
  let component: TablaCronogramaComponent;
  let fixture: ComponentFixture<TablaCronogramaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaCronogramaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
