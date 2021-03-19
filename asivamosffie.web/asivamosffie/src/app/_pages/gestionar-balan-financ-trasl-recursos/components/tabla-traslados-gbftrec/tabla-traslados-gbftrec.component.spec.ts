import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaTrasladosGbftrecComponent } from './tabla-traslados-gbftrec.component';

describe('TablaTrasladosGbftrecComponent', () => {
  let component: TablaTrasladosGbftrecComponent;
  let fixture: ComponentFixture<TablaTrasladosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaTrasladosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaTrasladosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
