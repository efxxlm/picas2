import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvanceFisicoFinancieroComponent } from './avance-fisico-financiero.component';

describe('AvanceFisicoFinancieroComponent', () => {
  let component: AvanceFisicoFinancieroComponent;
  let fixture: ComponentFixture<AvanceFisicoFinancieroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvanceFisicoFinancieroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvanceFisicoFinancieroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
