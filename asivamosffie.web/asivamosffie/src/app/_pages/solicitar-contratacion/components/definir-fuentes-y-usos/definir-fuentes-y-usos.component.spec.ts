import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefinirFuentesYUsosComponent } from './definir-fuentes-y-usos.component';

describe('DefinirFuentesYUsosComponent', () => {
  let component: DefinirFuentesYUsosComponent;
  let fixture: ComponentFixture<DefinirFuentesYUsosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefinirFuentesYUsosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefinirFuentesYUsosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
