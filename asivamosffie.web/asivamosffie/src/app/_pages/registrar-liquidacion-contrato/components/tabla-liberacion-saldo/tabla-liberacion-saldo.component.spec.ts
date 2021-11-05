import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TablaLiberacionSaldoComponent } from './tabla-liberacion-saldo.component';


describe('TablaLiberacionSaldoComponent', () => {
  let component: TablaLiberacionSaldoComponent;
  let fixture: ComponentFixture<TablaLiberacionSaldoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaLiberacionSaldoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaLiberacionSaldoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
