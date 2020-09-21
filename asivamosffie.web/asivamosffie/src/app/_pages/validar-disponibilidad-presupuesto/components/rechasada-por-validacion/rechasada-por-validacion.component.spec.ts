import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RechasadaPorValidacionComponent } from './rechasada-por-validacion.component';

describe('RechasadaPorValidacionComponent', () => {
  let component: RechasadaPorValidacionComponent;
  let fixture: ComponentFixture<RechasadaPorValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RechasadaPorValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RechasadaPorValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
