import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleTrasladoGbftrecComponent } from './verdetalle-traslado-gbftrec.component';

describe('VerdetalleTrasladoGbftrecComponent', () => {
  let component: VerdetalleTrasladoGbftrecComponent;
  let fixture: ComponentFixture<VerdetalleTrasladoGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleTrasladoGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleTrasladoGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
