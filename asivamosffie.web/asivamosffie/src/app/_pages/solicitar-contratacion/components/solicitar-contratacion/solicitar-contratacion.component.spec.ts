import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitarContratacionComponent } from './solicitar-contratacion.component';

describe('SolicitarContratacionComponent', () => {
  let component: SolicitarContratacionComponent;
  let fixture: ComponentFixture<SolicitarContratacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolicitarContratacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolicitarContratacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
