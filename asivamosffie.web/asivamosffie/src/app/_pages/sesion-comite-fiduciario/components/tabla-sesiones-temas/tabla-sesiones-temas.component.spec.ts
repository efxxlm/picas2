import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSesionesTemasComponent } from './tabla-sesiones-temas.component';

describe('TablaSesionesTemasComponent', () => {
  let component: TablaSesionesTemasComponent;
  let fixture: ComponentFixture<TablaSesionesTemasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSesionesTemasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSesionesTemasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
