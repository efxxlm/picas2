import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreparacionComponent } from './preparacion.component';

describe('PreparacionComponent', () => {
  let component: PreparacionComponent;
  let fixture: ComponentFixture<PreparacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreparacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreparacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
