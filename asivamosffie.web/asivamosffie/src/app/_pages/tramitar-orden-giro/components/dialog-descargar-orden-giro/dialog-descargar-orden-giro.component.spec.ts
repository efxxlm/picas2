import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDescargarOrdenGiroComponent } from './dialog-descargar-orden-giro.component';

describe('DialogDescargarOrdenGiroComponent', () => {
  let component: DialogDescargarOrdenGiroComponent;
  let fixture: ComponentFixture<DialogDescargarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogDescargarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDescargarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
