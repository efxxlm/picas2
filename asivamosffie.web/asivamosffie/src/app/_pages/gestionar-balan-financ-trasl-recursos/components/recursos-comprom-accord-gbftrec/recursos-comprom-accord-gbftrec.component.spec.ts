import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosCompromAccordGbftrecComponent } from './recursos-comprom-accord-gbftrec.component';

describe('RecursosCompromAccordGbftrecComponent', () => {
  let component: RecursosCompromAccordGbftrecComponent;
  let fixture: ComponentFixture<RecursosCompromAccordGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosCompromAccordGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosCompromAccordGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
