import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdenesDiaComponent } from './ordenes-dia.component';

describe('OrdenesDiaComponent', () => {
  let component: OrdenesDiaComponent;
  let fixture: ComponentFixture<OrdenesDiaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdenesDiaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdenesDiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
