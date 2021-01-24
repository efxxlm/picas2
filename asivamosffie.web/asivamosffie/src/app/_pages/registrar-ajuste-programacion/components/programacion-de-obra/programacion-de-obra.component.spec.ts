import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramacionDeObraComponent } from './programacion-de-obra.component';

describe('ProgramacionDeObraComponent', () => {
  let component: ProgramacionDeObraComponent;
  let fixture: ComponentFixture<ProgramacionDeObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramacionDeObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramacionDeObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
