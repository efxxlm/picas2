import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcesosContractualesComponent } from './procesos-contractuales.component';

describe('ProcesosContractualesComponent', () => {
  let component: ProcesosContractualesComponent;
  let fixture: ComponentFixture<ProcesosContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcesosContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcesosContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
