import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BtnRegistrarComponent } from './btn-registrar.component';

describe('BtnRegistrarComponent', () => {
  let component: BtnRegistrarComponent;
  let fixture: ComponentFixture<BtnRegistrarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BtnRegistrarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BtnRegistrarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
