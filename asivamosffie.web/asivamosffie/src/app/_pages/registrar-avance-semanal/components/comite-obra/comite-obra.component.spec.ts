import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComiteObraComponent } from './comite-obra.component';

describe('ComiteObraComponent', () => {
  let component: ComiteObraComponent;
  let fixture: ComponentFixture<ComiteObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComiteObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComiteObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
