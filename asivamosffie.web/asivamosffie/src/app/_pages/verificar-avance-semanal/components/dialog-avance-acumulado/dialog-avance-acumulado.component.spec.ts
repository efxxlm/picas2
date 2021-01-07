import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogAvanceAcumuladoComponent } from './dialog-avance-acumulado.component';

describe('DialogAvanceAcumuladoComponent', () => {
  let component: DialogAvanceAcumuladoComponent;
  let fixture: ComponentFixture<DialogAvanceAcumuladoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogAvanceAcumuladoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogAvanceAcumuladoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
