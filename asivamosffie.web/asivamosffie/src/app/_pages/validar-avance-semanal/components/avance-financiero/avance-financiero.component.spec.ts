import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvanceFinancieroComponent } from './avance-financiero.component';

describe('AvanceFinancieroComponent', () => {
  let component: AvanceFinancieroComponent;
  let fixture: ComponentFixture<AvanceFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvanceFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvanceFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
