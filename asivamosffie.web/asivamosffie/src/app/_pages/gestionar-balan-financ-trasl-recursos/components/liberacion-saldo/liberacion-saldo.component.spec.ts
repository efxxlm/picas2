import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LiberacionSaldoComponent } from './liberacion-saldo.component';

describe('LiberacionSaldoComponent', () => {
  let component: LiberacionSaldoComponent;
  let fixture: ComponentFixture<LiberacionSaldoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiberacionSaldoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiberacionSaldoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
