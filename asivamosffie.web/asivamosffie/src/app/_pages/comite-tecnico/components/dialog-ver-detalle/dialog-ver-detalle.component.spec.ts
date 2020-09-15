import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogVerDetalleComponent } from './dialog-ver-detalle.component';

describe('DialogVerDetalleComponent', () => {
  let component: DialogVerDetalleComponent;
  let fixture: ComponentFixture<DialogVerDetalleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogVerDetalleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogVerDetalleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
