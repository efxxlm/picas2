import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarBalanceComponent } from './aprobar-balance.component';

describe('AprobarBalanceComponent', () => {
  let component: AprobarBalanceComponent;
  let fixture: ComponentFixture<AprobarBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
