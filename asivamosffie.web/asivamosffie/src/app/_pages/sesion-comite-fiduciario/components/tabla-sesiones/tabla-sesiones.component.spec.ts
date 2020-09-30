import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSesionesComponent } from './tabla-sesiones.component';

describe('TablaSesionesComponent', () => {
  let component: TablaSesionesComponent;
  let fixture: ComponentFixture<TablaSesionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSesionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSesionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
