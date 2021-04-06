import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTablaProgObraComponent } from './detalle-tabla-prog-obra.component';

describe('DetalleTablaProgObraComponent', () => {
  let component: DetalleTablaProgObraComponent;
  let fixture: ComponentFixture<DetalleTablaProgObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTablaProgObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTablaProgObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
