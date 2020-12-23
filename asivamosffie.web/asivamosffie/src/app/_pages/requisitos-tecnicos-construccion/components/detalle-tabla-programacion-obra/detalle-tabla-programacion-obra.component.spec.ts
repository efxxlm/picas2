import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTablaProgramacionObraComponent } from './detalle-tabla-programacion-obra.component';

describe('DetalleTablaProgramacionObraComponent', () => {
  let component: DetalleTablaProgramacionObraComponent;
  let fixture: ComponentFixture<DetalleTablaProgramacionObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTablaProgramacionObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTablaProgramacionObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
