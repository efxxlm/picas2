import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProgramacionObraComponent } from './tabla-programacion-obra.component';

describe('TablaProgramacionObraComponent', () => {
  let component: TablaProgramacionObraComponent;
  let fixture: ComponentFixture<TablaProgramacionObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProgramacionObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProgramacionObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
