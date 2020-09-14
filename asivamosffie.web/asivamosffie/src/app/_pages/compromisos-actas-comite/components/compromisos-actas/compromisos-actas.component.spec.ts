import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompromisosActasComponent } from './compromisos-actas.component';

describe('CompromisosActasComponent', () => {
  let component: CompromisosActasComponent;
  let fixture: ComponentFixture<CompromisosActasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompromisosActasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompromisosActasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
