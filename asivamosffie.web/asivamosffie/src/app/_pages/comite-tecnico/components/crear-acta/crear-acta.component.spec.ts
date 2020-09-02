import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearActaComponent } from './crear-acta.component';

describe('CrearActaComponent', () => {
  let component: CrearActaComponent;
  let fixture: ComponentFixture<CrearActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
