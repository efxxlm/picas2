import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RevisionAprobacionRapgComponent } from './revision-aprobacion-rapg.component';

describe('RevisionAprobacionRapgComponent', () => {
  let component: RevisionAprobacionRapgComponent;
  let fixture: ComponentFixture<RevisionAprobacionRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RevisionAprobacionRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RevisionAprobacionRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
