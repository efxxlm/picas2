import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerLiberacionSaldosComponent } from './ver-liberacion-saldos.component';

describe('VerLiberacionSaldosComponent', () => {
  let component: VerLiberacionSaldosComponent;
  let fixture: ComponentFixture<VerLiberacionSaldosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerLiberacionSaldosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerLiberacionSaldosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
