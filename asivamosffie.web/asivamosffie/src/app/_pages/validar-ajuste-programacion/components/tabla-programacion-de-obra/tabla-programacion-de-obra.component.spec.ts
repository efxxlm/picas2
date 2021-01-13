import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProgramacionDeObraComponent } from './tabla-programacion-de-obra.component';

describe('TablaProgramacionDeObraComponent', () => {
  let component: TablaProgramacionDeObraComponent;
  let fixture: ComponentFixture<TablaProgramacionDeObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProgramacionDeObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProgramacionDeObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
