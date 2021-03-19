import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosCompromPagadosGbftrecComponent } from './recursos-comprom-pagados-gbftrec.component';

describe('RecursosCompromPagadosGbftrecComponent', () => {
  let component: RecursosCompromPagadosGbftrecComponent;
  let fixture: ComponentFixture<RecursosCompromPagadosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosCompromPagadosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosCompromPagadosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
