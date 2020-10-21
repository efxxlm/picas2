import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarEnRevisionComponent } from './editar-en-revision.component';

describe('EditarEnRevisionComponent', () => {
  let component: EditarEnRevisionComponent;
  let fixture: ComponentFixture<EditarEnRevisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarEnRevisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarEnRevisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
