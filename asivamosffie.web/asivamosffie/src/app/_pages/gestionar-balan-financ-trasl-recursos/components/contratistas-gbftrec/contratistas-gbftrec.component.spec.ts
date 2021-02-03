import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContratistasGbftrecComponent } from './contratistas-gbftrec.component';

describe('ContratistasGbftrecComponent', () => {
  let component: ContratistasGbftrecComponent;
  let fixture: ComponentFixture<ContratistasGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContratistasGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContratistasGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
