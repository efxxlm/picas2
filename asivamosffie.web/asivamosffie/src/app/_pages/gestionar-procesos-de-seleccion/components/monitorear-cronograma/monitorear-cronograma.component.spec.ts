import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MonitorearCronogramaComponent } from './monitorear-cronograma.component';

describe('MonitorearCronogramaComponent', () => {
  let component: MonitorearCronogramaComponent;
  let fixture: ComponentFixture<MonitorearCronogramaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MonitorearCronogramaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MonitorearCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
