import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SoporteProjectComponent } from './soporte-project.component';

describe('SoporteProjectComponent', () => {
  let component: SoporteProjectComponent;
  let fixture: ComponentFixture<SoporteProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SoporteProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SoporteProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
