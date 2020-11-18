import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramacionPersonalObraComponent } from './programacion-personal-obra.component';

describe('ProgramacionPersonalObraComponent', () => {
  let component: ProgramacionPersonalObraComponent;
  let fixture: ComponentFixture<ProgramacionPersonalObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramacionPersonalObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramacionPersonalObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
