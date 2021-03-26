import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControversiasComponent } from './controversias.component';

describe('ControversiasComponent', () => {
  let component: ControversiasComponent;
  let fixture: ComponentFixture<ControversiasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControversiasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControversiasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
