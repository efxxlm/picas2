import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogSubsanacionComponent } from './dialog-subsanacion.component';

describe('DialogSubsanacionComponent', () => {
  let component: DialogSubsanacionComponent;
  let fixture: ComponentFixture<DialogSubsanacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogSubsanacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogSubsanacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
