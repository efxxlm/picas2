import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSolicitudesContractualesComponent } from './tabla-solicitudes-contractuales.component';

describe('TablaSolicitudesContractualesComponent', () => {
  let component: TablaSolicitudesContractualesComponent;
  let fixture: ComponentFixture<TablaSolicitudesContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSolicitudesContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSolicitudesContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
