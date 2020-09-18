import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevisionActaComponent } from './revision-acta.component';

describe('RevisionActaComponent', () => {
  let component: RevisionActaComponent;
  let fixture: ComponentFixture<RevisionActaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevisionActaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevisionActaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
