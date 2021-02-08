import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleeditarTrasladoGbftrecComponent } from './verdetalleeditar-traslado-gbftrec.component';

describe('VerdetalleeditarTrasladoGbftrecComponent', () => {
  let component: VerdetalleeditarTrasladoGbftrecComponent;
  let fixture: ComponentFixture<VerdetalleeditarTrasladoGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleeditarTrasladoGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleeditarTrasladoGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
